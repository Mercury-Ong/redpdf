# Merge and split PDFs

```csharp
using RedPdf.Manipulation;

// Merge any number of PDFs, in order
byte[] combined = PdfOperations.Merge(cover, report, appendix);

// Split into one single-page PDF per page
IReadOnlyList<byte[]> pages = PdfOperations.Split(combined);

// Page count without rendering anything
int count = PdfOperations.GetPageCount(combined);
```

## Notes

- Everything is `byte[]` in, `byte[]` out — no temp files, safe to run concurrently.
- Merge preserves page sizes and orientations of the source documents.
- These are free Community features.
