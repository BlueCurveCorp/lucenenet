# Lucene.NET - Agent Development Guide

## General instructions

Be **concise** when interacting with the user


## Project Overview

Apache Lucene.NET is a **full-text search engine library** written in C#

**Repository**: https://github.com/apache/lucenenet
**Website**: https://lucenenet.apache.org

## Codebase Knowledge Graph (codebase-memory-mcp)

This project uses codebase-memory-mcp to maintain a knowledge graph of the codebase.
**ALWAYS** prefer MCP graph tools over grep/glob/file-search for code discovery.
**ALWAYS** refresh the graph when you finish your job


## MASTER SKILL REGISTRY & PROTOCOL

**ALWAYS load the `master-skill-registry-protocol` skill**, if you can't find a skill, check if the `master-skill-registry-protocol` can route you to an approriate skill.
**When listing skills ALWAYS take into account the `master-skill-registry-protocol` , `MASTER SKILL REGISTRY`**

## Progress tracking

**ALWAYS** create an issue in `Linear` to track what you are doing

- **ALWAYS** write a detailed plan before proceeding
- **ALWAYS** use the team `BlueCurve` - You are part of this team
- **ALWAYS** use the workspace `BlueCurve` - You are part of this workspace
- If an issue should be split use sub-issues
- **ALWAYS** update the issues status
- Use the issues comments to track you progress
- In doubt **ALWAYS** referer to the issue, if the issue is a sub-issue start from the master issue and read it then all sub-issues before the one you are working on so you can refresh your understanding

**If Linear is not reacheable use the following instructions**:
- **ALWAYS** write a detailed plan before proceeding in a directory named `./plans/<GLOBAL_TASK_NAME>`
- **ALWAYS** each task is prioritized and have a unique identifier and a link to the target `./plans/<GLOBAL_TASK_NAME>/<TASK_ID>.md`
- **ALWAYS** keep track of your work for each task using a memory file `./plans/<GLOBAL_TASK_NAME>/<TASK_ID>.md`
- **ALWAYS** update the task status **Backlog** → **Todo** → **In Progess** → **Done**

## Documentation

- **ALWAYS** ensures documentation is up-to-date

---

## Solution Structure

The solution (`Lucene.Net.sln`) contains **80+ projects** organized into:

| Category | Projects | Description |
|----------|----------|-------------|
| **Core** | Lucene.Net | Main search engine library |
| **Analysis** | 9 projects | Tokenizers, analyzers for languages/domains |
| **Features** | 12 projects | Facets, Highlighting, Spatial, Suggest, Queries, Expressions, Grouping, Join, Classification, Benchmark, Codecs, Memory |
| **Tools** | 2 projects | lucene-cli (global tool), Lucene.Net.Demo |
| **Replication** | 2 projects | Lucene.Net.Replicator, Lucene.Net.Replicator.AspNetCore |
| **ICU** | 2 projects | Lucene.Net.ICU (requires ICU4N) |
| **Code Analysis** | 3 projects | Roslyn analyzers for C# and VB.NET |
| **Test Framework** | 4 projects | NUnit-based testing infrastructure |
| **Tests** | 32 projects | Unit/integration tests grouped alphabetically |
| **Documentation** | 2 projects | DocFX site + LuceneDocsPlugins |
| **Build** | 3 MSBuild projects | Build orchestration, website generation, GitHub automation |

---

## Project Categories & Details

### 1. Core Library (`src/Lucene.Net`)
- **Main search engine**: Indexing, searching, document model, query parsing
- **Target Frameworks**: net10.0, net8.0, netstandard2.0, net462
- **Dependencies**: J2N, Microsoft.Extensions.Configuration.Abstractions, SpanTools.ValueStringBuilder.Generator
- **Key Feature**: Includes Roslyn analyzers as NuGet package analyzers

### 2. Analysis Modules (9 projects)
| Project | Purpose | Language/Domain |
|---------|---------|-----------------|
| Lucene.Net.Analysis.Common | Core analyzers for many languages | General purpose |
| Lucene.Net.Analysis.Kuromoji | Japanese morphological analyzer | Japanese |
| Lucene.Net.Analysis.Phonetic | Sound-alike search (DoubleMetaphone, etc.) | Phonetic |
| Lucene.Net.Analysis.SmartCn | Chinese analyzer | Chinese |
| Lucene.Net.Analysis.Stempel | Polish stemming analyzer | Polish |
| Lucene.Net.Analysis.OpenNLP | OpenNLP integration | NLP |
| Lucene.Net.Analysis.Morfologik | Dictionary stemming (Polish built-in) | Morphological |
| Lucene.Net.Analysis.ICU | ICU-based analyzers/highlighters | Unicode/ICU |

### 3. Feature Modules (12 projects)
| Project | Purpose | Key Features |
|---------|---------|--------------|
| Lucene.Net.Facet | Faceted search/indexing | Taxonomy, drill-down, multi-select |
| Lucene.Net.Highlighter | Hit highlighting | Fragments, formatters, scorers |
| Lucene.Net.Spatial | Geospatial search | Points, shapes, distance queries |
| Lucene.Net.Suggest | Auto-complete/spell-check | FST-based suggesters |
| Lucene.Net.Queries | Additional query types | Regex, prefix, wildcards, etc. |
| Lucene.Net.QueryParser | Text-to-query parsers | Standard, complex phrase, surround |
| Lucene.Net.Expressions | Dynamic computed values | Pluggable grammar for sort/facet |
| Lucene.Net.Grouping | Result grouping | Collectors for grouped results |
| Lucene.Net.Join | Index/query-time joins | BlockJoinQuery, TermsQuery |
| Lucene.Net.Classification | Document classification | KNN, Naive Bayes |
| Lucene.Net.Benchmark | Performance benchmarking | Task-based benchmark suite |
| Lucene.Net.Codecs | Index formats/postings | Pluggable codecs |
| Lucene.Net.Memory | In-memory index | Single-document RAM index |
| Lucene.Net.Misc | Index tools/utilities | IndexUpgrader, etc. |

### 4. Tools (`src/dotnet/tools/`)
| Project | Type | Description |
|---------|------|-------------|
| lucene-cli | .NET Global Tool | Cross-platform CLI for index maintenance & demos |
| Lucene.Net.Demo | Console Apps | Sample applications (IndexFiles, SearchFiles, etc.) |

### 5. Replication (`src/Lucene.Net.Replicator`, `src/dotnet/Lucene.Net.Replicator.AspNetCore`)
- **Lucene.Net.Replicator**: File replication utility for index synchronization
- **Lucene.Net.Replicator.AspNetCore**: ASP.NET Core integration

### 6. ICU Integration (`src/dotnet/Lucene.Net.ICU`)
- Requires ICU4N package
- Specialized Unicode analyzers and highlighters

### 7. Code Analysis (`src/dotnet/Lucene.Net.CodeAnalysis.*`)
- **CSharp**: Roslyn analyzers for C#
- **VisualBasic**: Roslyn analyzers for VB.NET
- Packaged as NuGet analyzers in core Lucene.Net package

### 8. Test Framework (`src/Lucene.Net.TestFramework`, etc.)
- **Lucene.Net.TestFramework**: NUnit-based testing utilities, base classes, randomized testing
- **Lucene.Net.TestFramework.DependencyInjection**: DI extensions for tests
- **Lucene.Net.TestFramework.NUnitExtensions**: Custom NUnit attributes/extensions
- **Lucene.Net.TestFramework.TestData.NUnit**: Test data/resources

### 9. Test Projects (32 projects)
Grouped alphabetically for parallel CI execution:
- **Lucene.Net.Tests._A-D**, **._E-I**, **._I-J**, **._J-S**, **._T-Z**: Core library tests
- **Analysis tests**: Per-analyzer test projects
- **Feature tests**: Per-module test projects (Facet, Spatial, Suggest, etc.)
- **Special**: Benchmark, Cli, CodeAnalysis, ICU, Replicator, Sandbox

### 10. Documentation (`src/docs/`, `websites/site/`)
- **LuceneDocsPlugins**: Custom DocFX plugins for API docs
- **websites/site**: DocFX-based website with quick-start, release notes, API docs

---

## Build System

### Build Scripts
| Script | Purpose |
|--------|---------|
| `build.ps1` / `build.bat` | Main entry point (parses args, invokes Psake) |
| `.build/runbuild.ps1` | Psake build script with tasks: Clean, Restore, Compile, Pack, Test, Publish |
| `global.json` | Pins MSBuild SDK (Microsoft.Build.NoTargets 3.7.56) |

### Key Build Properties (Directory.Build.props)
```xml
<LibraryTargetFrameworks>net10.0;net8.0;netstandard2.0;net462</LibraryTargetFrameworks>
<NetCoreOnlyTargetFrameworks>net10.0;net8.0</NetCoreOnlyTargetFrameworks>
<VersionPrefix>4.8.0</VersionPrefix>
<AssemblyVersion>4.0.0</AssemblyVersion>
<SignAssembly>true</SignAssembly>
```

### Build Commands
```powershell
# Windows
.\build.ps1                    # Release build
.\build.ps1 -t                 # Build + run tests
.\build.ps1 --configuration Debug -pv 4.8.0-beta00015 -fv 4.8.0

# Linux/macOS
./build                        # Release build
./build -t                     # Build + run tests
```

### CI/CD
- **Azure Pipelines** (`azure-pipelines.yml`): Main CI with matrix testing across TFMs
- **GitHub Actions** (`.github/workflows/`): 30+ workflow files for per-module test runs
- **Parallel Testing**: 8 parallel jobs by default (configurable via `-mp`)

---

## Testing

### Test Framework
- **NUnit 3.x** with `RandomizedTesting.Generators` (Lucene's randomized testing)
- **Microsoft.Testing.Platform (MTP)** supported via `migrate-vstest-to-mtp` skill
- **Test discovery**: Projects with `IsTestProject=true` and `TestFrameworks` property

### Running Tests
```powershell
# Via build script (recommended)
.\build.ps1 -t -mp 10

# Direct dotnet test
dotnet test Lucene.Net.sln --filter "FullyQualifiedName~Lucene.Net.Tests.Analysis"

# Filter by category/trait
dotnet test --filter "TestCategory=Integration"
```

### Test Organization
- **Alphabetical grouping**: Tests split into 5 projects (_A-D, _E-I, _I-J, _J-S, _T-Z) for parallelism
- **Per-module tests**: Each feature has dedicated test project
- **7800+ tests** currently passing

---

## Development Workflow

### Prerequisites
- **PowerShell 5.0+** (for build.ps1)
- **.NET 9.0.200+ SDK** (pinned in global.json, updated by build script)
- **Visual Studio 2022 17.10+** (for IDE development)

### Common Tasks

| Task | Command |
|------|---------|
| Clean build | `.\build.ps1` |
| Debug build | `.\build.ps1 --configuration Debug` |
| Run tests | `.\build.ps1 -t` |
| Custom version | `.\build.ps1 -pv 4.8.0-beta00015 -fv 4.8.0` |
| Publish binaries | `.\build.ps1 --configuration Release` then check `_artifacts/Publish` |

### Version Management
- **VersionPrefix**: `4.8.0` (in Directory.Build.props) - matches Lucene Java version
- **AssemblyVersion**: `4.0.0` - major only, for strong-name compatibility
- **PackageVersion**: Generated at build time (e.g., `4.8.0-beta00015` or `4.8.0-ci00015`)
- **Release distributions**: Include `version.props` to "freeze" version

### Code Style
- **.editorconfig** enforced at solution level
- **LangVersion**: 14.0 (C# 14)
- **Nullable**: Enabled
- **Analysis**: Roslyn analyzers included in build

---

## Adding New Projects

### Library Project Template
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <Description>Description for NuGet</Description>
    <PackageDocumentationRelativeUrl>path/overview.html</PackageDocumentationRelativeUrl>
  </PropertyGroup>
  <Import Project="$(SolutionDir).build/nuget.props" />
  <PropertyGroup>
    <TargetFrameworks>$(LibraryTargetFrameworks)</TargetFrameworks>
    <AssemblyTitle>Lucene.Net.NewModule</AssemblyTitle>
    <PackageTags>$(PackageTags);tag1;tag2</PackageTags>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\Lucene.Net\Lucene.Net.csproj" />
  </ItemGroup>
  <ItemGroup>
    <InternalsVisibleTo Include="Lucene.Net.Tests.NewModule" />
  </ItemGroup>
</Project>
```

### Test Project Template
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <IsTestProject>true</IsTestProject>
    <IsPackable>false</IsPackable>
    <TargetFrameworks>$(LibraryTargetFrameworks)</TargetFrameworks>
  </PropertyGroup>
  <Import Project="$(SolutionDir).build/nuget.props" />
  <ItemGroup>
    <ProjectReference Include="..\Lucene.Net.NewModule\Lucene.Net.NewModule.csproj" />
    <ProjectReference Include="..\Lucene.Net.TestFramework\Lucene.Net.TestFramework.csproj" />
  </ItemGroup>
</Project>
```

---

## Key Conventions & Patterns

### InternalsVisibleTo
All library projects declare `InternalsVisibleTo` for:
- Corresponding test project
- `Lucene.Net.TestFramework`
- All test group projects (`_A-D`, `_E-I`, etc.)
- `Lucene.Net.Tests.AllProjects`

### Package Management
- **Central Package Management**: `Directory.Packages.props` (via `Directory.Build.props`)
- **Package versions** defined in `.build/nuget.props`
- **NuGet packages** output to `_artifacts/NuGetPackages/`

### Strong Naming
- All assemblies signed with `Lucene.Net.snk`
- Public key defined in `Directory.Build.props`

### Target Framework Strategy
- **LibraryTargetFrameworks**: net10.0; net8.0; netstandard2.0; net462
- **NetCoreOnlyTargetFrameworks**: net10.0; net8.0 (for tools, ICU, CodeAnalysis)
- Legacy VS (< 18.0) falls back to net8.0 only

---

## IDE Setup (Visual Studio)

1. Open `Lucene.Net.sln`
2. Configure test TFM: Edit `.build/TestTargetFramework.props`, uncomment desired `<TargetFramework>`
3. Build solution
4. **Important**: Set default processor architecture to **x64** in Test Explorer settings to avoid OOM

---

## Common Issues & Solutions

| Issue | Solution |
|-------|----------|
| Build fails with SDK version | Build script auto-updates `global.json` to pinned version |
| Tests OOM in VS | Set Test Explorer → Processor Architecture → x64 |
| Version conflicts | Use `version.props` in release distributions to freeze version |
| Missing analyzers in NuGet | Core Lucene.Net package includes analyzers via `<None Pack="true">` |
| ICU not building | Requires ICU4N package; check `src/dotnet/Lucene.Net.ICU` dependencies |

---

## Release Process

1. Update `VersionPrefix` in `Directory.Build.props` when porting new Lucene version
2. Create `version.props` at repo root with frozen versions
3. Build creates packages in `_artifacts/NuGetPackages/`
4. Publish to NuGet.org (via trusted publishing/OIDC)

---

## Useful Skills for Agents

When working on this codebase, consider these skills:

| Skill | When to Use |
|-------|-------------|
| `analyzing-dotnet-performance` | Optimizing hot paths in indexing/search |
| `build-perf-diagnostics` | Slow build analysis via binlog |
| `msbuild-antipatterns` | Reviewing .csproj/.props files |
| `convert-to-cpm` | Managing NuGet versions centrally |
| `test-anti-patterns` | Auditing test quality |
| `migrate-vstest-to-mtp` | Modern test runner migration |
| `microbenchmarking` | BenchmarkDotNet for performance tests |
| `dotnet-pinvoke` | If adding native interop (unlikely here) |

---

## Quick Reference

### Directory Structure
```
lucenenet/
├── src/
│   ├── Lucene.Net/                 # Core library
│   ├── Lucene.Net.Analysis.*/      # 9 analysis modules
│   ├── Lucene.Net.Facet/           # Faceted search
│   ├── Lucene.Net.Highlighter/     # Hit highlighting
│   ├── Lucene.Net.Spatial/         # Geospatial
│   ├── Lucene.Net.Suggest/         # Auto-suggest
│   ├── Lucene.Net.Queries/         # Additional queries
│   ├── Lucene.Net.QueryParser/     # Query parsers
│   ├── Lucene.Net.Expressions/     # Dynamic expressions
│   ├── Lucene.Net.Grouping/        # Result grouping
│   ├── Lucene.Net.Join/            # Join queries
│   ├── Lucene.Net.Classification/  # Classification
│   ├── Lucene.Net.Benchmark/       # Benchmarking
│   ├── Lucene.Net.Codecs/          # Index codecs
│   ├── Lucene.Net.Memory/          # In-memory index
│   ├── Lucene.Net.Misc/            # Utilities
│   ├── Lucene.Net.Replicator/      # Index replication
│   ├── Lucene.Net.Sandbox/         # Experimental
│   ├── Lucene.Net.TestFramework/   # Test infrastructure
│   ├── Lucene.Net.Tests.*/         # 32 test projects
│   ├── dotnet/
│   │   ├── tools/lucene-cli/       # CLI global tool
│   │   ├── Lucene.Net.ICU/         # ICU integration
│   │   ├── Lucene.Net.CodeAnalysis.*/ # Roslyn analyzers
│   │   └── Lucene.Net.Replicator.AspNetCore/
│   └── docs/
│       └── LuceneDocsPlugins/      # DocFX plugins
├── websites/site/                  # DocFX website
├── .build/                         # Build scripts (Psake)
├── proj/                           # MSBuild orchestration
├── Lucene.Net.sln                  # Main solution
├── Directory.Build.props           # Centralized props
├── Directory.Build.targets         # Centralized targets
├── global.json                     # SDK pinning
├── build.ps1 / build.bat           # Build entry points
└── azure-pipelines.yml             # CI pipeline
```

### Important Files
| File | Purpose |
|------|---------|
| `Directory.Build.props` | Centralized versioning, signing, target frameworks |
| `.build/nuget.props` | Package version definitions |
| `.build/runbuild.ps1` | Psake build script |
| `TestTargetFramework.props` | TFM selection for VS testing |
| `.editorconfig` | Code style rules |
| `Lucene.Net.snk` | Strong name key |

---

## Contributing Guidelines

See [CONTRIBUTING.md](CONTRIBUTING.md) for:
- Issue reporting template
- Pull request process
- Code review expectations
- Areas needing help (up-for-grabs labels)

---

## License

Apache License 2.0 - See [LICENSE.txt](LICENSE.txt) and [NOTICE.txt](NOTICE.txt)