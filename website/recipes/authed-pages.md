# Authenticated pages and JavaScript-heavy SPAs

Render pages behind a login and pages whose content arrives late via JavaScript.

## Cookies and headers

```csharp
using RedPdf.Rendering;

await using var renderer = await HtmlToPdfRenderer.CreateAsync();

byte[] pdf = await renderer.RenderUrlAsync("https://app.example.com/report/7", new PdfRenderOptions
{
    HttpHeaders = { ["Authorization"] = $"Bearer {token}" },
    Cookies = { new RenderCookie { Name = "session", Value = sessionId } },
});
```

Cookie domains are inferred from the URL; when rendering an HTML *string* with cookies,
set `RenderCookie.Domain` explicitly (there is no URL to infer it from).

## Wait for late content

```csharp
byte[] pdf = await renderer.RenderHtmlAsync(html, new PdfRenderOptions
{
    WaitForSelector = "#chart-ready",          // wait for a CSS selector to appear…
    RenderDelay = TimeSpan.FromMilliseconds(300), // …then a settle delay for web fonts
});
```

## Viewport-dependent layouts

Responsive pages and JS that reads `window.innerWidth` see the viewport you set:

```csharp
byte[] pdf = await renderer.RenderHtmlAsync(html, new PdfRenderOptions
{
    ViewportWidth = 1280,
    ViewportHeight = 720,
});
```

## Notes

- All of these waits count against the render's `Timeout` (default 60 s), so a selector
  that never appears fails the render instead of hanging it.
