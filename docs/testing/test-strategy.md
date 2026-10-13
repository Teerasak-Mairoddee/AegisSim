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
- **Command line:** to be verified during M0 acceptance test A6.

## Results log

| Date | Milestone | Suite | Passed | Failed | Notes |
|---|---|---|---|---|---|
| (none yet) | | | | | |

## Known limitations

- No tests exist yet (M0).
