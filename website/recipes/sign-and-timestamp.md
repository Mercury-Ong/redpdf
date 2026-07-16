# Digital signatures and timestamps

> [!IMPORTANT]
> Licensed feature — works with a 30-day trial key. See [Licensing](../docs/licensing.md).

Sign PDFs with PKCS#7 signatures and optionally embed an RFC 3161 trusted timestamp
(PAdES B-T), which proves *when* the document was signed even after the certificate
expires.

```csharp
using System.Security.Cryptography.X509Certificates;
using RedPdf.Licensing;
using RedPdf.Signing;

License.Activate(Environment.GetEnvironmentVariable("REDPDF_LICENSE_KEY")!);

var certificate = new X509Certificate2("signing.pfx", pfxPassword);

byte[] signed = PdfSigner.Sign(pdf, certificate, new SignOptions
{
    Reason = "Contract approval",
    Location = "Singapore",
    TimestampAuthorityUrl = "http://timestamp.digicert.com",  // adds the RFC 3161 token
});
```

## Visible signature box

The signature is visible by default (page 1, bottom-left). Position it, or make it
invisible:

```csharp
new SignOptions
{
    PageIndex = 0,
    BoxX = 360, BoxY = 40, BoxWidth = 200, BoxHeight = 60,
    // or: VisibleSignature = false
}
```

## Notes

- Signatures verify in Adobe Acrobat and other standard readers; trust depends on the
  certificate chain, exactly as with any signing tool.
- Certificates from an HSM or Azure Key Vault: supported via the async signer
  interface — see the API reference for `IDigitalSigner`.
