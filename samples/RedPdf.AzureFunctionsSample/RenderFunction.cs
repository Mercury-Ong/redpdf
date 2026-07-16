using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using RedPdf.Licensing;
using RedPdf.Rendering;

namespace RedPdf.AzureFunctionsSample;

/// <summary>
/// HTTP-triggered HTML → PDF. GET returns a demo document; POST renders the request body.
/// One renderer is shared across invocations — RedPdf caps concurrent renders, bounds every
/// render with a timeout and relaunches Chromium if it crashes, so no extra plumbing is needed.
/// </summary>
public class RenderFunction
{
    static readonly Lazy<Task<HtmlToPdfRenderer>> SharedRenderer = new(CreateRendererAsync);

    // With a license (or 30-day trial) key in REDPDF_LICENSE_KEY this uses the supported
    // AzureFunctions preset (the Azure sandbox needs Chromium run single-process, which the
    // preset configures). Without one it falls back to hand-passed container switches, which
    // the free Community tier allows — enough for the containerized smoke test.
    static async Task<HtmlToPdfRenderer> CreateRendererAsync()
    {
        var options = new RendererOptions();
        if (Environment.GetEnvironmentVariable("REDPDF_LICENSE_KEY") is { Length: > 0 } licenseKey)
        {
            License.Activate(licenseKey);
            options.Environment = DeploymentEnvironment.AzureFunctions;
        }
        else
        {
            options.AdditionalChromiumArgs =
                ["--no-sandbox", "--disable-setuid-sandbox", "--disable-gpu", "--disable-dev-shm-usage"];
        }
        return await HtmlToPdfRenderer.CreateAsync(options);
    }

    [Function("render")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData request)
    {
        var html = request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase)
            ? await new StreamReader(request.Body).ReadToEndAsync()
            : null;
        if (string.IsNullOrWhiteSpace(html))
            html = """
                <h1>RedPdf on Azure Functions</h1>
                <p>POST your own HTML to this endpoint to convert it to PDF.</p>
                """;

        var renderer = await SharedRenderer.Value;
        var pdf = await renderer.RenderHtmlAsync(html,
            new PdfRenderOptions { Timeout = TimeSpan.FromSeconds(120) });

        var response = request.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/pdf");
        response.Headers.Add("Content-Disposition", "inline; filename=render.pdf");
        await response.Body.WriteAsync(pdf);
        return response;
    }
}
