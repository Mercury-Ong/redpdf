# HTML to image (PNG / JPEG)

Capture HTML or a URL as an image, like a browser screenshot — useful for thumbnails,
social cards and email-safe previews.

```csharp
using RedPdf.Rendering;

await using var renderer = await HtmlToPdfRenderer.CreateAsync();

byte[] png = await renderer.RenderHtmlToImageAsync(html, new ImageRenderOptions
{
    ViewportWidth = 1200,      // the output width
    FullPage = true,           // capture beyond the viewport height
});

byte[] jpeg = await renderer.RenderUrlToImageAsync("https://example.com", new ImageRenderOptions
{
    Format = ImageFormat.Jpeg,
    JpegQuality = 85,
});
```

## Transparent backgrounds

```csharp
byte[] png = await renderer.RenderHtmlToImageAsync(
    "<div style='color:white;font-size:48px'>Overlay text</div>",
    new ImageRenderOptions { TransparentBackground = true });
```

## Notes

- Images always render with **screen** CSS (like a browser window), not print styles.
- The viewport width is the image width; `FullPage = true` extends the height to the
  full document.
