# Phase 5: Memory & GC Optimization

## 5.1 ArrayPool in Hot Paths

Replace repeated `new byte[X]` / `new char[X]` in hot loops with pooled arrays.

```
byte[] buffer = ArrayPool<byte>.Shared.Rent(8192);
try {
    // use buffer
} finally {
    ArrayPool<byte>.Shared.Return(buffer);
}
```

**Targets:** Codecs (compression buffers), Store (file I/O), Analysis (char buffers).

## 5.2 ValueTask for Async Hot Paths

Convert `async Task<T>` to `async ValueTask<T>` where synchronous completion is common — typical in buffered I/O paths.

**Target:** `Lucene.Net.Store` (RAM-based directories), `Lucene.Net.Index` (cached lookups).

## 5.3 Struct Conversion for Hot-Path Types

Convert reference types that are created at high frequency to `readonly struct` or `readonly record struct`.

**Candidates:**
- `Term` — created per unique term
- `DocsEnum` implementation — created per segment
- `Scorer` subclasses — created per query per segment

**Rule of thumb:** If a type is smaller than ~48 bytes and created thousands of times per operation, convert to struct.

## 5.4 Avoid LOH Allocations

Keep individual allocations under 85 KB to avoid Gen 2 GC promotion.

- Large byte arrays → `ArrayPool<byte>`
- Large char arrays → `ArrayPool<char>`
- Large collections → pre-size with `List<T>(capacity)`
