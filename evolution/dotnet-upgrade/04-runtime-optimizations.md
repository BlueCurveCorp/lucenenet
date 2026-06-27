# Phase 4: .NET 10 Runtime & Library Optimizations

## 4.1 Span/Memory for String Slicing

**Replace `Substring()` with `ReadOnlySpan<char>` slicing throughout the codebase.**

```
// BEFORE (allocates a new string on every call)
string term = text.Substring(start, length);

// AFTER (zero allocation — span over the original string)
ReadOnlySpan<char> termSpan = text.AsSpan(start, length);
```

**Hot targets:** Tokenization, query parsing, term enumeration.

## 4.2 JIT Devirtualization — Remove Loop Workarounds

.NET 10 JIT devirtualizes array interface methods. Measure and remove manual `for` loops where `foreach` over `IEnumerable<T>` is now as fast.

**Audit:** Search for `for (int i = 0; i < array.Length; i++)` in hot paths. If the loop body doesn't need the index, `foreach` is now zero-cost.

## 4.3 JIT Stack-Allocation — Remove stackalloc Workarounds

.NET 10 JIT stack-allocates small non-escaping arrays created with `new T[]`.

**Specifically:** `BitConverter.GetBytes()` result arrays are now stack-allocated when they don't escape.

```
// Now zero-GC in .NET 10 (JIT stack-allocates the byte[])
void CopyBytes(int value, Span<byte> dest) =>
    BitConverter.GetBytes(value).AsSpan(0, 3).CopyTo(dest);
```

**Audit:** Remove manual `stackalloc` workarounds that existed to avoid the allocation; JIT handles it now.

## 4.4 LINQ Optimizations

Use new .NET 10 LINQ methods:

| Method | Benefit | When |
|--------|---------|------|
| `Order()` / `OrderDescending()` | Less allocation than `OrderBy`/`OrderByDescending` | Simple sort-by-default |
| `TryGetNonEnumeratedCount()` | O(1) count check | Before materializing results |
| `Chunk()` | Array-backed chunking | Batch processing |
| `CountBy()` / `AggregateBy()` | Dictionary-backed O(n) | Facet counting, grouping |

## 4.5 `SearchValues` for Vectorized Character Matching

Replace `IndexOfAny(char[])` with cached `SearchValues<char>`.

```
// BEFORE
private static readonly char[] Delimiters = [' ', '\t', '\n'];
int idx = text.IndexOfAny(Delimiters);

// AFTER
private static readonly SearchValues<char> Delimiters = SearchValues.Create([' ', '\t', '\n']);
int idx = text.AsSpan().IndexOfAny(Delimiters);
```

**Hot targets:** `StandardTokenizerImpl`, all analyzer character classifiers.

## 4.6 SIMD for Numeric Hot Paths

Use `System.Numerics` and `System.Runtime.Intrinsics` for tight numeric loops.

**Candidates:**
- `Lucene.Net.Search.Similarities` — TF-IDF/BM25 scoring
- `Lucene.Net.Util` — numeric encoding/decoding
- `Lucene.Net.Codecs` — compression

.NET 10 supports AVX10.2 and Arm64 SVE — ensure code paths cover both.
