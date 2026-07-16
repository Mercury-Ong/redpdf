# Bookmarks and file attachments

## Bookmarks (outlines)

Give long documents a navigable sidebar:

```csharp
using RedPdf.Manipulation;

byte[] outlined = PdfOperations.AddBookmarks(pdf, new[]
{
    new Bookmark { Title = "Introduction", PageNumber = 1 },
    new Bookmark
    {
        Title = "Financials",
        PageNumber = 3,
        Children =
        [
            new Bookmark { Title = "Revenue", PageNumber = 3 },
            new Bookmark { Title = "Costs", PageNumber = 5 },
        ],
    },
});
```

## File attachments

Embed any file inside the PDF — source data, XML, spreadsheets:

```csharp
byte[] withData = PdfOperations.AttachFile(pdf, "invoice-data.xml", xmlBytes);
```

## E-invoicing (ZUGFeRD / Factur-X)

Combining `AttachFile` with [PDF/A-3 conversion](pdfa.md) is the foundation of
ZUGFeRD/Factur-X e-invoices: a human-readable PDF with the machine-readable XML
invoice embedded in a standards-compliant container.

Bookmarks and attachments are free Community features (PDF/A conversion is licensed).
