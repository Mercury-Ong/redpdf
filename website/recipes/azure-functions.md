# Azure Functions

A complete working sample (isolated worker, HTTP trigger returning a PDF) lives at
[`samples/RedPdf.AzureFunctionsSample`](https://github.com/Mercury-Ong/redpdf/tree/main/samples/RedPdf.AzureFunctionsSample)
— CI builds the containerized function and verifies a real render on every push.

## The function

```csharp
public class RenderFunction
{
    static readonly Lazy<Task<HtmlToPdfRenderer>> SharedRenderer = new(async () =>
    {
        License.Activate(Environment.GetEnvironmentVariable("REDPDF_LICENSE_KEY")!);
        return await HtmlToPdfRenderer.CreateAsync(new RendererOptions
        {
            // The Azure sandbox needs Chromium run single-process; the preset does that.
            Environment = DeploymentEnvironment.AzureFunctions,
        });
    });

    [Function("render")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData request)
    {
        var html = await new StreamReader(request.Body).ReadToEndAsync();
        var renderer = await SharedRenderer.Value;
        var pdf = await renderer.RenderHtmlAsync(html);

        var response = request.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/pdf");
        await response.Body.WriteAsync(pdf);
        return response;
    }
}
```

## Notes

- **Share one renderer across invocations** — the `Lazy<Task<...>>` above. Cold start
  pays the browser launch once; every invocation after that just opens a tab.
- The `AzureFunctions` preset is a licensed feature (trial keys work). For
  containerized Functions on Premium/Dedicated plans, `LinuxContainer` also works.
- Reference `RedPdf.Chromium.linux-x64` (or set `REDPDF_CHROMIUM_PATH` in a custom
  container) so cold starts never download a browser.
- Renders are capped and time-bounded by default, so a burst of requests queues instead
  of exhausting the plan's memory.
