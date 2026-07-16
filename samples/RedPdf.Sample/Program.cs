using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using RedPdf.Compliance;
using RedPdf.Licensing;
using RedPdf.Manipulation;
using RedPdf.Rendering;
using RedPdf.Signing;

// End-to-end tour of RedPdf. Community features run as-is; the licensed half needs a key:
//   REDPDF_LICENSE_KEY=REDPDF.... dotnet run --project samples/RedPdf.Sample

var outDir = Path.Combine(AppContext.BaseDirectory, "out");
Directory.CreateDirectory(outDir);

// ---- Community features -------------------------------------------------------------

await using var renderer = await HtmlToPdfRenderer.CreateAsync();

var invoice = await renderer.RenderHtmlAsync("""
    <html><head><style>
        body { font-family: -apple-system, 'Segoe UI', Roboto, sans-serif; margin: 0; }
        header { background: #1a237e; color: white; padding: 24px 32px; }
        table { width: 100%; border-collapse: collapse; margin: 32px; }
        td, th { border-bottom: 1px solid #ddd; padding: 8px 12px; text-align: left; }
        .total { font-weight: bold; font-size: 1.2em; }
    </style></head>
    <body>
        <header><h1>Invoice #2026-042</h1><p>Mercuryong &mdash; July 2026</p></header>
        <table>
            <tr><th>Item</th><th>Qty</th><th>Price</th></tr>
            <tr><td>RedPdf Professional license</td><td>2</td><td>$598.00</td></tr>
            <tr><td>Priority support (12 months)</td><td>1</td><td>$199.00</td></tr>
            <tr class="total"><td>Total</td><td></td><td>$797.00</td></tr>
        </table>
    </body></html>
    """,
    new PdfRenderOptions
    {
        FooterHtml = "<div style='font-size:8px;width:100%;text-align:center;color:#888;'>" +
                     "Page <span class='pageNumber'></span> of <span class='totalPages'></span></div>",
    });

var terms = await renderer.RenderHtmlAsync("<h1>Terms &amp; Conditions</h1><p>Payment due in 30 days.</p>");

var merged = PdfOperations.Merge(invoice, terms);
var stamped = PdfOperations.AddTextWatermark(merged, "SPECIMEN");

File.WriteAllBytes(Path.Combine(outDir, "invoice-community.pdf"), stamped);
Console.WriteLine($"[community] rendered + merged + watermarked -> {PdfOperations.GetPageCount(stamped)} pages");

// ---- Licensed features --------------------------------------------------------------

var licenseKey = Environment.GetEnvironmentVariable("REDPDF_LICENSE_KEY");
if (string.IsNullOrWhiteSpace(licenseKey))
{
    Console.WriteLine("[licensed]  skipped - set REDPDF_LICENSE_KEY to demo signing and PDF/A");
    return;
}

License.Activate(licenseKey);
Console.WriteLine($"[licensed]  activated: {License.Current.Licensee}, {License.Current.Edition}, " +
                  $"expires {License.Current.ExpiresUtc:yyyy-MM-dd}");

// Self-signed demo certificate; production code would load a real one from a store or PFX.
using var rsa = RSA.Create(2048);
var request = new CertificateRequest("CN=Mercuryong Demo Signer", rsa,
    HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
using var certificate = request.CreateSelfSigned(
    DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddYears(1));

var signed = PdfSigner.Sign(merged, certificate, new SignOptions { Reason = "Invoice approval" });
File.WriteAllBytes(Path.Combine(outDir, "invoice-signed.pdf"), signed);
Console.WriteLine("[licensed]  digitally signed -> invoice-signed.pdf");

var archival = PdfAConverter.Convert(merged, new PdfAOptions { DocumentTitle = "Invoice #2026-042" });
File.WriteAllBytes(Path.Combine(outDir, "invoice-pdfa.pdf"), archival);
Console.WriteLine("[licensed]  PDF/A metadata applied -> invoice-pdfa.pdf");

Console.WriteLine($"\nOutput written to {outDir}");
