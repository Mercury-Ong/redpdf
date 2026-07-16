# Markdown to PDF

Render CommonMark — plus tables, task lists and footnotes — with clean default styling.

```csharp
using RedPdf.Rendering;

await using var renderer = await HtmlToPdfRenderer.CreateAsync();

byte[] pdf = await renderer.RenderMarkdownAsync("""
    # Quarterly report

    | Region | Revenue |
    |--------|--------:|
    | EMEA   | $1.2M   |
    | APAC   | $0.9M   |

    - [x] Reviewed by finance
    - [ ] Board approval
    """);
```

## Custom styling

Pass CSS to replace the built-in stylesheet entirely:

```csharp
byte[] pdf = await renderer.RenderMarkdownAsync(markdown, css: """
    body { font-family: Georgia, serif; max-width: 46em; margin: auto; }
    h1 { color: #b71c1c; }
    table { border-collapse: collapse; }
    td, th { border: 1px solid #ccc; padding: 4px 10px; }
    """);
```

All `PdfRenderOptions` (headers/footers, paper, margins) apply as usual via the
`options` parameter.
