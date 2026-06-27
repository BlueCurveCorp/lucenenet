# .NET 10 Upgrade & Optimization Plan

## Decision: .NET 10 Only, Zero Backward Compatibility

We are dropping **all** pre-.NET 10 targets. No `net8.0`, no `netstandard2.0`, no `net462`. The entire codebase moves to `net10.0` exclusively.

This means:

- **No** `#if NET10_0_OR_GREATER` conditionals — just write .NET 10 code directly
- **No** `System.Memory` shim packages — spans are in-box
- **No** feature-flag constants for older runtimes — all code assumes .NET 10 APIs
- **No** `Microsoft.Bcl.Memory` polyfill — use the runtime's built-in types directly
- **All** `.AsSpan()` calls become implicit conversions
- **All** APIs can use `Span<T>`, `ReadOnlySpan<T>`, `Memory<T>` directly

## C# (csharp) references

- C# 14: `https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14`

## Plan Overview

| Phase | Focus | Plan |
|-------|-------|-------|
| **Phase 1** | Build infrastructure — SDK version, global.json, VS checks | `01-build-infrastructure.md` |
| **Phase 2** | Target frameworks — drop everything except `net10.0` | `02-target-framework-strategy.md` |
| **Phase 3** | C# 14 language features — adopt across the codebase | `03-csharp14-language-features.md` |
| **Phase 4** | .NET 10 runtime & library optimizations — spans, devirtualization, SearchValues, LINQ | `04-runtime-optimizations.md` |
| **Phase 5** | Memory & GC optimization — ArrayPool, ValueTask, struct conversion | `05-memory-gc-optimizations.md` |
| **Phase 6** | Build configuration — TieredPGO, R2R for tools | `06-pgo-and-r2r.md` |
| **Phase 7** | CI/CD pipeline — simplify to single-TFM matrix | `07-ci-cd-updates.md` |
| **Phase 8** | Performance benchmarking — baseline + post-opt comparison | `08-benchmarking.md` |

---

## Migration Checklist

### Infrastructure

- [ ] Set `minimumSdkVersion` in `runbuild.ps1` to `10.0.301`
- [ ] Pin SDK version in `global.json`
- [ ] Simplify `Directory.Build.props` — single TFM, remove VS version checks
- [ ] Strip `Directory.Build.targets` — remove all pre-net10 feature flags
- [ ] Remove conditional package references from all `.csproj` files
- [ ] Remove `TestTargetFramework.props` backward-compat fallbacks
- [ ] Update all GitHub workflow `setup-dotnet` actions
- [ ] Bump test SDK and remaining package versions

### Code Optimization

- [ ] Replace all `.AsSpan()` calls with implicit conversions
- [ ] Adopt `field` keyword in properties with backing fields
- [ ] Convert extension helpers to extension members (properties, operators)
- [ ] Use `SearchValues` for analyzer character matching
- [ ] Use `Order()`/`OrderDescending()` where applicable
- [ ] Enable `ValueTask` in Store/Index async paths
- [ ] Convert hot-path reference types to structs
- [ ] Replace `Substring()` with `ReadOnlySpan<char>` slicing throughout
- [ ] Audit `BitConverter.GetBytes` — now stack-allocated by JIT
- [ ] Add `ArrayPool` in hot allocation paths

### Housekeeping

- [ ] Remove `Microsoft.Bcl.Memory`, `System.Memory` package refs
- [ ] Remove `Microsoft.NETFramework.ReferenceAssemblies`
- [ ] Remove netstandard/netframework InternalsVisibleTo if any
- [ ] Remove `FEATURE_*` conditional compilation constants
- [ ] Remove `FEATURE_SERIALIZABLE` blocks for .NET FW
- [ ] Clean up `.editorconfig` — remove legacy CA suppressions
- [ ] Clean up legacy `NoWarn` entries specific to old TFMs

### Testing & Validation

- [ ] Full test suite pass on `net10.0`
- [ ] Baseline benchmark run (before optimizations)
- [ ] Post-optimization benchmark comparison
- [ ] Verify NuGet packaging
- [ ] Verify SourceLink and deterministic builds

---

## References

- [Performance Improvements in .NET 10](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-10/)
- [Introducing C# 14](https://devblogs.microsoft.com/dotnet/introducing-csharp-14/)
- [What's New in .NET 10](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10/overview)
- [Breaking Changes in .NET 10](https://learn.microsoft.com/en-us/dotnet/core/compatibility/10.0)
