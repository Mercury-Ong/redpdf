# Deployment

RedPdf runs anywhere .NET 8 runs. The moving part is Chromium — this page covers giving
it a browser and keeping it healthy under production load.

## Give the deployment a browser

Reference the Chromium package for your runtime identifier so the browser ships inside
your build output and nothing is downloaded at runtime:

```bash
dotnet add package RedPdf.Chromium.linux-x64 --prerelease   # match your RID
```

Or point RedPdf at a browser you install yourself (e.g. a distro package) with the
`REDPDF_CHROMIUM_PATH` environment variable — no code change needed. Extra Chromium
switches can be appended the same way via `REDPDF_CHROMIUM_ARGS`.

## Docker

A complete, CI-smoke-tested sample lives at
[`samples/RedPdf.DockerSample`](https://github.com/Mercury-Ong/redpdf/tree/main/samples/RedPdf.DockerSample).
The essentials:

```dockerfile
FROM mcr.microsoft.com/dotnet/runtime:8.0
RUN apt-get update \
    && apt-get install -y --no-install-recommends chromium fonts-liberation \
    && rm -rf /var/lib/apt/lists/*
ENV REDPDF_CHROMIUM_PATH=/usr/bin/chromium
```

```csharp
var options = new RendererOptions
{
    // Licensed (or 30-day trial): the supported container preset
    Environment = DeploymentEnvironment.LinuxContainer,
};
```

## Azure Functions

A complete sample (isolated worker, HTTP trigger returning a PDF) lives at
[`samples/RedPdf.AzureFunctionsSample`](https://github.com/Mercury-Ong/redpdf/tree/main/samples/RedPdf.AzureFunctionsSample).
On Azure, use the `AzureFunctions` preset — the Azure sandbox requires Chromium to run
single-process, which the preset configures:

```csharp
License.Activate(Environment.GetEnvironmentVariable("REDPDF_LICENSE_KEY")!);
var renderer = await HtmlToPdfRenderer.CreateAsync(new RendererOptions
{
    Environment = DeploymentEnvironment.AzureFunctions,
});
```

Share one renderer across invocations (a `Lazy<Task<HtmlToPdfRenderer>>` works well).

## Hardening under load

Everything here is built in — see the
[production hardening recipe](../recipes/production-hardening.md) for details:

- `RendererOptions.MaxConcurrentRenders` caps in-flight renders (default: processor count).
- `PdfRenderOptions.Timeout` bounds each render end to end; a hung page throws
  `TimeoutException` instead of wedging a worker.
- Every render method takes a `CancellationToken`.
- A crashed or OOM-killed Chromium is relaunched automatically
  (`RendererOptions.RelaunchOnCrash`, on by default).

## Platform coverage

Every push to `main` runs rendering integration tests on Windows, Ubuntu, macOS,
linux-arm64 and Alpine (musl), plus Docker and Azure Functions sample smoke tests that
build the images and verify a real render.
