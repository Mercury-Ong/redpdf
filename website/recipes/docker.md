# Docker

A complete working sample lives at
[`samples/RedPdf.DockerSample`](https://github.com/Mercury-Ong/redpdf/tree/main/samples/RedPdf.DockerSample)
— CI builds the image and verifies a real render on every push.

## Dockerfile

Install a distro Chromium and point RedPdf at it, so the image never downloads a
browser at runtime:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY . .
RUN dotnet publish MyApp -c Release -o /app

FROM mcr.microsoft.com/dotnet/runtime:8.0
RUN apt-get update \
    && apt-get install -y --no-install-recommends chromium fonts-liberation \
    && rm -rf /var/lib/apt/lists/*
ENV REDPDF_CHROMIUM_PATH=/usr/bin/chromium
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "MyApp.dll"]
```

Alternatively, reference `RedPdf.Chromium.linux-x64` and drop the `apt-get` layer — the
browser then ships inside your publish output.

## Renderer options

```csharp
using RedPdf.Licensing;
using RedPdf.Rendering;

// Licensed (or 30-day trial): the supported container preset.
License.Activate(Environment.GetEnvironmentVariable("REDPDF_LICENSE_KEY")!);
var renderer = await HtmlToPdfRenderer.CreateAsync(new RendererOptions
{
    Environment = DeploymentEnvironment.LinuxContainer,
});
```

The preset disables the Chromium sandbox and `/dev/shm` usage — the standard
configuration for hardened Linux containers (Docker, Kubernetes, AWS Lambda).

## Notes

- Install a font package (`fonts-liberation`, `fonts-noto-core`) or text renders as
  boxes.
- Don't set memory limits so low that Chromium gets OOM-killed mid-render; if it does
  happen, RedPdf relaunches the browser automatically on the next render.
