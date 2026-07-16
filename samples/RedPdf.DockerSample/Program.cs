using RedPdf.Licensing;
using RedPdf.Rendering;

// HTML → PDF inside a Linux container. The Dockerfile installs Debian's Chromium and points
// REDPDF_CHROMIUM_PATH at it, so the image never downloads a browser at runtime.
//
// With a license (or 30-day trial) key in REDPDF_LICENSE_KEY this uses the supported
// LinuxContainer preset. Without one it falls back to passing the container Chromium switches
// by hand, which the free Community tier allows — the preset is the tested, supported path.

var options = new RendererOptions();
if (Environment.GetEnvironmentVariable("REDPDF_LICENSE_KEY") is { Length: > 0 } licenseKey)
{
    License.Activate(licenseKey);
    options.Environment = DeploymentEnvironment.LinuxContainer;
}
else
{
    options.AdditionalChromiumArgs =
        ["--no-sandbox", "--disable-setuid-sandbox", "--disable-gpu", "--disable-dev-shm-usage"];
}

await using var renderer = await HtmlToPdfRenderer.CreateAsync(options);

var pdf = await renderer.RenderHtmlAsync("""
    <html><head><style>
        body { font-family: 'Liberation Sans', sans-serif; margin: 0; }
        header { background: #b71c1c; color: white; padding: 24px 32px; }
        main { padding: 32px; }
    </style></head>
    <body>
        <header><h1>RedPdf in Docker</h1></header>
        <main>
            <p>This PDF was rendered by Chromium inside a Linux container.</p>
            <p>Machine: <b id="os"></b></p>
            <script>document.getElementById('os').textContent = navigator.platform;</script>
        </main>
    </body></html>
    """,
    new PdfRenderOptions
    {
        Timeout = TimeSpan.FromSeconds(120),
        FooterHtml = "<div style='font-size:8px;width:100%;text-align:center;color:#888;'>" +
                     "Page <span class='pageNumber'></span> of <span class='totalPages'></span></div>",
    });

if (pdf.Length < 4 || pdf[0] != '%' || pdf[1] != 'P' || pdf[2] != 'D' || pdf[3] != 'F')
{
    Console.Error.WriteLine("Render produced something that is not a PDF.");
    return 1;
}

var outDir = Environment.GetEnvironmentVariable("REDPDF_OUT_DIR") ?? "out";
Directory.CreateDirectory(outDir);
var outPath = Path.Combine(outDir, "sample.pdf");
await File.WriteAllBytesAsync(outPath, pdf);
Console.WriteLine($"Wrote {pdf.Length:N0} bytes to {outPath}");
return 0;
