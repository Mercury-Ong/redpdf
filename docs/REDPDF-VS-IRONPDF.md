# RedPDF vs IronPDF — an honest comparison

*Last verified 2026-07-13 against [ironpdf.com](https://ironpdf.com/features/) and
[selectpdf.com](https://selectpdf.com/pricing/). Competitor claims below come from their
public pages on that date. Spot something outdated or wrong? [Open an
issue](https://github.com/Mercury-Ong/redpdf/issues) — we'll fix it fast.*

RedPDF is a cross-platform .NET PDF library with the same shape as IronPDF: pixel-accurate
HTML → PDF through a real Chromium engine, plus the manipulation, forms, security, signing
and PDF/A features that production apps need. It is new (currently `1.0.0-preview`), and
this page doesn't pretend otherwise — see [where IronPDF is ahead](#where-ironpdf-is-ahead-today).
What RedPDF offers that IronPDF doesn't: a **genuinely useful free tier** (IronPDF has
none), **veraPDF-validated** PDF/A output, first-class RFC 3161 trusted timestamps, and
license keys that verify **fully offline** — no activation server, no telemetry.

## TL;DR

|  | RedPDF | IronPDF | SelectPdf |
|---|---|---|---|
| Rendering engine | Chromium (pinned build, or point at your own Chrome) | Chromium-based | WebKit-based |
| Platforms | Windows / Linux / macOS | Windows / Linux / macOS | **Windows only** |
| .NET support | .NET 8+ | .NET Framework 4.6.2+ through .NET 10 | .NET Framework 2.0+ through .NET 10 |
| Free tier | **Rendering + all manipulation, unlimited pages, forever** | None (trial only) | 5 pages max, conversion only |
| Trial | 30 days, every paid feature, offline key | 30 days, full functionality | Watermarked trial DLL |
| Entry price (perpetual) | TBA with 1.0 — roadmap targets below both competitors' entry tiers | $999 (1 dev / 1 location / 1 project) | $499 (1 dev / 1 deployment) |
| Top tier | TBA | $5,999 + $1,999 OEM add-on | $1,599 (Enterprise OEM) |
| Maturity | New — preview on NuGet | Established, large product surface | Since 2014, v26.x |

If you need .NET Framework support or DOCX conversion, use IronPDF — RedPDF doesn't do
either today. For everything below, RedPDF does the same job on every platform, and most
of it is free.

## The 10 things every evaluation checks

Every snippet on this page compiles against the current `RedPdf` package as-is.

### 1. HTML → PDF with headers, footers and page numbers

The renderer is real Chromium, so anything Chrome prints correctly — flexbox, grid, web
fonts, canvas, SVG — renders correctly. Headers and footers are HTML templates with
`pageNumber` / `totalPages` placeholders, same model as IronPDF's `HtmlHeaderFooter`.

```csharp
await using var renderer = await HtmlToPdfRenderer.CreateAsync();

byte[] pdf = await renderer.RenderHtmlAsync(html, new PdfRenderOptions
{
    Paper      = PaperSize.A4,
    MarginTop  = "20mm",
    HeaderHtml = "<div style='font-size:9px;width:100%;text-align:center;'>ACME Corp</div>",
    FooterHtml = "<div style='font-size:9px;width:100%;text-align:center;'>" +
                 "Page <span class='pageNumber'></span> of <span class='totalPages'></span></div>",
});
```

IronPDF equivalent: `new ChromePdfRenderer().RenderHtmlAsPdf(html)`.

### 2. URL → PDF behind authentication

Real dashboards live behind logins. Pass cookies and HTTP headers, then wait for the
JavaScript to actually finish before printing — by CSS selector, not by guessing a delay.

```csharp
byte[] report = await renderer.RenderUrlAsync("https://app.example.com/report/42",
    new PdfRenderOptions
    {
        HttpHeaders     = { ["Authorization"] = "Bearer " + token },
        Cookies         = { new RenderCookie { Name = "session", Value = sid, Domain = "app.example.com" } },
        WaitForSelector = "#chart-ready",          // wait for the SPA to settle
        Media           = CssMediaType.Screen,     // render what the user sees, not print CSS
        ViewportWidth   = 1280,
    });
```

IronPDF equivalent: `RenderUrlAsPdf` with `LoginCredentials` / custom headers.

### 3. Merge and split

Every manipulation API in RedPDF is static and stateless — `byte[]` in, `byte[]` out. No
document objects to track or dispose, which makes pipelines trivially composable.

```csharp
byte[] combined = PdfOperations.Merge(cover, body, appendix);
IReadOnlyList<byte[]> pages = PdfOperations.Split(combined);   // one PDF per page
int count = PdfOperations.GetPageCount(combined);
```

IronPDF equivalent: `PdfDocument.Merge(a, b)`.

### 4. Watermarks and stamps

Diagonal text watermarks, positioned text stamps with `{page}`/`{pages}` placeholders,
image/logo stamps (in front of or behind the content), and one-call page numbers.

```csharp
byte[] marked = PdfOperations.AddTextWatermark(pdf, "CONFIDENTIAL",
    new WatermarkOptions { Opacity = 0.15, RotationDegrees = 45 });

byte[] logoed = PdfOperations.StampImage(marked, logoPng,
    new ImageStampOptions { Position = StampPosition.TopRight, Width = 96 });

byte[] numbered = PdfOperations.AddPageNumbers(logoed);        // "Page {page} of {pages}"
```

IronPDF equivalent: `pdf.ApplyWatermark(...)` and stamper classes.

### 5. Password protection and permissions

AES-256 (or AES-128 for older readers), user/owner passwords, and the full permission set
— printing, copying, modification, form fill, annotations.

```csharp
byte[] locked = PdfSecurity.Protect(pdf, new ProtectionOptions
{
    UserPassword           = "open-me",
    OwnerPassword          = "admin-only",
    Encryption             = EncryptionStrength.Aes256,
    AllowPrinting          = true,
    AllowContentExtraction = false,
    AllowModification      = false,
});

byte[] unlocked = PdfSecurity.Unprotect(locked, "admin-only");
```

IronPDF equivalent: `pdf.SecuritySettings` + `pdf.Password`.

### 6. Forms — fill, flatten, create

Read and fill AcroForm text fields and checkboxes, flatten filled forms into plain page
content, and create new fields on existing documents.

```csharp
var fields = PdfForms.ReadFields(pdf);                          // name → current value

byte[] filled = PdfForms.FillFields(pdf, new Dictionary<string, string>
{
    ["customer_name"] = "Jane Doe",
    ["accept_terms"]  = "true",
});

byte[] final = PdfForms.Flatten(filled);                        // values become page content

byte[] withFields = PdfForms.AddFields(blank,
    new TextFieldSpec { Name = "notes", PageNumber = 1, X = 72, Y = 200, Multiline = true },
    new CheckBoxSpec { Name = "subscribe", PageNumber = 1, X = 72, Y = 260 });
```

IronPDF equivalent: `pdf.Form` field access plus form-creation APIs.

### 7. Text and image extraction

Whole-document or per-page text, and embedded images normalized to PNG (JPEGs pass
through untouched).

```csharp
string allText  = PdfTextExtractor.ExtractText(pdf);
string page2    = PdfTextExtractor.ExtractText(pdf, pageNumber: 2);

foreach (var img in PdfImageExtractor.ExtractImages(pdf))
    File.WriteAllBytes($"p{img.PageNumber}_{img.Width}x{img.Height}.{(img.Format == ExtractedImageFormat.Jpeg ? "jpg" : "png")}", img.Data);
```

IronPDF equivalent: `pdf.ExtractAllText()` / `pdf.ExtractAllImages()`.

### 8. PDF → image and HTML → image

Thumbnails and previews via PDFium rasterization; screenshots (full-page, transparent,
PNG or JPEG) straight from Chromium without an intermediate PDF.

```csharp
byte[] thumb = PdfRasterizer.ToPng(pdf, pageNumber: 1, new RasterizeOptions { Dpi = 150 });
IReadOnlyList<byte[]> pagePngs = PdfRasterizer.ToPngs(pdf);

byte[] screenshot = await renderer.RenderUrlToImageAsync("https://example.com",
    new ImageRenderOptions { Format = ImageFormat.Jpeg, JpegQuality = 85, FullPage = true });
```

IronPDF equivalent: `pdf.ToPngImages(...)` / `RasterizeToImageFiles`.

### 9. Digital signatures with trusted timestamps *(licensed)*

PKCS#7 detached signatures from any `X509Certificate2` (.pfx/.p12 included), with an
optional **RFC 3161 trusted timestamp** (PAdES B-T) in the same call — a feature IronPDF's
public feature page doesn't advertise. The signer interface is async, so external signing
(Azure Key Vault, HSMs) is on the roadmap without an API break.

```csharp
License.Activate("REDPDF.…");   // or a free 30-day trial key

byte[] signed = PdfSigner.Sign(pdf, new X509Certificate2("signing.pfx", pfxPassword),
    new SignOptions
    {
        Reason                = "Contract approval",
        Location              = "Singapore",
        TimestampAuthorityUrl = "http://timestamp.digicert.com",   // RFC 3161 token
    });
```

IronPDF equivalent: `pdf.Sign(new PdfSignature("cert.pfx", password))`.

### 10. PDF/A archival — validated, not just claimed *(licensed)*

Most vendors say "PDF/A support". RedPDF's PDF/A-2b and PDF/A-3b output **passes veraPDF**,
the industry reference validator — and that's enforced by regression tests that run the
real validator in CI, so it stays true release after release. PDF/A-3b allows embedded
files, which combined with `PdfOperations.AttachFile` covers ZUGFeRD / Factur-X e-invoicing.

```csharp
byte[] archival = PdfAConverter.Convert(pdf, new PdfAOptions
{
    Conformance   = PdfAConformance.PdfA3b,
    DocumentTitle = "Invoice 2026-0042",
});
```

IronPDF advertises PDF/A 1–3 (a+b) — a wider matrix (see below) — but does not advertise
independent validation of the output.

### Bonus: Markdown → PDF

```csharp
byte[] pdf = await renderer.RenderMarkdownAsync(File.ReadAllText("CHANGELOG.md"));
```

Tables, task lists and footnotes work out of the box; pass your own CSS to restyle.

## Full feature matrix

✅ free (Community) · ⭕ paid tier / trial · ❌ not available · – not advertised on the
vendor's public feature pages as of July 2026

| Capability | RedPDF | IronPDF |
|---|:--:|:--:|
| HTML string / URL → PDF (Chromium) | ✅ | ⭕ |
| Headers, footers, page numbers | ✅ | ⭕ |
| Authed URLs (cookies / HTTP headers) | ✅ | ⭕ |
| Wait-for-selector, render delay, viewport, print/screen media | ✅ | ⭕ (delays) |
| Merge / split | ✅ | ⭕ |
| Watermarks, text + image stamps | ✅ | ⭕ |
| Images → PDF / PDF → image | ✅ | ⭕ |
| HTML / URL → image | ✅ | ⭕ |
| Text extraction | ✅ | ⭕ |
| Image extraction | ✅ | ⭕ |
| Forms: read / fill / flatten / create | ✅ | ⭕ |
| Bookmarks (nested) + file attachments | ✅ | ⭕ |
| Encryption + permissions (AES-256) | ✅ | ⭕ |
| Markdown → PDF | ✅ | ⭕ |
| Digital signatures (PKCS#7, .pfx/.p12) | ⭕ | ⭕ |
| RFC 3161 trusted timestamps (PAdES B-T) | ⭕ | – |
| PDF/A | ⭕ 2b / 3b, **veraPDF-validated** | ⭕ 1–3 (a+b) |
| Serverless presets (Azure Functions / containers) | ⭕ | guidance docs |
| DOCX / RTF → PDF | ❌ | ⭕ |
| Redaction / find-and-replace text | ❌ (planned) | ⭕ |
| Compression / optimization | ❌ (planned) | ⭕ |
| Tagged PDF / PDF/UA-1 | ❌ (planned) | ⭕ |
| OCR / barcodes | ❌ (out of scope) | separate paid products |
| .NET Framework 4.x | ❌ (.NET 8+) | 4.6.2+ |

Note the pattern in the IronPDF column: **everything requires a paid license or a trial
key** — IronPDF has no free tier. The entire ✅ column in RedPDF is free forever,
unlimited pages, no watermarks.

## Where IronPDF is ahead today

Being honest about this is the point of this page:

- **.NET Framework support.** IronPDF goes back to 4.6.2; RedPDF requires .NET 8+. If
  you're on legacy Framework, IronPDF is the practical choice.
- **DOCX / RTF → PDF.** A deliberate RedPDF non-goal for now (it's a LibreOffice-class
  problem done right).
- **Redaction and find-and-replace text.** On the RedPDF roadmap (find/replace is scoped);
  IronPDF ships both today.
- **Compression / optimization.** Roadmapped for RedPDF; IronPDF ships it.
- **PDF/A breadth and PDF/UA-1.** IronPDF advertises PDF/A 1–3 in both a- and
  b-conformance plus PDF/UA-1. RedPDF ships veraPDF-validated 2b/3b today; A-1b and
  tagged-PDF accessibility are roadmapped.
- **Maturity and surface area.** IronPDF has years of production mileage, a huge docs
  site, and adjacent products. RedPDF is a focused preview — the core engine (Chromium
  rendering, signing, PDF/A) is tested on a Windows/Linux/macOS CI matrix, but the
  ecosystem around it is young.

## Where RedPDF is ahead

- **A real free tier.** Rendering plus every manipulation feature, unlimited pages,
  forever. IronPDF offers only a 30-day trial; after that the minimum spend is $999.
- **Validated compliance, not claimed compliance.** PDF/A output is veraPDF-PASS and
  regression-tested against the real validator on every build.
- **RFC 3161 trusted timestamps** as a first-class, one-property API.
- **Offline, transparent licensing.** Keys are ECDSA-signed payloads verified against a
  public key embedded in the library. No activation server, no phone-home, no telemetry —
  air-gapped environments just work. (IronPDF sells air-gapped development as a $4,999
  add-on.)
- **A stateless, pipeline-friendly API.** Static pure functions over `byte[]` — no
  document object lifetimes, no dispose-ordering bugs, safe to compose.
- **No forced browser downloads.** Reference a `RedPdf.Chromium.<rid>` package (from
  `1.0.0-preview.2`) to embed the browser in your deployment — air-gapped hosts, Docker
  builds and locked-down CI just work — or point `RendererOptions.ChromiumExecutablePath`
  at any installed Chrome/Chromium.

## Pricing

IronPDF (perpetual, verified 2026-07-13): Lite $999 (1 dev/1 location/1 project) ·
Plus $1,499 · Professional $2,999 · Unlimited $5,999 · OEM redistribution +$1,999 ·
air-gapped development +$4,999. One year of updates included, renewals extra.

RedPDF pricing is announced with 1.0. Two commitments we can make now: the Community
tier stays free, and the [roadmap](../ROADMAP.md) direction is to price below both
IronPDF's $999 and SelectPdf's $499 entry tiers. Until then, a 30-day trial key unlocks
every paid feature — offline, no card, no sales call.

## Migrating from IronPDF

| IronPDF | RedPDF |
|---|---|
| `IronPdf.License.LicenseKey = "…"` | `License.Activate("REDPDF.…")` |
| `new ChromePdfRenderer()` | `await HtmlToPdfRenderer.CreateAsync()` |
| `renderer.RenderHtmlAsPdf(html)` | `await renderer.RenderHtmlAsync(html)` |
| `renderer.RenderUrlAsPdf(url)` | `await renderer.RenderUrlAsync(url)` |
| `RenderingOptions.HtmlHeader/HtmlFooter` | `PdfRenderOptions.HeaderHtml/FooterHtml` |
| `PdfDocument.Merge(a, b)` | `PdfOperations.Merge(a, b)` |
| `pdf.ApplyWatermark(html)` | `PdfOperations.AddTextWatermark(pdf, text, …)` |
| `pdf.SecuritySettings` / `pdf.Password` | `PdfSecurity.Protect(pdf, new ProtectionOptions { … })` |
| `pdf.ExtractAllText()` | `PdfTextExtractor.ExtractText(pdf)` |
| `pdf.ExtractAllImages()` | `PdfImageExtractor.ExtractImages(pdf)` |
| `pdf.Form` field access | `PdfForms.ReadFields / FillFields / Flatten / AddFields` |
| `pdf.ToPngImages(…)` | `PdfRasterizer.ToPngs(pdf)` |
| `pdf.Sign(new PdfSignature("cert.pfx", pw))` | `PdfSigner.Sign(pdf, cert, new SignOptions { … })` |

The structural difference: IronPDF hands you a `PdfDocument` object you mutate and save;
RedPDF's manipulation APIs are static transforms over `byte[]`. Migrations mostly consist
of deleting `SaveAs` calls and chaining functions instead.

## Try it

```bash
dotnet add package RedPdf --prerelease
```

Then the [Getting Started guide](GETTING-STARTED.md) goes from install to a signed,
archived PDF in five steps. Everything in the ✅ column above works immediately — no key,
no trial clock, no watermark.

---

*IronPDF is a trademark of Iron Software LLC; SelectPdf is a product of Outside Software
Inc. Neither is affiliated with RedPDF. Comparison based on publicly available information,
verified on the date above; if we got something wrong, [tell
us](https://github.com/Mercury-Ong/redpdf/issues) and we'll correct it.*
