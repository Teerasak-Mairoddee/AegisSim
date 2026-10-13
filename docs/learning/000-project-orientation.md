# 000: Project orientation

**Date and milestone:** 2026-10-12, Milestone 0

## 1. Concept

AegisSim is a pipeline. Each simulation step, the world advances, sensors produce imperfect observations, perception and tracking turn those into beliefs, a controller acts on the beliefs under safety constraints, and evaluation compares everything with ground truth.

## 2. Why it matters here

Every later milestone adds one stage of this pipeline. Knowing where a stage sits, and what it is and is not allowed to see, is what keeps the code separable and testable.

## 3. Mental model

Think of a referee and players. The **simulation** is the referee: it knows exactly where everything is. The **agents** are players with poor eyesight: they only know what their **sensors** report. **Perception** says "I think that's a drone." **Tracking** says "that's the same drone I saw a second ago, and it's moving left." **Control** says "move here without hitting anyone." **Coordination** says "you take that one, I'll take this one." **Evaluation** is the referee's scorebook.

## 4. Technical explanation

### Glossary (initial)

| Term | Meaning in AegisSim |
|---|---|
| Ground truth | The simulator's exact state of every object. Only Simulation and Evaluation may read it. |
| Observation | A timestamped, possibly noisy, delayed or missing sensor output. |
| Detection | "Something is at this place in this frame," with a confidence score. |
| Classification | Which kind of object a detection is: drone-like, bird-like, other, or unknown. |
| Track | A detection history linked into one identity over time, with an estimated position and velocity. |
| State estimation | Estimating quantities you can't observe directly (such as velocity) from noisy observations. |
| Controller | Converts a desired motion into control commands the vehicle model accepts. |
| Kinematic model | Moves a body by setting velocity or acceleration directly, without simulating forces. |
| Dynamic model | Moves a body by simulating forces, torques and mass. |
| Fixed timestep | Physics advances in equal time steps (for example 0.02 s), independent of frame rate. |
| Deterministic | The same seed and configuration produce the same run, step for step. |
| Virtual tag | AegisSim's only "intercept": an agent stays within a set distance of a target for a set time. |
| Minimum separation | The smallest allowed distance between agents or obstacles. Breaking it is a safety violation. |
| Latency | Delay between when something happens and when a module learns about it. |
| Edge inference | Running a model on limited hardware with time, memory and power budgets. |
| Assembly (`.asmdef`) | A unit of compiled C# in Unity; controls which code can reference which. |
| ADR | Architecture decision record: a short note of a decision, its options and why. |

### Architecture diagram

See [architecture overview](../architecture/overview.md).

### Risks and assumptions

| Risk or assumption | Impact | Mitigation |
|---|---|---|
| Limited weekday time (late sessions) | Slow progress; tired sessions cause mistakes | 60–90 min sessions ending at a committed checkpoint |
| Unity physics is not bit-for-bit deterministic across machines | Repeatability tests could be flaky | Keep vehicle integration in engine-free Core with our own fixed-step maths |
| ML-Agents / Sentis compatibility with Unity 6000.6.5f1 unverified | Could block M6/M7 tooling | Verify before adopting; ONNX Runtime in Python is a fallback |
| 16 GB RAM | Unity and training together may swap | Don't run them simultaneously; small models first |
| Synthetic-to-real gap | Results could be over-interpreted | Every report states what it does not show |
| Scope creep toward later, more exciting milestones | Weak foundations | Milestone order and learning gates are enforced |
| Previous projects were mostly AI-written | Overestimating my own skill | Progress is judged by what I can explain, not by what code exists |

### Compatibility plan

| Component | Version | Status |
|---|---|---|
| Unity Editor | 6000.6.5f1 (URP 17.6) | Installed (used by RoverCore) |
| Unity Test Framework | 1.8.0 (as in RoverCore) | Installed with Unity project template |
| Visual Studio | Installed (version to record) | Installed |
| Python | via `uv`, version chosen at M3 | Not needed yet |
| PyTorch | chosen at M4, CUDA build for RTX 4060 | Not checked |
| ML-Agents | chosen at M7 | Not checked |
| Sentis or ONNX Runtime | chosen at M6 | Not checked |

## 5. Implementation walkthrough

No code yet. Files created in M0: README, CLAUDE.md, `.gitignore`, `.gitattributes`, this journal, ADR 0001, test strategy, backlog.

## 6. Alternatives and trade-offs

See ADR 0001 for alternatives to the repo layout, the assembly structure and the backlog tool.

## 7. Common failure modes

- Committing `Library/` (thousands of generated files, huge repo). Prevented by `.gitignore`; checked by acceptance test A2.
- Forgetting `.meta` files (broken references after cloning). `.gitignore` explicitly keeps them.
- Letting Unity types leak into Core (logic that can't be tested without a scene). Prevented by `noEngineReferences`.

## 8. Experiment

Open RoverCore and find one place where a Unity type would have been convenient inside `RoverCore.Core`. What would break if you added it?

## 9. Comprehension questions

1. **Learning gate:** In your own words, what is the difference between simulation, perception, tracking, control and coordination?
2. Which module is allowed to read a vehicle's true position, and why are the others not?
3. Why does `.gitignore` exclude `Library/` but keep `.meta` files?
4. (Experiment, 2026-10-13) A script in `AegisSim.Core` (No Engine References ticked) declares a `Vector3` field. What happens when you save it?
5. Why do we *want* the compiler to stop us using Unity inside Core?

## 10. My answers

### Background (2026-10-12, recorded from my messages)

- **Unity:** "I have built FPS games and puzzle games using unity, we also have another project Called RoverCore"
- **C#:** "I've used C# to script player controllers and animation controllers, used lots of forums and stack overflow to find and jerry rig solutions but never fully understood code architecture. It is important that this project has the correct file structure and architecture."
- **Python:** "small scripts, completed coursework at school with it. Mostly used AI to code python recently for Cyber security"
- **Machine learning:** "I know very minimal, i have used training for churn but i do not know much about it only the concept"
- **Robotics:** "Learning robotics". Prior project: RoverCore (Milestone 1 complete; Claude wrote the C# code).
- **Hardware:** "13th gen i7-13700F 16Gb of ram and RTX 4060 8Gb"
- **Time:** "i have 40 hrs a week at work and go to the gym, 11:30-20:00 shifts and 9:00-17:00 shifts then an hour of gym, cook sleep and then projects usually till late - i need to fix sleep schedule"
- **Teaching style:** B (short explanation, then I try first)

### Comprehension questions

1. Awaiting my answer.
2. Awaiting my answer.
3. **First attempt (2026-10-13):** ".meta may include dependencies to build the libraries"
   - *Feedback:* Partly right. `Library/` is indeed rebuilt from what's committed, so it never needs to be in Git. The missing piece: each `.meta` file holds the asset's **GUID** (its permanent ID) and its import settings. Scenes and prefabs refer to assets by GUID, not by file name, so a missing `.meta` makes Unity generate a new GUID on the next machine and every reference to that asset breaks.
   - *Second attempt (2026-10-13), answering "a material's `.meta` is missing; what does a teammate see?":* "they see missing references / no assets/asset names"
   - *Feedback:* Correct on the key point: the scene shows **missing references** (a material slot reads "Missing", and URP draws the object magenta). One refinement: the material file itself is still in the Project window with its name. Unity just gives it a brand-new GUID, so nothing that pointed at the old GUID can find it. **Status: understood (shown by answer).**

4. **Prediction 1:** "this takes control of its world space position as an override?"
   - *Feedback:* Misconception. `Probe` is a plain class, not a MonoBehaviour, so it never runs; the experiment is about compiling.
   - **Prediction 2:** "unsure". Also noted before saving: "its setting a vector 3 without any values or references on what v is". *Feedback:* accurate reading; an unset `Vector3` field is legal and defaults to (0, 0, 0), so that isn't what matters here.
   - **Result:** "unity engine could not be found" (compile error as soon as the file was saved). The wall works: Core cannot see `UnityEngine`.
5. **Answer (after asking for clarification on what Core holds and what a test is):** "building code with the wrong environment. Avoids writing realtime events in during compilation which takes longer"
   - *Feedback:* Partly right. "Wrong environment" is the key idea: Core logic must not depend on a scene. Separate assemblies do recompile faster, a real bonus. The main reason is testing: Edit Mode tests can call Core directly and check results in milliseconds, with no scene or Play mode. **Status: partly understood; revisit at M1 when the first real Core test is written.**

## 11. Evidence

- Agreement to the plan: project thread, 2026-10-12 ("agreed").
- Unity project committed by me: `6b6568e` (branch `m0-unity-setup`).
- Assemblies and placeholder test committed by me: `cefbca8`.
- Acceptance tests A2 and A6 passed on my PC, 2026-10-13 (see `docs/testing/test-strategy.md`).

## 12. Reflection

Awaiting my answer.

## 13. References

- [Unity Manual: Assembly definitions](https://docs.unity3d.com/Manual/assembly-definition-files.html)
- [Unity Test Framework documentation](https://docs.unity3d.com/Packages/com.unity.test-framework@latest)
- [Michael Nygard, "Documenting Architecture Decisions"](https://cognitect.com/blog/2011/11/15/documenting-architecture-decisions)

## 14. Date and milestone

Started 2026-10-12, Milestone 0.
