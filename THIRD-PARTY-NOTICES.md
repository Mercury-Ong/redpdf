# Third-party notices

RedPDF incorporates or depends on the following open-source components:

| Component | License | Use |
| --- | --- | --- |
| [PDFsharp](https://github.com/empira/PDFsharp) | MIT | PDF document model, manipulation, signature handler |
| [PuppeteerSharp](https://github.com/hardkoded/puppeteer-sharp) | MIT | Chromium automation for HTML-to-PDF rendering |
| Chromium ("Chrome for Testing") | BSD-3-Clause | Rendering engine, downloaded at runtime from Google's CDN |
| Chromium (open-source snapshot builds) | BSD-3-Clause + bundled third-party licenses | Redistributed in the optional `RedPdf.Chromium.<rid>` packages (Chrome for Testing binaries are not redistributable) |
| [Compact-ICC-Profiles](https://github.com/saucecontrol/Compact-ICC-Profiles) sRGB-v4.icc | CC0 1.0 | Bundled sRGB output-intent profile for PDF/A |
| [PdfPig](https://github.com/UglyToad/PdfPig) | Apache-2.0 | Text extraction |
| [PDFium](https://pdfium.googlesource.com/pdfium/) via [pdfium-binaries](https://github.com/bblanchon/pdfium-binaries) | BSD-3-Clause | PDF-to-image rasterization |
| [Markdig](https://github.com/xoofx/markdig) | BSD-2-Clause | Markdown-to-PDF conversion |
| System.Security.Cryptography.Pkcs | MIT | PKCS#7 / CMS signatures, RFC 3161 timestamps |
