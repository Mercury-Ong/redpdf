# PDF to PNG

Rasterize pages to images for thumbnails, previews and visual regression tests —
PDFium-backed, no browser involved.

```csharp
using RedPdf.Rendering;

// One page (1-based)
byte[] png = PdfRasterizer.ToPng(pdf, pageNumber: 1);

// Every page
IReadOnlyList<byte[]> pages = PdfRasterizer.ToPngs(pdf);

// Tuned
byte[] thumb = PdfRasterizer.ToPng(pdf, 1, new RasterizeOptions
{
    Dpi = 72,                 // default 150
    Transparent = true,       // no white background
    Password = "s3cret",      // for protected documents
    IncludeAnnotations = true,
});

// A page range
IReadOnlyList<byte[]> firstThree = PdfRasterizer.ToPngs(pdf, new RasterizeOptions
{
    FromPage = 1,
    ToPage = 3,
});
```

## Visual regression testing

Rasterize at a fixed DPI and compare bytes (or pixels) against a checked-in baseline —
a cheap way to catch layout regressions in generated documents.

Free Community feature.
