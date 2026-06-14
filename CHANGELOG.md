# Changelog

All notable changes to this project are documented here. The format is based on
[Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to
[Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [12.0.0] - 2026-06-14

### Changed (breaking)
- `Area()` now returns a non-negative magnitude. Previously it returned the *signed* shoelace area,
  which was negative for clockwise-wound polygons. ([#7])
- `MaxIndex()` and `FindMinMaxInOn()` now throw `InvalidOperationException` on an empty sequence,
  matching the LINQ `Max()`/`Min()` convention (previously `MaxIndex()` threw
  `ArgumentOutOfRangeException` from an internal index access and `FindMinMaxInOn()` returned an
  inverted `(double.MaxValue, double.MinValue)` range). `Scale()` on an empty sequence still returns
  an empty sequence. ([#9])
- `AsTime(TimeSpan)` no longer leaves a trailing `", "` separator and now returns `"0 ms"` for
  `TimeSpan.Zero`, consistent with the `AsTime(milliseconds)` overload. ([#10])

### Removed (breaking)
- Removed the redundant `Scale<T>(IEnumerable<double>)` overload whose type parameter was never used.
  Use the non-generic `Scale()` (0..1 normalisation) instead. ([#8])

### Added
- Black-box test coverage for previously-untested public surface (`HsVtoArgb`, `Head`/`Tail`/
  `HeadAndTail`, `PluralizeWhenNeeded`, `AsTime(TimeSpan)`, `FindMinMaxInOn`, `Scale()`, `MaxIndex`).
- Code coverage upload to Codecov from CI, and tokenless SonarCloud Automatic Analysis (see `SONAR_SETUP.md`).

## [11.0.0]

### Changed (breaking)
- `DeepCopy` is now a dependency-free, reflection-based deep clone (copies private/inherited fields,
  preserves shared references and cycles, no `[Serializable]`/parameterless-ctor requirement).
- Removed the `Newtonsoft.Json` dependency and the `JsonSerializerSettings` overload of `DeepCopy`.

## [10.0.0]

### Changed (breaking)
- Multi-targeted (`netstandard2.0`, `net8.0`), modernised dependencies, fixed correctness bugs.

### Deprecated
- `InClosedRange`/`InOpenRange` (inverted semantics — use `InRangeExclusive`/`InRangeInclusive`) and
  `ConstructLine` (use `ConstructLineFromRadians`/`ConstructLineFromDegrees`).

[12.0.0]: https://github.com/PFalkowski/Extensions.Standard/releases/tag/v12.0.0
[11.0.0]: https://github.com/PFalkowski/Extensions.Standard/releases/tag/v11.0.0
[10.0.0]: https://github.com/PFalkowski/Extensions.Standard/releases/tag/v10.0.0
[#7]: https://github.com/PFalkowski/Extensions.Standard/issues/7
[#8]: https://github.com/PFalkowski/Extensions.Standard/issues/8
[#9]: https://github.com/PFalkowski/Extensions.Standard/issues/9
[#10]: https://github.com/PFalkowski/Extensions.Standard/issues/10
