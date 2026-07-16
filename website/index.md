---
_layout: landing
---

<div class="rp-hero">

<h1>Pixel-perfect PDFs,<br/><span class="rp-accent">straight from HTML.</span></h1>

<p class="rp-sub">RedPdf drives a real Chromium engine inside your .NET app, so documents
come out exactly like Chrome prints them — modern CSS, web fonts, JavaScript charts and
all. Free Community tier for rendering and manipulation; licensed tiers add signatures,
PDF/A and serverless presets.</p>

<div class="rp-install"><span class="rp-prompt">$</span> dotnet add package RedPdf --prerelease</div>

<p class="rp-cta">
  <a class="btn btn-primary btn-lg" href="docs/getting-started.md">Get started</a>
  <a class="btn rp-btn-ghost btn-lg" href="recipes/index.md">Browse recipes</a>
  <a class="btn rp-btn-ghost btn-lg" href="https://github.com/Mercury-Ong/redpdf/blob/main/docs/REDPDF-VS-IRONPDF.md">vs IronPDF</a>
</p>

</div>

<h2 class="rp-section">Two lines to your first PDF</h2>

<p>One renderer, shared app-wide. Every render gets its own browser tab.</p>

```csharp
await using var renderer = await HtmlToPdfRenderer.CreateAsync();
byte[] pdf = await renderer.RenderHtmlAsync("<h1>Hello from RedPdf</h1>");
```

<h2 class="rp-section">Why RedPdf</h2>

<p>Built for the jobs PDF libraries actually get hired for.</p>

<div class="rp-cards">
  <div class="rp-card"><h3><span class="rp-glyph">◆</span>Chromium-accurate</h3>
  <p>What you see in Chrome's print preview is what your users get — flexbox, grid,
  web fonts, <code>@page</code> rules, JS-rendered charts.</p></div>

  <div class="rp-card"><h3><span class="rp-glyph">◆</span>Production-hardened</h3>
  <p>Concurrency caps, end-to-end timeouts, cancellation tokens and automatic Chromium
  crash recovery — built in, not bolted on.</p></div>

  <div class="rp-card"><h3><span class="rp-glyph">◆</span>Runs anywhere</h3>
  <p>Windows, Linux (glibc &amp; Alpine/musl), macOS, x64 &amp; arm64. Docker and Azure
  Functions samples smoke-tested in CI.</p></div>

  <div class="rp-card"><h3><span class="rp-glyph">◆</span>The whole toolbox</h3>
  <p>Merge, split, stamp, watermark, AcroForms, text &amp; image extraction, PDF→PNG,
  AES-256 encryption, bookmarks, attachments.</p></div>

  <div class="rp-card"><h3><span class="rp-glyph">◆</span>Compliance built in</h3>
  <p>veraPDF-validated PDF/A-2b/3b, PKCS#7 signatures and RFC 3161 trusted timestamps
  for archiving and e-invoicing.</p></div>

  <div class="rp-card"><h3><span class="rp-glyph">◆</span>Honest licensing</h3>
  <p>Community tier is free, forever. Paid keys verify offline — no activation server,
  no phone-home, no telemetry. 30-day trial &amp; refunds.</p></div>
</div>

<h2 class="rp-section">Where to start</h2>

<div class="rp-cards">
  <a class="rp-card" href="docs/getting-started.md"><h3>🚀 Getting started</h3>
  <p>Install, first render, and how Chromium is resolved on every platform.</p></a>
  <a class="rp-card" href="recipes/index.md"><h3>🧑‍🍳 Recipes</h3>
  <p>Copy-paste solutions: headers &amp; footers, forms, stamping, extraction, Docker…</p></a>
  <a class="rp-card" href="docs/deployment.md"><h3>📦 Deployment</h3>
  <p>Docker, Azure Functions, and hardening the renderer for production load.</p></a>
  <a class="rp-card" href="api/RedPdf.html"><h3>🔍 API reference</h3>
  <p>Every class and option, generated from the shipped package.</p></a>
</div>
