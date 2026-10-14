# Test strategy

Tests are added with each feature, not afterwards. Every test states **what it proves and what it does not prove.**

## Test categories

| Category | Where | Example | Starts at |
|---|---|---|---|
| Unit | Edit Mode (Core) | State update, tag rule, metric maths | M1 |
| Simulation | Play Mode (Runtime) | Agent stays in arena; separation held | M1/M2 |
| Repeatability | Edit Mode | Same seed gives identical trajectory | M1 |
| Integration | Edit Mode | Observation to track data flow | M5 |
| Evaluation | pytest | Precision/recall on a known toy set | M4 |
| Failure injection | Edit/Play Mode | Dropped detections, latency, comms loss | M5+ |
| Regression | wherever the bug lived | Named after the bug it guards | Any |

## Rules

- Never delete a test or weaken an assertion to make a build pass. A change to a test needs a written reason in the commit message.
- Tests use fixed seeds. A flaky test is a bug.
- Results are reported exactly: passed, failed, or could not be run (and why).
- The results of each milestone's test run are recorded below.

## How to run

- **Unity:** Window > General > Test Runner, then the Edit Mode or Play Mode tab, then Run All.
- **Command line:** see the PowerShell command in `CLAUDE.md` (Commands). Verified on Tee's PC on 2026-10-14. Exit code 0 = passed, 2 = a test failed, 1 = tests did not run.

## Results log

| Date | Milestone | Suite | Passed | Failed | Notes |
|---|---|---|---|---|---|
| 2026-10-13 | M0 | Edit Mode (`PlaceholderTests`), run manually in Test Runner on Tee's PC | 1 | 0 | A6: proves the test assembly compiles and is discovered; proves nothing about simulation behaviour |
| 2026-10-13 | M0 | Repo hygiene (A2): `git status` after opening Unity | pass | – | Only `.vsconfig`, `Assets`, `Packages`, `ProjectSettings` tracked; `Library`, `Temp`, `Logs`, `UserSettings` ignored |
| 2026-10-14 | M0 | Edit Mode, command line (M0-4), batch mode on Tee's PC | 1 | 0 | Exit code 0; `editmode.xml` shows `PlaceholderTests.TestPipeline_IsWired` passed. First attempt exited 1, most likely because the editor still had the project open (`Temp/UnityLockfile` present; passed once Unity was closed) |

## Known limitations

- Only a placeholder test exists (M0). No simulation logic is tested yet.
- The command-line run is manual. Nothing runs tests automatically on push yet.
- No Play Mode test exists yet; the Play Mode assembly is untested wiring.
