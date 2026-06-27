# Phase 3: C# 14 Language Feature Adoption

## Prerequisite

LangVersion `14.0` is already set in `Directory.Build.props:29`.

## 3.1 Implicit Span Conversions

**Replace all `.AsSpan()` and `.AsReadOnlySpan()` calls with implicit conversions.**

```
// BEFORE (everywhere in the codebase)
void Process(byte[] data) {
    var span = data.AsSpan();
    ...
}

// AFTER
void Process(ReadOnlySpan<byte> data) {
    // callers with byte[] pass implicitly — zero cost
    // callers with span pass directly
    ...
}
```

**Audit:** grep for `.AsSpan(`, `.AsReadOnlySpan(`, and method signatures taking `byte[]`, `char[]`, `int[]` that immediately convert to spans.

## 3.2 Extension Members

Convert extension methods that act on a single receiver type to extension blocks.

**Target:** `Lucene.Net.Support`, `Lucene.Net.Util`, analysis utilities.

```
// BEFORE
public static class StringExtensions {
    public static bool IsNullOrEmpty(string s) => string.IsNullOrEmpty(s);
}

// AFTER
public static class StringExtensions {
    extension(string s) {
        public bool IsNullOrEmpty => string.IsNullOrEmpty(s);
    }
}
```

Extension operators can also be used for collection/vector types.

## 3.3 `field` Keyword in Auto-Properties

Find properties that declare a backing field solely for validation in the setter.

```
// BEFORE
private string _value;
public string Value {
    get => _value;
    set => _value = value ?? string.Empty;
}

// AFTER
public string Value {
    get => field;
    set => field = value ?? string.Empty;
}
```

**Audit:** Search for `private T _<name>;` followed by `public T <Name>` across the solution.

## 3.4 Lambda Parameter Modifiers

Lambda parameters can now use `ref`, `out`, `in` without explicit type annotations.

```
// BEFORE
TryParse<int> tryParse = (string text, out int result) => int.TryParse(text, out result);

// AFTER
TryParse<int> tryParse = (text, out result) => int.TryParse(text, out result);
```

**Target:** Delegate-heavy code in `Lucene.Net.Search` (scoring, comparators).

## 3.5 Unbound Generic Types in `nameof`

```
// BEFORE
nameof(Dictionary<int, string>)

// AFTER
nameof(Dictionary<,>)
```

**Target:** Error messages, logging, diagnostics code.

## Execution Order

| Order | Feature | Effort | Impact |
|-------|---------|--------|--------|
| 1 | Implicit span conversions | Large | High — removes allocation overhead |
| 2 | `field` keyword | Medium | Medium — reduces boilerplate |
| 3 | Extension members | Medium | Medium — cleaner API surface |
| 4 | Lambda modifiers | Small | Low — readability |
| 5 | Unbound generics | Small | Low — cosmetic |
