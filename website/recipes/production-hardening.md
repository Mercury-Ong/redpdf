# Production hardening

One renderer, shared across your whole app, survives real traffic. The guardrails are
built in — this recipe shows how to tune them.

```csharp
using RedPdf.Rendering;

// Create once at startup, share everywhere (each render runs in its own tab).
await using var renderer = await HtmlToPdfRenderer.CreateAsync(new RendererOptions
{
    MaxConcurrentRenders = 4,   // default: processor count
    RelaunchOnCrash = true,     // default: on
});
```

## Concurrency cap

`MaxConcurrentRenders` bounds in-flight renders; extra calls wait for a free slot.
Chromium tabs are expensive — an unbounded burst can exhaust memory on small containers.
The waiting time counts against each render's `Timeout`.

## Per-render timeout

`Timeout` (default 60 s) bounds each render **end to end** — navigation, waits and
capture. A hung page throws `TimeoutException` and its tab is closed; the renderer stays
healthy:

```csharp
try
{
    pdf = await renderer.RenderUrlAsync(url, new PdfRenderOptions
    {
        Timeout = TimeSpan.FromSeconds(30),
    });
}
catch (TimeoutException)
{
    // log and move on — the renderer is still usable
}
```

## Cancellation

Every render method takes a `CancellationToken`, so you can tie renders to request
aborts:

```csharp
byte[] pdf = await renderer.RenderHtmlAsync(html, options, httpContext.RequestAborted);
```

Caller cancellation surfaces as `OperationCanceledException`; the timeout surfaces as
`TimeoutException` — you can tell them apart.

## Crash recovery

If Chromium crashes or is OOM-killed, the next render relaunches it automatically, and
a render interrupted by the crash is retried once on the fresh browser. Set
`RelaunchOnCrash = false` if you'd rather fail fast and recreate the renderer yourself.
