# Getting started with RedPdf

From zero to your first PDF in about two minutes, on Windows, Linux or macOS.

## 1. Install

```bash
dotnet new console -n MyFirstPdf && cd MyFirstPdf
dotnet add package RedPdf --prerelease
```

## 2. Your first PDF

```csharp
using RedPdf.Rendering;

await using var renderer = await HtmlToPdfRenderer.CreateAsync();
var pdf = await renderer.RenderHtmlAsync("<h1>Hello, RedPdf!</h1>");
File.WriteAllBytes("hello.pdf", pdf);
```

```bash
dotnet run
```

> **First run only:** RedPdf downloads a pinned Chromium build (~150 MB) into a per-user
> cache, so the first render takes a few minutes. Every run after that starts instantly.
> To skip the download entirely, add the browser package for your platform —
> `dotnet add package RedPdf.Chromium.win-x64` (also `linux-x64`, `osx-x64`, `osx-arm64`;
> match the RedPdf version) — or point at an existing install:
> `HtmlToPdfRenderer.CreateAsync(new RendererOptions { ChromiumExecutablePath = "…/chrome" })`

Everything above — and everything in step 3 — is the free Community edition. No key,
no watermark, no page limits.

## 3. The everyday toolbox (all free)

```csharp
using RedPdf.Manipulation;
using RedPdf.Rendering;

// Real-world pages: authed URLs, late JavaScript, screen CSS
var report = await renderer.RenderUrlAsync("https://app.example.com/report/42",
    new PdfRenderOptions
    {
        Cookies = { new RenderCookie { Name = "session", Value = "…" } },
        WaitForSelector = "#chart-ready",
    });

var merged    = PdfOperations.Merge(pdf, report);              // combine documents
var stamped   = PdfOperations.AddTextWatermark(merged, "DRAFT");
var numbered  = PdfOperations.AddPageNumbers(stamped);         // "Page 1 of N"
var branded   = PdfOperations.StampImage(numbered, File.ReadAllBytes("logo.png"),
    new ImageStampOptions { Position = StampPosition.TopRight, Width = 100 });

var thumbnail = PdfRasterizer.ToPng(branded, pageNumber: 1);   // PDF → PNG preview
var text      = PdfTextExtractor.ExtractText(branded);         // PDF → text
var locked    = PdfSecurity.Protect(branded, new ProtectionOptions
    { UserPassword = "s3cret" });                              // AES-256

var fields = PdfForms.ReadFields(formPdf);                     // AcroForms
var filled = PdfForms.FillFields(formPdf, new Dictionary<string, string>
    { ["name"] = "Ada Lovelace" });
var flat   = PdfForms.Flatten(filled);
```

## 4. Try the paid features free for 30 days

Digital signatures, PDF/A archiving and serverless presets need a license key.
Trial keys unlock **everything** for 30 days — request one at the contact in the
README, then activate once at startup:

```csharp
using RedPdf.Licensing;

License.Activate("REDPDF.…your trial key…");

// PAdES signature with a trusted timestamp
var signed = PdfSigner.Sign(pdf, myCertificate, new SignOptions
{
    Reason = "Approved",
    TimestampAuthorityUrl = "http://timestamp.digicert.com",
});

// veraPDF-validated PDF/A-3b, e.g. for e-invoicing / archives
var archival = PdfAConverter.Convert(pdf, new PdfAOptions
{
    Conformance = PdfAConformance.PdfA3b,
});
```

Keys are verified offline — no activation server, no telemetry, nothing phones home.
When the trial lapses, licensed calls throw `LicenseException`; everything in step 3
keeps working forever.

## 5. If something goes wrong

| Symptom | Fix |
|---|---|
| First render is slow / downloads | One-time Chromium fetch — see step 2 note |
| `No usable sandbox!` on Ubuntu 23.10+ | `sudo sysctl -w kernel.apparmor_restrict_unprivileged_userns=0`, or the licensed `LinuxContainer` preset |
| `NotSupportedException` stamping a JPEG | Progressive JPEG — re-save as baseline JPEG or PNG |
| `LicenseException` | The API needs a key (step 4); message names the missing feature |

Full feature matrix and licensing details: see the [README](../README.md).
