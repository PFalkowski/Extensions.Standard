# Extensions.Standard

[![CI](https://github.com/PFalkowski/Extensions.Standard/actions/workflows/ci.yml/badge.svg)](https://github.com/PFalkowski/Extensions.Standard/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Extensions.Standard.svg)](https://www.nuget.org/packages/Extensions.Standard/)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=PFalkowski_Extensions.Standard&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=PFalkowski_Extensions.Standard)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://choosealicense.com/licenses/mit/)
[![Buy Me a Coffee](https://img.shields.io/badge/Buy%20Me%20a%20Coffee-support-yellow.svg)](https://www.buymeacoffee.com/piotrfalkowski)

Lightweight extension methods (and constants) for common .NET programming tasks. Target: `net8.0`.

## Install

```bash
dotnet add package Extensions.Standard
```

## Highlights

- Suffixes (`AsMemory`, `AsTime`)
- `Interpolate`
- `Partition`
- `Shuffle` (O(n) Fisher-Yates)
- `Softmax`
- `InnerProduct`
- `IsPrime`
- `ManhattanDistance`, `EuclideanDistance`
- `InRangeInclusive` / `InRangeExclusive`
- `MeanSquaredError`
- `EqualsWithTolerance`
- `HsVtoArgb`
- `Scale`, `Fit`
- `DeepCopy` (reflection-based deep clone, no dependencies)
- `Truncate` (string truncation with suffix)

and many more.

Extensions for `System.Random` (NextBool, NextChar, NextString etc.) moved to [StrongRandom](https://github.com/PFalkowski/StrongRandom).
