# Recipes

Copy-paste solutions for the jobs PDF libraries actually get used for. Every snippet
compiles against the current `RedPdf` package; the deployment recipes are backed by
samples that CI builds and smoke-tests on every push.

## Rendering

<div class="rp-cards">
  <a class="rp-card" href="html-to-pdf.md"><h3>HTML → PDF</h3>
  <p>Headers, footers, page numbers, margins, paper sizes, print vs screen CSS.</p></a>
  <a class="rp-card" href="authed-pages.md"><h3>Authenticated pages &amp; SPAs</h3>
  <p>Cookies, headers, wait-for-selector and viewports for late JavaScript.</p></a>
  <a class="rp-card" href="markdown-to-pdf.md"><h3>Markdown → PDF</h3>
  <p>CommonMark with tables, task lists and footnotes — restyle with your own CSS.</p></a>
  <a class="rp-card" href="html-to-image.md"><h3>HTML → image</h3>
  <p>PNG/JPEG screenshots: full-page, transparent, quality-tuned.</p></a>
  <a class="rp-card" href="production-hardening.md"><h3>Production hardening</h3>
  <p>Concurrency caps, per-render timeouts, cancellation and crash recovery.</p></a>
</div>

## Deployment

<div class="rp-cards">
  <a class="rp-card" href="docker.md"><h3>Docker</h3>
  <p>A container that never downloads a browser at runtime.</p></a>
  <a class="rp-card" href="azure-functions.md"><h3>Azure Functions</h3>
  <p>An HTTP trigger that returns PDFs, with one shared renderer.</p></a>
</div>

## Manipulation

<div class="rp-cards">
  <a class="rp-card" href="merge-split.md"><h3>Merge &amp; split</h3>
  <p>Combine documents, burst into pages, count pages.</p></a>
  <a class="rp-card" href="stamp-watermark.md"><h3>Stamps &amp; watermarks</h3>
  <p>Page numbers, text stamps, diagonal watermarks, logo images.</p></a>
  <a class="rp-card" href="forms.md"><h3>Fill &amp; flatten forms</h3>
  <p>Read, fill and permanently flatten AcroForms.</p></a>
  <a class="rp-card" href="create-form-fields.md"><h3>Create form fields</h3>
  <p>Turn any PDF into a fillable form: text fields and checkboxes.</p></a>
  <a class="rp-card" href="extract-text-images.md"><h3>Extract text &amp; images</h3>
  <p>Whole-document or per-page text; embedded images as PNG/JPEG.</p></a>
  <a class="rp-card" href="rasterize.md"><h3>PDF → PNG</h3>
  <p>Thumbnails, previews and visual regression baselines via PDFium.</p></a>
  <a class="rp-card" href="password-protection.md"><h3>Password protection</h3>
  <p>AES-128/256 encryption with printing/copy/edit permissions.</p></a>
  <a class="rp-card" href="bookmarks-attachments.md"><h3>Bookmarks &amp; attachments</h3>
  <p>Nested outlines and embedded files (ZUGFeRD/Factur-X ready).</p></a>
</div>

## Compliance <small>(licensed)</small>

<div class="rp-cards">
  <a class="rp-card" href="sign-and-timestamp.md"><h3>Signatures &amp; timestamps</h3>
  <p>PKCS#7 signatures with RFC 3161 trusted timestamps (PAdES B-T).</p></a>
  <a class="rp-card" href="pdfa.md"><h3>PDF/A conversion</h3>
  <p>veraPDF-validated PDF/A-2b and PDF/A-3b for archiving and e-invoicing.</p></a>
</div>
