# Extract text and images

## Text

```csharp
using RedPdf.Manipulation;

string all = PdfTextExtractor.ExtractText(pdf);          // whole document
string p2  = PdfTextExtractor.ExtractText(pdf, 2);       // one page (1-based)
IReadOnlyList<string> perPage = PdfTextExtractor.ExtractPages(pdf);
```

Useful for search indexing, content validation in tests, and feeding LLM pipelines.

## Images

```csharp
foreach (var image in PdfImageExtractor.ExtractImages(pdf))
{
    // image.PageNumber, image.Width, image.Height
    var extension = image.Format == ExtractedImageFormat.Jpeg ? "jpg" : "png";
    await File.WriteAllBytesAsync($"page{image.PageNumber}-{image.Width}x{image.Height}.{extension}",
        image.Data);
}

// Or just one page
var pageImages = PdfImageExtractor.ExtractImages(pdf, pageNumber: 1);
```

## Notes

- JPEGs pass through untouched; other formats are normalized to PNG.
- Exotic compressions (CCITT fax, JBIG2, JPEG 2000) are skipped rather than corrupted.
- Both are free Community features.
