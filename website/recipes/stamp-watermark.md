# Stamps, watermarks and page numbers on existing PDFs

Add text or images onto PDFs you didn't render — scans, uploads, third-party documents.

## Page numbers

```csharp
using RedPdf.Manipulation;

byte[] numbered = PdfOperations.AddPageNumbers(pdf);                      // "Page 1 of 9"
byte[] custom   = PdfOperations.AddPageNumbers(pdf, "{page} / {pages}");
```

## Text stamps

```csharp
byte[] stamped = PdfOperations.StampText(pdf, "CONFIDENTIAL — {page} of {pages}",
    new StampOptions
    {
        Position = StampPosition.TopRight,   // nine positions
        FontSize = 9,
        Red = 183, Green = 28, Blue = 28,
        FromPage = 2,                        // optional page range
    });
```

## Diagonal watermark

```csharp
byte[] watermarked = PdfOperations.AddTextWatermark(pdf, "DRAFT", new WatermarkOptions
{
    Opacity = 0.15,
    RotationDegrees = 45,
    FontSize = 72,
});
```

## Image stamps (logos, signatures scans)

```csharp
byte[] logo = await File.ReadAllBytesAsync("logo.png");   // PNG or baseline JPEG
byte[] branded = PdfOperations.StampImage(pdf, logo, new ImageStampOptions
{
    Position = StampPosition.BottomRight,
});
```

## Images to PDF

```csharp
byte[] album = PdfOperations.ImagesToPdf(scan1, scan2, scan3);  // one page per image
```

All free Community features. `{page}` and `{pages}` placeholders work in any stamp text.
