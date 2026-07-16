# Password protection and permissions

Encrypt PDFs with AES and control what readers may do with them.

```csharp
using RedPdf.Manipulation;

byte[] secured = PdfSecurity.Protect(pdf, new ProtectionOptions
{
    UserPassword = "open-me",         // required to open
    OwnerPassword = "admin-only",     // required to change permissions
    Encryption = EncryptionStrength.Aes256,   // default; Aes128 for old readers

    AllowPrinting = true,
    AllowContentExtraction = false,   // block copy/paste
    AllowModification = false,
    AllowFormsFill = true,
});

// Check and remove protection (requires the password)
bool isProtected = PdfSecurity.IsPasswordProtected(secured);
byte[] open = PdfSecurity.Unprotect(secured, "open-me");
```

## Notes

- AES-256 (PDF 2.0) is the default — strongest, needs a reasonably modern reader.
  Use `Aes128` for the broadest compatibility.
- Permissions are advisory *enforcement flags* honored by compliant readers; only the
  encryption itself is cryptographic.
- Free Community feature.
