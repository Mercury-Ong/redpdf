# RedPDF

A cross-platform commercial .NET PDF library.
HTML → PDF via an embedded Chromium engine, plus PDF manipulation — on Windows, Linux
(glibc and musl/Alpine) and macOS, x64 and arm64.

The free **Community** tier covers rendering and manipulation. Paid licenses unlock digital
signatures (with RFC 3161 trusted timestamps), **veraPDF-validated PDF/A** output and
serverless deployment presets — enforced by offline-verified, cryptographically signed
license keys.

📖 **[Documentation & recipes](https://mercury-ong.github.io/redpdf/)** ·
🚀 **[Getting started](docs/GETTING-STARTED.md)** ·
⚖️ **[RedPDF vs IronPDF](docs/REDPDF-VS-IRONPDF.md)**

```bash
dotnet add package RedPdf --prerelease
```

## Quick start

```csharp
// Free Community features — no license needed
await using var renderer = await HtmlToPdfRenderer.CreateAsync();
byte[] invoice = await renderer.RenderHtmlAsync("<h1>Invoice</h1>", new PdfRenderOptions
{
    FooterHtml = "<div style='font-size:8px;text-align:center;width:100%;'>" +
                 "Page <span class='pageNumber'></span> of <span class='totalPages'></span></div>",
});

byte[] merged  = PdfOperations.Merge(invoice, otherPdf);
byte[] stamped = PdfOperations.AddTextWatermark(merged, "CONFIDENTIAL");

byte[] thumbnail = PdfRasterizer.ToPng(stamped, pageNumber: 1, new RasterizeOptions { Dpi = 150 });

// Licensed features — activate once at startup
License.Activate("REDPDF.…");

byte[] signed = PdfSigner.Sign(merged, certificate, new SignOptions
{
    Reason = "Contract approval",
    TimestampAuthorityUrl = "http://timestamp.digicert.com", // RFC 3161 token (PAdES B-T)
});

byte[] archival = PdfAConverter.Convert(merged, new PdfAOptions
{
    Conformance = PdfAConformance.PdfA3b,   // validated PASS by veraPDF
    DocumentTitle = "Invoice",
});
```

## Chromium, your way

The first render downloads a pinned Chromium build (~150 MB) into a per-user cache. Skip
the download entirely by referencing the browser package for your platform:

```bash
dotnet add package RedPdf.Chromium.win-x64   # also: linux-x64, osx-x64, osx-arm64
```

or point `RendererOptions.ChromiumExecutablePath` / the `REDPDF_CHROMIUM_PATH` environment
variable at any existing Chrome/Chromium install (`REDPDF_CHROMIUM_ARGS` appends switches
without a code change).

**Built for production load:** one shared renderer runs each render in its own tab, caps
concurrency (`MaxConcurrentRenders`), bounds every render end to end (`Timeout` →
`TimeoutException`, never a wedged worker), accepts `CancellationToken`s, and relaunches
Chromium automatically if it crashes.

## Working samples

| Sample | What it shows |
| --- | --- |
| [`samples/RedPdf.Sample`](samples/RedPdf.Sample) | End-to-end tour: render, manipulate, sign, PDF/A |
| [`samples/RedPdf.DockerSample`](samples/RedPdf.DockerSample) | HTML → PDF inside a Linux container |
| [`samples/RedPdf.AzureFunctionsSample`](samples/RedPdf.AzureFunctionsSample) | HTTP-triggered PDF function (isolated worker) |

## Feature matrix

| Capability                                   | Community (free) | Professional | Enterprise |
| -------------------------------------------- | :--: | :--: | :--: |
| HTML / URL → PDF (Chromium-accurate)          | ✅ | ✅ | ✅ |
| Embedded Chromium NuGet packages (no runtime download) | ✅ | ✅ | ✅ |
| Headers, footers, page numbers, margins       | ✅ | ✅ | ✅ |
| Authed URLs (cookies/headers), wait-for-selector, viewport | ✅ | ✅ | ✅ |
| Merge / split / watermark                     | ✅ | ✅ | ✅ |
| Password protection & permissions (AES-256)   | ✅ | ✅ | ✅ |
| Text & image extraction                       | ✅ | ✅ | ✅ |
| Stamp text / images / page numbers onto PDFs  | ✅ | ✅ | ✅ |
| Images → PDF · HTML → image · Markdown → PDF  | ✅ | ✅ | ✅ |
| Form create / fill / read / flatten (AcroForms) | ✅ | ✅ | ✅ |
| Bookmarks (nested) & file attachments         | ✅ | ✅ | ✅ |
| PDF → PNG rasterization (PDFium)              | ✅ | ✅ | ✅ |
| Digital signatures (PKCS#7 detached)          | — | ✅ | ✅ |
| RFC 3161 trusted timestamps (PAdES B-T)       | — | ✅ | ✅ |
| PDF/A-2b / PDF/A-3b (veraPDF-validated)       | — | ✅ | ✅ |
| Azure Functions / container presets           | — | — | ✅ |

**Free 30-day trial:** trial keys unlock every paid feature. While the self-service
purchase flow is being built, [open an issue](https://github.com/Mercury-Ong/redpdf/issues)
to request one. 30-day money-back guarantee on all purchases — see
[REFUND-POLICY.md](REFUND-POLICY.md).

## How licensing works

Keys are ECDSA P-256–signed payloads verified **fully offline** against a public key
embedded in the library — no activation server, no telemetry, works air-gapped. Licensed
APIs throw `LicenseException` without a valid key; Community features never require one.

## Support & issues

Bug reports, feature requests and trial key requests:
[github.com/Mercury-Ong/redpdf/issues](https://github.com/Mercury-Ong/redpdf/issues).
