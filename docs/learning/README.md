# Learning journal

This journal records what Tee learned, in Tee's own words, with evidence. Claude never writes Tee's answers; unanswered questions say **Awaiting my answer**.

## Lesson notes

| # | Topic | Milestone | Status |
|---|---|---|---|
| 000 | [Project orientation](000-project-orientation.md) | M0 | Complete (gate passed 2026-10-14) |
| 001 | [Unity simulation loop](001-unity-simulation-loop.md) | M1 | Complete |
| 002 | [Vehicle dynamics](002-vehicle-dynamics.md) | M1 | Complete |
| 003 | Control and navigation | M2 | Not started |
| 004 | Sensors and observations | M3 | Not started |
| 005 | Computer vision | M4 | Not started |
| 006 | Tracking and estimation | M5 | Not started |
| 007 | Edge inference | M6 | Not started |
| 008 | Multi-agent systems | M8 | Not started |
| 009 | Experimental methods | M9 | Not started |

A note is created when its topic is introduced. When understanding improves, the note gains a dated "What changed in my understanding?" entry; earlier text is kept.

## Session log

### 2026-10-16/17 (practice, chat): Sheet 02 Q8 and stopping from top speed

- **What we built:** No code. Worked sheet 02 Q8 (a braking clamp) and a new question in chat: a drone at (20, 0, 0) m/s, top speed 20 m/s, a_max 10 m/s², dt 0.1 s, Control asks for (0, 0, 0). How many steps to stop?
- **What was covered:** `ClampMagnitude` gives a scale factor (limit ÷ magnitude) that multiplies every part of the vector; a value exactly at the limit passes unchanged (`length <= maxLength`); reaching top speed means cruising, not stopping; the request is clamped before acceleration.
- **Questions answered:** On Q8 step 3: "its -10 which is within a_max so no change", with ÷ dt done unprompted. On the stopping question I worked the whole chain myself: needed a = −200 m/s², "-200 is over a_max so it becomes -10ms2", v after one step (19, 0, 0), and 20 steps to stop.
- **Mistakes and corrections:** On Q8 step 1 I treated the scale factor (0.333) as the answer instead of multiplying by it, and said v was over the limit when it was a. Asked whether "allowed" was always 20 m/s (it is the request after the top-speed cap, so here (0, 0, 0)).
- **Evidence:** On the stopping question I did not make either of the slips recorded in session 7: I divided by dt and gave velocities as vectors.
- **Tests and results:** None (no code).

### 2026-10-15 (session 7): Limits, `Vec3` and 3D (M1-3 complete)

- **What we built:** Tests for braking, holding speed and a bad `dt`. A new shared folder `Core/Maths` with `Vec3` (I wrote `Magnitude` and the body of `ClampMagnitude`, test-first). `VehicleState` and `Step` switched to 3D; `Step` now caps the request at a top speed, then caps acceleration by magnitude. I wrote the hold, bad-dt and top-speed tests and converted three tests to 3D. **M1-3 is done.**
- **What was covered** (my understanding is evidenced only where the answers show it): failing fast vs quiet defaults; `Assert.Throws` and what makes it pass; one test class per class; why shared maths sits outside `Simulation` (ground truth stays private); test-first (red for the right reason, then green); refactoring under tests; magnitude and `ClampMagnitude`; "Control asks, Simulation decides" for the top speed.
- **Questions answered:** Chose throwing over a default dt, with a correct reason. Predicted the bad-dt test red (was green) and, with the guard removed, green (was red); then explained it correctly as a question. Explained why `Magnitude` tests were red before the fix. Explained what the refactor's green tests proved (first half). Explained in my own words why Simulation caps the request: "Control may ask and want a high speed but it must be bound to its limits." Details in [002](002-vehicle-dynamics.md) section 8.
- **Mistakes and corrections:** Said velocity 0 when `a` was 0. Thought dt=0 multiplies everything to 0 (it divides first). Put the dt test in its own file (moved). Gave lengths where vectors were asked ((6, 8, 0) → "5", velocity → "10m/s"). Forgot ÷ dt when working out `a` (the same slip as the practice sheet). Forgot calculator brackets for √. First top-speed test expected 10 on every axis.
- **Tests and results:** Unity Test Runner, Edit Mode, on my PC: 13 passed, 0 failed. Command-line run not repeated.
- **Files changed:** `Core/Maths/Vec3.cs` (new), `Core/Simulation/VehicleState.cs`, `Core/Simulation/VehicleDynamics.cs`, `Tests/EditMode/VehicleDynamicsTests.cs`, `Tests/EditMode/Vec3Tests.cs` (new); lesson 002, this journal, `docs/BACKLOG.md` (M1-3 ticked), `docs/architecture/overview.md` (Maths folder), `docs/testing/test-strategy.md`; formula sheet (magnitude) in the project files.
- **Decisions:** Own `Vec3` of `double` instead of Unity's `Vector3` or `System.Numerics.Vector3` (float). `Maths` is a shared folder that depends on nothing. `Step` throws on bad input rather than substituting defaults. The top speed is enforced inside Simulation by capping the request. Known weakness: `maxSpeed` and `maxAcceleration` can be swapped silently; a `VehicleLimits` struct may fix it later.
- **Blockers:** None.
- **Next smallest task:** M1-4 repeatability: the same seed and inputs give identical trajectories over 1,000 steps (Edit Mode test). Starts with a short lesson on what a seed is.
- **Retrieval question for next session:** A drone's velocity is (0, 6, 8) m/s and its top speed is 5 m/s. What velocity does `ClampMagnitude` give, and which way is it now moving in Unity?

### 2026-10-15 (practice, at work): M1 practice sheet 01

- **What we built:** No code. Worked through [the practice sheet](https://claude.ai/artifact/F698Yp1exTUqhE4bRZAAvL) on my phone, and asked for a formula sheet, now kept in the project files and updated as we go.
- **What was covered:** steps from duration (`N = duration ÷ dt`), the Euler step, `a × dt` as the change in velocity per step, braking with a negative `a`, steps to reach a speed, 3D speed and caps (preview).
- **Questions answered:** `N = duration ÷ dt` correct. Spotted myself that `((requested − v) ÷ dt) × dt` is just the gap, so the change per step is the gap, but no bigger than `a_max × dt`. Asked what happens when `a` is under `a_max` (it's used as is).
- **Mistakes and corrections:** Used dt as the speed change per step several times (0.1 instead of `a × dt` = 0.2). Called 25 steps "25 seconds". Fixed with hints each time; the dt slip came back in session 7.
- **Tests and results:** None (no code).

### 2026-10-14 (session 6): First Core code and test (M1-2 done, M1-3 started)

- **What we built:** The first Core module, `Core/Simulation`: `VehicleState` (1D position and velocity) and `VehicleDynamics.Step` (bounded acceleration, semi-implicit Euler, input checks). Edit Mode test `Step_FromRest_MatchesHandTrace` checks Trace B steps 1 to 3. I created the files from Claude's code and wrote steps 2 and 3 of the test.
- **What was covered** (my understanding is evidenced only where the answers show it): state vs `a_max` (asked again), operator order, what a unit test does, floating-point error and tolerances.
- **Questions answered:** Lesson 002 question 4 asked again: state correct, `a_max` misconception repeated, then fixed with scaffolding ("a_max caps acceleration", "requested speed alters a value"). Predicted the test would pass, twice; it failed both times, for different reasons. Explained in my own words why a tolerance is not cheating. Details in [002](002-vehicle-dynamics.md).
- **Mistakes and corrections:** Computed `10 + 10 × 0.1` as 2 (multiply first: 11). Left out the `Step` calls for steps 2 and 3, so the test checked step 1's state twice more. Suggested switching to `double` to fix rounding (already `double`; the fix is a tolerance).
- **Tests and results:** Unity Test Runner, Edit Mode, on my PC: all green (placeholder test and `Step_FromRest_MatchesHandTrace`). Command-line run not repeated.
- **Files changed:** `Core/Simulation/VehicleState.cs`, `Core/Simulation/VehicleDynamics.cs`, `Tests/EditMode/VehicleDynamicsTests.cs` (new); lesson 002, this journal, `docs/BACKLOG.md` (M1-2 ticked).
- **Decisions:** Start in 1D with `double`, as a `readonly struct` plus a static step function: easy to trace by hand. `Step` throws on `dt <= 0` or negative `a_max`. Decimal checks use a `1e-9` tolerance.
- **Blockers:** None.
- **Next smallest task:** Add tests for the limits: holding at 5 m/s (Trace B steps 4 to 6), a braking case, and `dt <= 0` throwing. Then move to 3D vectors with a magnitude speed limit (rest of M1-3).
- **Retrieval question for next session:** A drone moves at 8 m/s and is asked for 5 m/s, with `a_max = 10 m/s²` and `dt = 0.1 s`. What is its velocity after one step, and after two?

### 2026-10-14 (session 5): Lesson 002 model limits and 2D (M1)

- **What we built:** No code. Lesson note [002](002-vehicle-dynamics.md) now covers the model's limits and 2D vectors.
- **What was covered** (my understanding is evidenced only where the answers show it): what a point-mass kinematic model ignores; per-axis position update; vector magnitude; why a speed cap must use the magnitude.
- **Questions answered:** Retrieval (`a` at the requested speed) correct. Listed props, tilt and wind as ignored, correct. 2D step correct with help substituting. Diagonal-speed question correct unaided. "What is the state, and what stops speed growing?": mixed up step vs state and `a_max` vs top speed; to be asked again.
- **Mistakes and corrections:** See the last question above.
- **Tests and results:** None (no code).
- **Files changed:** `docs/learning/002-vehicle-dynamics.md`, this journal.
- **Decisions:** Speed limits cap the velocity's magnitude, not each axis.
- **Blockers:** None.
- **Next smallest task:** Answer lesson 002 question 4 fresh, then M1-3: create `Core/Simulation`, a `VehicleState` and `Step(dt)`, and the first real Edit Mode test from Trace B.
- **Retrieval question for next session:** In our model, what is the drone's state, and which rule stops its speed from growing forever?

### 2026-10-14 (session 4, continued): Lesson 002 started, vehicle state (M1)

- **What we built:** No code. Two hand-trace tables in lesson note [002](002-vehicle-dynamics.md).
- **What was covered** (my understanding is evidenced only where the answers show it): state (position, velocity); units m, m/s, m/s²; the step rule with velocity first; acceleration capped at `a_max` and dropping to 0 at the requested speed.
- **Questions answered:** Constant-velocity trace correct. Acceleration trace steps 2–3 correct unaided after help with units and step 1. Predicted the drone would reach 6 m/s at step 6 (misconception: kept accelerating past the requested speed), corrected with an explanation; not yet shown in my own answer.
- **Mistakes and corrections:** Units mixed up several times (m vs m/s vs m/s²).
- **Tests and results:** None (no code).
- **Files changed:** `docs/learning/002-vehicle-dynamics.md` (new), this journal.
- **Decisions:** Semi-implicit Euler (velocity first, then position) for the first model, to be tested in M1-3.
- **Blockers:** None.
- **Next smallest task:** Answer lesson 002 question 4, cover what the model leaves out, then plan M1-3 (first Core test from Trace B).
- **Retrieval question for next session:** A drone is already moving at the 5 m/s Control asked for, with `a_max = 10 m/s²` and `dt = 0.1 s`. What is `a` this step, and why?

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
