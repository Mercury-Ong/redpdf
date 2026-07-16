# HTML to PDF with headers, footers and page numbers

Render an HTML string (or a URL) to PDF with a repeating footer, custom margins and
paper size.

```csharp
using RedPdf.Rendering;

await using var renderer = await HtmlToPdfRenderer.CreateAsync();

byte[] pdf = await renderer.RenderHtmlAsync(html, new PdfRenderOptions
{
    Paper = PaperSize.A4,                 // A3, A5, Letter, Legal, Tabloid
    Landscape = false,
    MarginTop = "20mm",
    MarginBottom = "18mm",
    FooterHtml = "<div style='font-size:8px;width:100%;text-align:center;'>" +
                 "Page <span class='pageNumber'></span> of <span class='totalPages'></span>" +
                 "</div>",
});
```

For a live page instead of a string:

```csharp
byte[] pdf = await renderer.RenderUrlAsync("https://example.com/invoice/42");
```

## Notes

- Chromium substitutes values into elements with the classes `pageNumber`,
  `totalPages`, `date`, `title` and `url` inside `HeaderHtml`/`FooterHtml`. Setting
  either turns header/footer printing on.
- `PrintBackground` is **on** by default (unlike raw Chromium), so CSS backgrounds show.
- Let the document control its own paper via `@page` CSS with
  `PreferCssPageSize = true`.
- Use `Media = CssMediaType.Screen` to render the page as it looks in a browser window
  instead of through print stylesheets.
- `RenderUrlAsync` waits for the network to go idle; `RenderHtmlAsync` waits for load.
  Both are bounded end to end by `Timeout` (default 60 s).
