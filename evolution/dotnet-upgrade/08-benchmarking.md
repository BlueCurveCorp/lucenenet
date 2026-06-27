# Phase 8: Performance Benchmarking

## Baseline (Before Optimizations)

### Test Suite
```powershell
dotnet test Lucene.Net.sln --configuration Release -f net10.0
```
Record: total time, pass rate, per-project times.

### Microbenchmarks
Create a `benchmarks/` directory with BenchmarkDotNet projects targeting critical paths:

| Benchmark | Module | Metric |
|-----------|--------|--------|
| IndexWriter.AddDocument | Index | docs/sec, allocs/doc |
| IndexSearcher.Search | Search | queries/sec, allocs/query |
| StandardTokenizer.IncrementToken | Analysis | tokens/sec, allocs/token |
| Segment merge | Index | MB/sec, allocs/MB |
| FST compilation | Codecs | builds/sec, memory |

### GC Metrics
```powershell
dotnet trace collect --providers Microsoft-Windows-DotNETRuntime
```
Record: Gen 0/1/2 collections, pause times, allocation rate.

## Post-Optimization (After Each Phase)

```
=== [Phase Name] ===
Before:  X ops/sec, Y MB allocated
After:   Z ops/sec, W MB allocated
Δ:       +A% throughput, -B% allocation
```
