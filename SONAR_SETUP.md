# SonarCloud setup (one-time)

SonarCloud (now "SonarQube Cloud") is free for public repositories. The
[`.github/workflows/sonar.yml`](.github/workflows/sonar.yml) workflow runs the analysis on every
push/PR to `master`. It needs a one-time account setup plus a `SONAR_TOKEN` repository secret. Until
the secret exists the workflow is a no-op (it logs a message and exits 0), so it will not turn CI red.

## Steps

1. Go to <https://sonarcloud.io> and **log in with GitHub**.
2. **Analyze a new project** → choose the GitHub organization, then import
   `PFalkowski/Extensions.Standard`.
3. When prompted for the analysis method, pick **"With GitHub Actions"** (CI-based analysis —
   required for C#; automatic analysis does not support C#).
4. SonarCloud shows your **Organization Key** and **Project Key**. Confirm they match the values in
   `sonar.yml`:
   - `/o:` → Organization Key (the workflow assumes `pfalkowski`)
   - `/k:` → Project Key (the workflow assumes `PFalkowski_Extensions.Standard`)

   If SonarCloud generated different values, update those two lines in `sonar.yml`.
5. SonarCloud generates a token. In GitHub: **Settings → Secrets and variables → Actions → New
   repository secret**, name it `SONAR_TOKEN`, paste the value.
6. (Recommended) In the SonarCloud project under **Administration → Analysis Method**, turn **off**
   "Automatic Analysis" so it does not conflict with the CI-based analysis.

## Notes

- Coverage is collected with Microsoft's `dotnet-coverage` tool and handed to Sonar via
  `sonar.cs.vscoveragexml.reportsPaths=coverage.xml`.
- The scanner engine runs on Java 17, which the workflow installs.
- A green PR badge / quality gate can be added to `README.md` once the project exists:
  `[![Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=PFalkowski_Extensions.Standard&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=PFalkowski_Extensions.Standard)`
