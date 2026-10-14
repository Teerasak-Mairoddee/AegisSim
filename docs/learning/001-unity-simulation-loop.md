# 001: Unity simulation loop

**Date and milestone:** 2026-10-14, Milestone 1

## 1. Concept

A simulation advances in steps. Each step moves the world forward by a time step **dt**. Unity offers two loops: `Update()` (once per drawn frame) and `FixedUpdate()` (on a fixed clock, every `Time.fixedDeltaTime` = 0.02 s by default).

## 2. Why it matters here

If the step size changed with the frame rate, the same experiment would give different results on different machines, or on the same machine on a busy night. Every later measurement (tracking error, separation violations, tag success) depends on runs being repeatable.

## 3. Mental model

A metronome and a dancer. `FixedUpdate` is the metronome: it ticks evenly whatever happens. `Update` is the dancer: it moves as fast as it can tonight. Our simulation follows the metronome.

## 4. Technical explanation

- `Update()` runs once per frame. `Time.deltaTime` (the last frame's duration) varies.
- `FixedUpdate()` runs every 0.02 s of game time. When a frame is slow, Unity runs it several times before drawing to catch up; when frames are fast, some frames get none.
- One simulation step: `new position = old position + velocity × dt`.
- **Real time** is the wall clock. **Simulated time** only advances when `Step(dt)` is called. Steps needed for a duration: `N = duration ÷ dt` (1 s ÷ 0.02 s = 50; 1 s ÷ 0.01 s = 100).
- Variable `dt` changes results: a jump integrated in `Update` can reach a slightly different height at 30 FPS than at 144 FPS.

## 5. Implementation walkthrough

Planned for M1-3 (not built yet): the step lives in **Core** as a plain method, e.g. `world.Step(dt)`. A Runtime MonoBehaviour's `FixedUpdate` calls it with the fixed 0.02 s. Core never asks Unity for the time, so an Edit Mode test can call `Step(0.02)` 50 times in a loop to simulate 1 second in a few milliseconds of real time.

## 6. Alternatives and trade-offs

- **Step in `Update` with `Time.deltaTime`:** smooth visuals, but results depend on frame rate. Rejected for simulation state.
- **Fixed step in `FixedUpdate` (chosen):** repeatable; visuals may look slightly stepped at high frame rates (interpolation can fix that later if needed).
- **Run Core in our own loop, independent of Unity:** what tests do; also useful later for fast batch experiments.

## 7. Common failure modes

- Putting simulation logic in `Update` (frame-rate dependent results).
- Reading `Time.time` or `Time.deltaTime` inside Core (breaks the Core/Unity wall and makes tests impossible).
- Very slow frames: many catch-up `FixedUpdate` calls per frame can make the editor stutter.

## 8. Experiment

**Question:** how many times do `Update` and `FixedUpdate` run in 1 second at 60 FPS vs 30 FPS?

**Method:** throwaway `LoopCounter` MonoBehaviour (in `Runtime/Experiments`, not committed). It set `vSyncCount = 0` and `Application.targetFrameRate`, then counted calls between t = 1 s and t = 2 s (the first second skipped to avoid uneven startup frames). I wrote the two counting lines (`updateCount++`, `fixedUpdateCount++`).

**My prediction:** "update would run them the same amount of times, fixed update only half at 30 fps?"

**Result (Tee's PC, 2026-10-14, Editor Play mode):**

| Target | `Update` calls | `FixedUpdate` calls |
|---|---|---|
| 60 FPS | 60 | 50 |
| 30 FPS | 30 | 50 |

**Conclusion:** the prediction was the wrong way round. `FixedUpdate` stayed at 50 per second (about two per frame at 30 FPS); `Update` followed the frame rate. One run per setting, in the editor; not a precise timing benchmark.

## 9. Comprehension questions

1. Compare the result with your prediction. What was the wrong way round, and why should the simulation step use the fixed clock?
2. An Edit Mode test has no `FixedUpdate`. How can it move a drone forward exactly 1 second of simulated time?
3. With `Step(0.01)`, how many calls make 1 second?
4. In your own words: why does AegisSim's step take a fixed `dt`, and what does that make possible for our tests?

## 10. My answers

1. "i the results the other way round. A fixed clock will produce the same amount of updates regardless of the frames so it is replicable on other systems with different performances."
   - *Feedback:* Correct. **Status: understood (shown by answer and experiment).**
2. **First attempt:** "it uses clock time instead since there are no frames to work against performance"
   - *Feedback:* Misconception: the test waits on no clock. Simulated time only advances when `Step(dt)` is called; a test calls it in a loop as fast as the CPU allows.
   - *Clarifying questions I asked:* "does 1 step equate to a second?" (no: one step = `dt`) and "if we call world.Step(0.02) that increments the loop by 0.02?" (each call advances simulated time by 0.02 s).
   - *Real-time part:* "0.008s" (much less than a second; correct idea).
   - *Steps for 1 s:* "5", then "5*0.02=0.1", then "10?". *Feedback:* 1.0 ÷ 0.02 = 50, matching the experiment's 50 `FixedUpdate` calls. Decimal step counting needed help.
3. "100" — correct.
4. "Fixed dt will produce consistent update results regardless of computing performance. Using frame rates will provide diminished updates depending on performance on alternate computing systems. We will have more consistent results using dt across different devices"
   - *Feedback:* Correct on repeatability. Missing the test half: because Core takes `dt` as an input and never reads Unity's clock, a test can step it directly, 50 calls for 1 s, in milliseconds. **Status: fixed-step repeatability understood; stepping Core from a test shown with help. Revisit when writing the first integrator test (M1-3).**

## 11. Evidence

- Experiment result above, run on my PC, 2026-10-14.

## 12. Reflection

Awaiting my answer.

## 13. References

- [Unity Manual: Order of execution for event functions](https://docs.unity3d.com/Manual/execution-order.html)
- [Unity Manual: Time](https://docs.unity3d.com/Manual/TimeFrameManagement.html)

## 14. Date and milestone

2026-10-14, Milestone 1 (M1-1).

## What changed in my understanding?

Awaiting my answer.
