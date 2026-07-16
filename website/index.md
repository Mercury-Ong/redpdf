---
_layout: landing
---

# RedPdf

**Chromium-accurate HTML to PDF conversion and PDF manipulation for .NET** — Windows,
Linux (glibc and musl), macOS, x64 and arm64. Free Community tier; licensed tiers add
digital signatures, PDF/A and serverless deployment presets.

```bash
dotnet add package RedPdf --prerelease
```

```csharp
await using var renderer = await HtmlToPdfRenderer.CreateAsync();
byte[] pdf = await renderer.RenderHtmlAsync("<h1>Hello from RedPdf</h1>");
await File.WriteAllBytesAsync("hello.pdf", pdf);
```

## Why RedPdf

- **Pixel-accurate rendering** — an embedded Chromium prints your HTML exactly like
  Chrome does: modern CSS, web fonts, JavaScript charts, `@page` rules.
- **Built for production load** — concurrent renders are capped, every render has an
  end-to-end timeout and cancellation, and a crashed Chromium relaunches automatically.
- **No runtime downloads if you don't want them** — `RedPdf.Chromium.<rid>` packages
  embed the browser in your build output for air-gapped hosts, Docker and serverless.
- **The whole PDF toolbox** — merge, split, stamp, watermark, forms, text/image
  extraction, rasterization, password protection, bookmarks, attachments.
- **Compliance when you need it** — veraPDF-validated PDF/A-2b/3b and PKCS#7 signatures
  with RFC 3161 timestamps in the licensed tiers.

## Where to start

- [Getting started](docs/getting-started.md) — install, first render, how Chromium is resolved.
- [Recipes](recipes/index.md) — copy-paste solutions for the common jobs.
- [Deployment](docs/deployment.md) — Docker, Azure Functions, hardening for load.
- [Licensing](docs/licensing.md) — what's free, what's paid, how keys work.
