# Phase 6: PGO, R2R & Build Configuration

## 6.1 Tiered PGO

Enable in `Directory.Build.props`:

```xml
<PropertyGroup Label=".NET 10 Performance">
  <TieredPGO>true</TieredPGO>
</PropertyGroup>
```

JIT collects runtime profiling data and re-optimizes hot methods with better inlining, devirtualization, and loop optimization. Expects 10-15% throughput improvement on server workloads.

## 6.2 ReadyToRun for CLI Tools

Enable in `lucene-cli` and `Lucene.Net.Demo` project files:

```xml
<PropertyGroup>
  <PublishReadyToRun>true</PublishReadyToRun>
</PropertyGroup>
```

~50% faster startup, 20-30% less memory at runtime. Only applies to published tools, not the library packages.

## 6.3 Enable Source Generators

`SpanTools.ValueStringBuilder.Generator` is already used. Verify compatibility and update to latest stable .NET 10-compatible version.

## 6.4 Trimming Readiness (Optional)

If Native AOT or self-contained trimming is considered:

```xml
<PropertyGroup>
  <IsTrimmable>true</IsTrimmable>
</PropertyGroup>
```

**Note:** High effort — Lucene.NET uses reflection for analysis SPI, codec loading, query parsing. Trim warnings must be resolved with `DynamicallyAccessedMembers` annotations. Not recommended for initial pass.
