# Phase 2: Target Framework — net10.0 Only

## Decision

**Drop everything. Target `net10.0` exclusively.**

| Old TFM | Action | Rationale |
|---------|--------|-----------|
| `net10.0` | Keep | Primary (and only) target |
| `net8.0` | Drop | No backward compat |
| `netstandard2.0` | Drop | No backward compat |
| `net462` | Drop | No backward compat |

## What This Enables

Without backward compat constraints, we can use all of .NET 10 / C# 14 directly:
- **Spans everywhere** — no `System.Memory` polyfill needed
- **No `.AsSpan()` calls** — implicit conversions from arrays
- **No conditional compilation** — no `#if NET10_0_OR_GREATER`
- **No feature-flag constants** — remove all `FEATURE_*` from `Directory.Build.targets`
- **No serialization polyfills** — remove `FEATURE_SERIALIZABLE`, `FEATURE_SERIALIZABLE_EXCEPTIONS` blocks

## What to Remove

### 1. Feature Flag Constants in Directory.Build.targets

Delete the entire `PropertyGroup` chain at lines 51-176. All `FEATURE_*` constants are dead code when targeting net10.0 only.

### 2. NetStandard/NetFramework InternalsVisibleTo

Search for `InternalsVisibleTo` entries referencing test projects that target old TFMs — remove if they only existed for backward-compat testing.

### 3. Serialization Attributes

Remove `[Serializable]`, `ISerializable` implementations that only existed for .NET Framework. .NET 10 does not require these for the runtime serialization scenarios they supported.

## Simplification Benefits

| Before | After |
|--------|-------|
| 4 TFMs × N projects = 4x builds | 1 TFM = 1x build |
| Condition chains in 254-line .targets | ~50 lines |
| Conditional NuGet refs per TFM | Single unconditional ref |
| Complex TestTargetFramework.props | 3-line file |
| VS version detection logic | None |
| Multi-TFM CI matrix | Single TFM |
