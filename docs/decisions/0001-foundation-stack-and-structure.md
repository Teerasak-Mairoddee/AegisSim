# ADR 0001: Foundation stack, repository and code structure

- **Date:** 2026-10-12
- **Status:** Accepted
- **Decided by:** Tee (agreed in the project thread, 2026-10-12), proposed by Claude

## Context

Milestone 0 needs a repository, an engine version and a code structure before any simulation code exists. Tee's priority is a correct, explainable architecture.

## Decisions and options considered

### 1. Repository
- **Chosen:** new public GitHub repo `Teerasak-Mairoddee/AegisSim`, local clone at `C:\Users\teera\Documents\WarTechProjects\AegisSim`.
- Alternative: a folder inside RoverCore. Rejected: different goals and milestones would tangle the history.
- Alternative: private repo. Rejected for now: portfolio visibility is a goal (M10). Can be made private at any time.

### 2. Unity version
- **Chosen:** Unity 6000.6.5f1 with URP, already installed and used by RoverCore.
- Alternative: a newer or LTS editor. Not chosen: no current need, and a second editor costs disk space and setup time. Revisit if ML-Agents or Sentis requires a different version (unverified, check at M6/M7).

### 3. Code structure
- **Chosen:** `AegisSim.Core` (pure C#, `noEngineReferences`), `AegisSim.Runtime` (MonoBehaviours), `AegisSim.Tests.EditMode`, `AegisSim.Tests.PlayMode`. One folder and namespace per module inside Core, created when its milestone starts.
- Alternative: everything in the default `Assembly-CSharp`. Rejected: any script can reference any other, logic can't be tested without a scene, and every change recompiles everything.
- Alternative: one assembly per module from day one. Rejected as premature: more files to manage before we know where the real boundaries are. We split a module out when a test or dependency problem shows the need.

### 4. Python timing and tooling
- **Chosen:** no Python until Milestone 3; then `uv` with pinned versions and `pytest`.
- Alternative: set up Python now. Rejected: nothing in M0–M2 needs it, and unused tooling goes stale.

### 5. Backlog
- **Chosen:** GitHub Issues with milestone labels, starting with M0–M2.
- Alternative: `docs/BACKLOG.md` only. Kept as the drafting copy until the repo exists.

## Consequences

- Core can be unit tested in milliseconds without opening a scene.
- Runtime code must translate between Unity types (`Vector3`, `Transform`) and Core types, which is extra code we accept.
- Determinism is easier because vehicle integration lives in our own Core maths, not in Unity's physics engine.

## How we'll know it was wrong

- If most Core code ends up needing Unity types, the boundary is in the wrong place.
- If Edit Mode tests can't cover the important logic, too much logic lives in Runtime.
