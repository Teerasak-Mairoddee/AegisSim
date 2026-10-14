# Learning journal

This journal records what Tee learned, in Tee's own words, with evidence. Claude never writes Tee's answers; unanswered questions say **Awaiting my answer**.

## Lesson notes

| # | Topic | Milestone | Status |
|---|---|---|---|
| 000 | [Project orientation](000-project-orientation.md) | M0 | Complete (gate passed 2026-10-14) |
| 001 | [Unity simulation loop](001-unity-simulation-loop.md) | M1 | Taught; reflection awaiting |
| 002 | Vehicle dynamics | M1 | Not started |
| 003 | Control and navigation | M2 | Not started |
| 004 | Sensors and observations | M3 | Not started |
| 005 | Computer vision | M4 | Not started |
| 006 | Tracking and estimation | M5 | Not started |
| 007 | Edge inference | M6 | Not started |
| 008 | Multi-agent systems | M8 | Not started |
| 009 | Experimental methods | M9 | Not started |

A note is created when its topic is introduced. When understanding improves, the note gains a dated "What changed in my understanding?" entry; earlier text is kept.

## Session log

### 2026-10-14 (session 4, continued): Lesson 001, the simulation loop (M1)

- **What we built:** A throwaway `LoopCounter` experiment (not committed) and lesson note [001](001-unity-simulation-loop.md). I wrote the two counting lines.
- **What was covered** (my understanding is evidenced only where the answers show it): `Update` vs `FixedUpdate`; fixed `dt`; real vs simulated time; `N = duration ÷ dt`; why the step will live in Core and be called from `FixedUpdate`.
- **Questions answered:** Prediction (Update same, FixedUpdate halves at 30 FPS) was the wrong way round; measured 60/50 and 30/50. Explained the result correctly. "How can a test step 1 s?": first said it uses clock time (misconception, corrected). Step counting: needed help to get 50 for `dt` = 0.02, then answered 100 for `dt` = 0.01 correctly. Final explanation covered repeatability but not the test half. Details in [001](001-unity-simulation-loop.md).
- **Mistakes and corrections:** Edited the wrong file at first (the new script was still the empty template); fixed. My PC hadn't pulled the session 3 and 4 merges; ran `git restore` on `QualitySettings.asset` and pulled.
- **Tests and results:** None run (no committed code changed). Experiment result in lesson 001.
- **Files changed:** `docs/learning/001-unity-simulation-loop.md` (new), this journal, `docs/BACKLOG.md` (M1-1 ticked).
- **Decisions:** The simulation step will be `Step(dt)` in Core, called from a Runtime `FixedUpdate` with 0.02 s.
- **Blockers:** None.
- **Next smallest task:** Delete the `Experiments` folder in Unity, then lesson 002 (vehicle state: position, velocity, acceleration limits) and hand-trace three integration steps before writing the first Core test (M1-3).
- **Retrieval question for next session:** A drone at position 0 m moves at 5 m/s. With `dt` = 0.02 s, where is it after 3 steps?

### 2026-10-14 (session 4): Milestone 0 gate passed (M0)

- **What we built:** No code. Closed the M0 learning gate and the orientation note. **Milestone 0 is complete.**
- **What was covered** (my understanding is evidenced only where the answers below show it): Simulation as the world that applies physics and writes the true state; Control only sends commands; a collision is recorded by Simulation and counted as a failure by Evaluation.
- **Questions answered:** Retrieval question (Control vs a wall): "Simulation" and "should simulate a crash", correct. Final gate answer: all five modules correct after I fixed a slip (wrote "Tracking" for coordination). Reflection recorded. Details in [000](000-project-orientation.md).
- **Mistakes and corrections:** Last session's misconception (Control moves the drone) corrected. Slip: named Tracking twice; corrected to Coordination myself when asked.
- **Tests and results:** None run (no code changed).
- **Files changed:** Orientation note, this journal, `docs/BACKLOG.md` (M0-5, M0-6 ticked).
- **Decisions:** None.
- **Blockers:** None.
- **Next smallest task:** M1 lesson 001: the Unity simulation loop (`Update` vs `FixedUpdate`, fixed timestep) and what a real test checks, compared with gameplay testing.
- **Retrieval question for next session:** In one sentence, why is a collision in AegisSim recorded by Simulation but judged by Evaluation?

### 2026-10-14: Command-line tests and the M0 gate (M0)

- **What we built:** No new code. Ran the Edit Mode tests from PowerShell in batch mode (M0-4) and recorded the verified command in `CLAUDE.md`.
- **What was covered** (my understanding is evidenced only where the answers below show it): why tests are run by a command and not only by clicking; what each batch-mode flag does; exit codes (0 passed, 2 a test failed, 1 tests did not run); that batch mode can't open a project already open in the editor; ground truth vs belief.
- **Questions answered:** Retrieval question (PlayMode vs `UnityEditor.TestRunner`): first said it is because Play Mode tests run during a live playtest (partly right), then thought `UnityEditor` could be read after a build (misconception: builds strip it). Restated correctly: "If the playmode assembly tries to read from the UnityEditor test runner it would fail becuase a built game excludes the editor". Exit-code check (`Assert.IsTrue(false)` gives which code?): "2", correct. Q2 (who may read ground truth): understood on the second attempt. Learning gate (Q1): perception, tracking, and control vs coordination shown; Simulation not yet (said Control moves the drone). Details in [000](000-project-orientation.md).
- **Mistakes and corrections:** First batch-mode run returned 1 with no results file; the log stopped right after opening the project and `Temp/UnityLockfile` was present. After closing Unity, the run returned 0.
- **Tests and results:** Edit Mode, command line: 1 passed, 0 failed, exit code 0. See [test strategy](../testing/test-strategy.md).
- **Files changed:** `CLAUDE.md` (verified command), `docs/testing/test-strategy.md`, `docs/BACKLOG.md` (M0-4 ticked), orientation note, this journal, `ProjectSettings/QualitySettings.asset` (Unity's own format upgrade).
- **Decisions:** Use `Start-Process -Wait -PassThru` so PowerShell waits for Unity and can read its exit code. The gate stays open until the Simulation part is shown.
- **Blockers:** None.
- **Next smallest task:** Revisit Simulation with a new example and close the M0 gate (#6) and orientation note (#5), then start M1 lesson 001.
- **Retrieval question for next session:** A defender's Control sends "fly north at 5 m/s" towards a wall 0.05 m away. Which module decides where the drone actually ends up, and what happens?

### 2026-10-13: Unity project and first test (M0)

- **What we built:** Cloned the repo to my PC, created the Unity 6000.6.5f1 URP project in `unity/AegisSim/`, and created the four assemblies (`AegisSim.Core` with No Engine References, `AegisSim.Runtime`, `AegisSim.Tests.EditMode`, `AegisSim.Tests.PlayMode`) with a placeholder Edit Mode test. My first commits: `6b6568e`, `cefbca8`.
- **What was covered** (my understanding is evidenced only where the answers below show it): What `git clone`, `git log`, branches and `push -u` do; that `.gitignore` keeps generated folders out (A2); that an assembly definition is a wall that controls what code can reference; that No Engine References makes Unity invisible to Core at compile time; what `[Test]` and `Assert` do.
- **Questions answered:** Commit count prediction (said 2, then 3; actual 4, I missed the two commits recording my answers). A2 prediction correct. Probe experiment: first predicted it would control a position (misconception, corrected), result was a compile error. "Why ban Unity in Core?": partly understood. A6 prediction ("1 test, green, because it cannot fail") correct. Details in [000](000-project-orientation.md).
- **Mistakes and corrections:** First clone landed in `AegisSim\AegisSim` because the folder already existed; I deleted my local copy and re-cloned. Unity's "Tests Assembly Folder" menu did not create an assembly for me, so I built the test assemblies by hand. A review of my `.asmdef` files found four issues (PlayMode used the Editor TestRunner, EditMode not Editor-only, Core had no root namespace, a leftover folder); all fixed and re-checked.
- **Tests and results:** A2 pass; A6 pass (1 passed, 0 failed, Edit Mode, manual run). See [test strategy](../testing/test-strategy.md).
- **Files changed:** `unity/AegisSim/` (new project), `Assets/_Project/**` (assemblies, test), this journal, orientation note, test strategy, backlog.
- **Decisions:** Code lives under `Assets/_Project/` (as in RoverCore). Tonight's work goes through branch `m0-unity-setup` and a pull request.
- **Blockers:** None.
- **Next smallest task:** Review and merge the pull request, then verify the command-line test run (M0-4) and answer the M0 learning gate.
- **Retrieval question for next session:** Why does the PlayMode test assembly reference `UnityEngine.TestRunner` but not `UnityEditor.TestRunner`?

### 2026-10-12: Kick-off (M0)

- **What we built:** No code. Agreed the learning plan and Milestone 0 scope; drafted this repository's foundation (README, CLAUDE.md, docs skeleton, ADR 0001, backlog).
- **What I learned:** Awaiting my answer (the M0 learning gate is still open).
- **Questions answered:** Experience with Unity, C#, Python, ML and robotics; hardware; weekly time; teaching style. Recorded in [000](000-project-orientation.md).
- **Tests and results:** None yet. The first test run is acceptance test A6 (placeholder Edit Mode test).
- **Files changed:** Repository foundation files (see CHANGELOG).
- **Decisions:** ADR 0001 (repo, Unity version, assembly structure, Python timing, backlog in GitHub Issues).
- **Blockers:** The GitHub repository needs to be created.
- **Next smallest task:** Create the GitHub repo and push this foundation, then create the Unity project in `unity/AegisSim/`.
- **Retrieval question for next session:** In AegisSim, which module is allowed to read the true position of a vehicle, and why are the others not?
