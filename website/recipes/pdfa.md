# PDF/A conversion

> [!IMPORTANT]
> Licensed feature — works with a 30-day trial key. See [Licensing](../docs/licensing.md).

Convert PDFs to PDF/A-2b or PDF/A-3b for long-term archiving, court filings, and
e-invoicing. Output is validated against **veraPDF** — the industry reference
validator — in RedPdf's own test suite, and conformance is treated as a regression if
it ever breaks.

```csharp
using RedPdf.Compliance;
using RedPdf.Licensing;

License.Activate(Environment.GetEnvironmentVariable("REDPDF_LICENSE_KEY")!);

byte[] archival = PdfAConverter.Convert(pdf, new PdfAOptions
{
    Conformance = PdfAConformance.PdfA3b,   // or PdfA2b
    DocumentTitle = "Invoice 2026-042",
});
```

## PDF/A-3 e-invoicing (ZUGFeRD / Factur-X)

PDF/A-3 permits embedded files, which is exactly what ZUGFeRD/Factur-X e-invoices are:

```csharp
byte[] invoice = PdfOperations.AttachFile(pdf, "factur-x.xml", invoiceXml);
byte[] compliant = PdfAConverter.Convert(invoice, new PdfAOptions
{
    Conformance = PdfAConformance.PdfA3b,
});
```

## Notes

- An sRGB output intent is embedded automatically; supply your own ICC profile via
  `PdfAOptions.IccProfile` if you need a specific one.
- Fonts are embedded and metadata is rewritten as XMP as part of conversion — no
  manual preparation needed.
