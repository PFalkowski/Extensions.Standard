# SonarCloud setup (one-time, no secrets)

SonarCloud ("SonarQube Cloud") is free for public repositories and analyzes C# via **Automatic
Analysis** — driven by the SonarCloud GitHub App, with **no CI workflow, no config file, and no
`SONAR_TOKEN` secret**. (This is how the sibling `PFalkowski/LoggerLite` repo is set up.)

## Steps

1. Go to <https://sonarcloud.io> and **log in with GitHub**.
2. **Analyze a new project** → choose the GitHub organization → import
   `PFalkowski/Extensions.Standard`.
3. Leave the analysis method on the default **Automatic Analysis**. SonarCloud now analyzes on every
   push/PR automatically. That's it — nothing to add to the repo.

## Notes

- **No token, no workflow.** Automatic Analysis and a CI-based scanner are mutually exclusive, which
  is why this repo intentionally has no `sonar.yml`.
- **Coverage** is not imported by Automatic Analysis; test coverage is tracked separately in
  [Codecov](https://codecov.io/gh/PFalkowski/Extensions.Standard) (uploaded from `ci.yml`).
- Optional quality-gate badge for `README.md` once the project exists:
  `[![Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=PFalkowski_Extensions.Standard&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=PFalkowski_Extensions.Standard)`
