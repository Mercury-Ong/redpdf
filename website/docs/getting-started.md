# Getting started

## Install

```bash
dotnet add package RedPdf --prerelease
```

RedPdf targets .NET 8 and runs on Windows, Linux (glibc and musl/Alpine) and macOS,
on x64 and arm64.

## First render

```csharp
using RedPdf.Rendering;

await using var renderer = await HtmlToPdfRenderer.CreateAsync();

byte[] pdf = await renderer.RenderHtmlAsync("""
    <h1>Hello from RedPdf</h1>
    <p>Rendered by Chromium, so it looks exactly like Chrome's print preview.</p>
    """);

await File.WriteAllBytesAsync("hello.pdf", pdf);
```

Create **one renderer and reuse it** — each render runs in its own browser tab, and the
renderer manages concurrency, timeouts and crash recovery for you
(see [Production hardening](../recipes/production-hardening.md)).

## How Chromium is resolved

The renderer looks for a browser in this order:

1. `RendererOptions.ChromiumExecutablePath` — an explicit path in code.
2. The `REDPDF_CHROMIUM_PATH` environment variable — an explicit path in config.
3. A referenced `RedPdf.Chromium.<rid>` package in your build output — no download,
   works air-gapped:
   ```bash
   dotnet add package RedPdf.Chromium.win-x64 --prerelease
   # also: linux-x64, osx-x64, osx-arm64 — install the same version as RedPdf
   ```
4. A pinned Chromium build downloaded once (~150 MB) into a per-user cache.

For servers, containers and CI, option 2 or 3 is recommended so cold starts never
download a browser.

> [!NOTE]
> **Ubuntu 23.10+**: stock Ubuntu restricts unprivileged user namespaces via AppArmor,
> which blocks Chromium's sandbox (`No usable sandbox!`). Either re-enable them with
> `sudo sysctl -w kernel.apparmor_restrict_unprivileged_userns=0` or use the licensed
> `DeploymentEnvironment.LinuxContainer` preset.

## Next steps

- [Recipes](../recipes/index.md) — headers/footers, forms, stamping, extraction and more.
- [Deployment](deployment.md) — Docker, Azure Functions, hardening under load.
- [Licensing](licensing.md) — the Community tier is free; signatures, PDF/A and
  serverless presets need a key.
