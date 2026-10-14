# Backlog (M0–M2)

The live backlog is [GitHub Issues](https://github.com/Teerasak-Mairoddee/AegisSim/issues) (labels `M0`, `M1`, `M2`, `lesson`, `learning-gate`; issues #1–#22). This file is the readable overview. Later milestones are added only when M2 is close to done.

## Milestone 0: Requirements and repository foundation

- [x] **M0-1 Push the repository foundation.** README, CLAUDE.md, `.gitignore`, `.gitattributes`, docs skeleton. *Done when:* A1, A3, A4 pass.
- [x] **M0-2 Create the Unity project** in `unity/AegisSim/` (6000.6.5f1, URP template). *Done when:* the project opens and `git status` is clean afterwards (A2).
- [x] **M0-3 Create the assemblies and a placeholder test.** `AegisSim.Core` (`noEngineReferences`), `AegisSim.Runtime`, `AegisSim.Tests.EditMode`, `AegisSim.Tests.PlayMode`; one Edit Mode test that asserts `true`. *Done when:* Test Runner shows 1 passed (A6), recorded in `test-strategy.md`.
- [x] **M0-4 Verify command-line test runs.** *Done when:* the batch-mode command in CLAUDE.md produces a results XML, or the doc is corrected.
- [x] **M0-5 Complete the orientation note.** Glossary, diagram, risks, compatibility plan, my answers (A5).
- [x] **M0-6 Learning gate.** I explain simulation vs perception vs tracking vs control vs coordination in my own words, recorded verbatim.

## Milestone 1: Unity arena and one vehicle

- [x] **M1-1 Lesson 001: Unity simulation loop.** `Update` vs `FixedUpdate`, fixed timestep, why our integration lives in Core.
- [x] **M1-2 Lesson 002: Vehicle dynamics.** State variables; kinematic model with bounded acceleration; what it omits.
- [ ] **M1-3 Vehicle state and integrator in Core.** *Done when:* Edit Mode tests show acceleration and speed limits hold, and a hand-traced 3-step table matches the test.
- [ ] **M1-4 Repeatability.** *Done when:* the same seed and inputs give identical trajectories over 1,000 steps (Edit Mode test).
- [ ] **M1-5 Bounded arena.** *Done when:* a test shows the vehicle can't leave the arena and the violation is reported.
- [ ] **M1-6 Runtime adapter and keyboard/debug control.** *Done when:* I can fly the vehicle with keys; a Play Mode test confirms the adapter applies Core state to the Transform.
- [ ] **M1-7 Debug visualisation.** Velocity vector, heading and arena bounds drawn in the scene.
- [ ] **M1-8 Reset control.** *Done when:* reset returns the run to the seeded initial state (test).
- [ ] **M1 Learning gate.** I explain the state variables, the update loop and the limits of the chosen model.

## Milestone 2: Multiple agents and safe navigation

- [ ] **M2-1 Lesson 003: Control and navigation.** Proportional control, gains, overshoot.
- [ ] **M2-2 Several independent agents.** *Done when:* N agents update independently with no shared state (test).
- [ ] **M2-3 Waypoint navigation.** *Done when:* an agent reaches each waypoint within tolerance and within a time limit (test).
- [ ] **M2-4 Static obstacles and avoidance.** *Done when:* no agent enters an obstacle's keep-out radius across seeded scenarios.
- [ ] **M2-5 Minimum separation between agents.** *Done when:* separation is held in a head-on test; violations are logged, not hidden.
- [ ] **M2-6 Telemetry and safety-violation log.** *Done when:* each run writes a JSON/CSV log with positions, commands and violations.
- [ ] **M2 Learning gate.** I explain how controller outputs affect movement and how constraints are tested.
