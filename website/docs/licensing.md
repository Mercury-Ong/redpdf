# Licensing

RedPdf uses a simple model: the **Community tier is free** and genuinely useful, and a
handful of advanced features need a paid license key.

## What's in each tier

| Capability | Community (free) | Licensed |
| --- | :--: | :--: |
| HTML / URL / Markdown → PDF (Chromium-accurate) | ✅ | ✅ |
| HTML → image screenshots | ✅ | ✅ |
| Merge, split, stamp, watermark, page numbers | ✅ | ✅ |
| AcroForms: read, fill, flatten, create | ✅ | ✅ |
| Text & image extraction, PDF → PNG rasterization | ✅ | ✅ |
| Password protection & permissions | ✅ | ✅ |
| Bookmarks & file attachments | ✅ | ✅ |
| Embedded Chromium packages (no runtime download) | ✅ | ✅ |
| PKCS#7 digital signatures + RFC 3161 timestamps | — | ✅ |
| PDF/A-2b / PDF/A-3b conversion (veraPDF-validated) | — | ✅ |
| Serverless deployment presets (Azure Functions, containers) | — | ✅ |

## How keys work

Keys are offline — a signed string with the prefix `REDPDF.` that the library verifies
with an embedded public key. No phone-home, no network dependency, works air-gapped.

```csharp
using RedPdf.Licensing;

License.Activate(Environment.GetEnvironmentVariable("REDPDF_LICENSE_KEY")!);
```

Call `License.Activate` once at startup, before using licensed features. Licensed
features throw `LicenseException` when no valid key is active — nothing else changes
behavior.

## Trials

Trial keys unlock **every feature for 30 days**. While the self-service purchase flow
is being built, trial and paid keys are issued directly — open an issue on
[GitHub](https://github.com/Mercury-Ong/redpdf/issues) or watch the repository for the
purchase page.
