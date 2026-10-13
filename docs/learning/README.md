# Learning journal

This journal records what Tee learned, in Tee's own words, with evidence. Claude never writes Tee's answers; unanswered questions say **Awaiting my answer**.

## Lesson notes

| # | Topic | Milestone | Status |
|---|---|---|---|
| 000 | [Project orientation](000-project-orientation.md) | M0 | In progress |
| 001 | Unity simulation loop | M1 | Not started |
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

### 2026-10-13: Unity project and first test (M0)

- **What we built:** Cloned the repo to my PC, created the Unity 6000.6.5f1 URP project in `unity/AegisSim/`, and created the four assemblies (`AegisSim.Core` with No Engine References, `AegisSim.Runtime`, `AegisSim.Tests.EditMode`, `AegisSim.Tests.PlayMode`) with a placeholder Edit Mode test. My first commits: `6b6568e`, `cefbca8`.
- **What I learned:** What `git clone`, `git log`, branches and `push -u` do; that `.gitignore` keeps generated folders out (A2); that an assembly definition is a wall that controls what code can reference; that No Engine References makes Unity invisible to Core at compile time; what `[Test]` and `Assert` do.
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
