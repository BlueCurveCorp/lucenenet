# Phase 1: Build Infrastructure Updates

## Task 1.1 — Update Minimum SDK Version

**File:** `.build\runbuild.ps1:30`

```diff
- [string]$minimumSdkVersion = "9.0.200"
+ [string]$minimumSdkVersion = "10.0.301"
```

---

## Task 1.2 — Pin SDK Version in global.json

**File:** `global.json`

```json
{
  "sdk": {
    "version": "10.0.301",
    "rollForward": "latestFeature"
  },
  "msbuild-sdks": {
    "Microsoft.Build.NoTargets": "3.7.56"
  },
  "sources": [ "src" ]
}
```

Pins the SDK version across all environments for reproducible builds.

---

## Task 1.3 — Remove VS Legacy Version Check

**File:** `Directory.Build.props:34-41`

Delete the entire `Legacy Visual Studio Support` property group. We no longer need to detect old VS versions — `net10.0` requires the current toolchain.

---

## Task 1.4 — Simplify Centralized Target Frameworks

**File:** `Directory.Build.props:43-52`

```diff
  <PropertyGroup Label="Centralized Target Frameworks">
-   <LibraryTargetFrameworks>net10.0;net8.0;netstandard2.0;net462</LibraryTargetFrameworks>
-   <LibraryTargetFrameworks Condition=" '$(IsLegacyVisualStudioVersion)' == 'true' ">net8.0</LibraryTargetFrameworks>
-   <NetCoreOnlyTargetFrameworks>net10.0;net8.0</NetCoreOnlyTargetFrameworks>
-   <NetCoreOnlyTargetFrameworks Condition=" '$(IsLegacyVisualStudioVersion)' == 'true' ">net8.0</NetCoreOnlyTargetFrameworks>
+   <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>
```

**All projects now target net10.0 only.** No multi-targeting, no conditionals.

---

## Task 1.5 — Update NuGet Dependencies

**File:** `.build\dependencies.props`

Remove packages that were only needed for pre-net10 compatibility:

| Remove | Reason |
|--------|--------|
| `Microsoft.Bcl.Memory` | Span/memory types are in-box in net10.0 |
| `System.Memory` | In-box |
| `System.Runtime.InteropServices.RuntimeInformation` | In-box |
| `System.Text.Encoding.CodePages` version conditionals | Simplify |
| `Microsoft.NETFramework.ReferenceAssemblies` | No .NET Framework targets |

**Update remaining packages:**
- `Microsoft.NET.Test.Sdk` → latest
- `NUnit` / `NUnit3TestAdapter` → latest compatible
- `Microsoft.Extensions.*` → 10.0.x stable

---

## Task 1.6 — Remove Conditional Package References

**File:** `src\Lucene.Net\Lucene.Net.csproj:69-86`

Delete the `TargetFramework`-conditioned `ItemGroup` blocks for netstandard2.0, netstandard2.1, and net462. These packages are no longer needed.

Keep only unconditional package references:
```xml
<ItemGroup>
  <PackageReference Include="J2N" Version="$(J2NPackageVersion)" />
  <PackageReference Include="Microsoft.Extensions.Configuration.Abstractions" Version="$(MicrosoftExtensionsConfigurationAbstractionsPackageVersion)" />
  <PackageReference Include="SpanTools.ValueStringBuilder.Generator" Version="$(SpanToolsValueStringBuilderGeneratorPackageVersion)" PrivateAssets="all" />
</ItemGroup>
```

---

## Task 1.7 — Simplify TestTargetFramework.props

**File:** `TestTargetFramework.props`

Remove the multi-TFM CI matrix and version detection. Single target only:

```xml
<Project>
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <IsTestProject>true</IsTestProject>
    <IsPublishable>true</IsPublishable>
  </PropertyGroup>
</Project>
```
