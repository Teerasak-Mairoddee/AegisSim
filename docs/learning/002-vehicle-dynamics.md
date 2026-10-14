# 002: Vehicle dynamics

**Date and milestone:** 2026-10-14, Milestone 1 (in progress: sections 6, 7 and the model's limits still to cover)

## 1. Concept

A vehicle's **state** is the small set of numbers that fully describes it at one moment. Given the state and the rule for one step, every later step can be worked out. Our first model is one-dimensional: position `p` and velocity `v`, with a limited acceleration.

## 2. Why it matters here

Every later module reasons about state: sensors observe it, tracking estimates it, control changes it. If the step rule is wrong or unbounded (a drone that accelerates forever), every result built on it is wrong too.

## 3. Mental model

Cruise control. Control sets the speed it wants; the vehicle accelerates no harder than its limit; once at that speed it takes its foot off the accelerator.

## 4. Technical explanation

| Quantity | Unit | Meaning |
|---|---|---|
| position `p` | m | where it is |
| velocity `v` | m/s | metres gained per second |
| acceleration `a` | m/s² | m/s of speed gained per second |

One step, in this order (velocity first, then position using the new velocity):

```
a     = (requested speed − v) ÷ dt, capped at ±a_max
v_new = v + a × dt
p_new = p + v_new × dt
```

Units check: m/s² × s = m/s (a change of speed); m/s × s = m (a change of position).

Updating velocity before position is called **semi-implicit Euler** integration. It is a modelling choice; it will be written down in the M1-3 code and tested.

## 5. Implementation walkthrough

Not built yet. M1-3 will put this step in Core and test it against the hand traces below.

## 6. Alternatives and trade-offs

Awaiting next session.

## 7. Common failure modes

- Applying `a_max` every step regardless of the target, so speed climbs forever (see question 3).
- Mixing up units (m, m/s, m/s²).

## 8. Experiment

Hand traces (no code).

**Trace A: constant velocity.** `p = 0`, `v = 5 m/s`, `dt = 0.02 s`.

| Step | Time (s) | Position (m) |
|---|---|---|
| 0 | 0.00 | 0 |
| 1 | 0.02 | 0.1 |
| 2 | 0.04 | 0.2 |
| 3 | 0.06 | 0.3 |

**Trace B: acceleration with a limit.** `p = 0`, `v = 0`, requested speed 5 m/s, `a_max = 10 m/s²`, `dt = 0.1 s` (chosen for easy paper maths; the simulation uses 0.02 s).

| Step | Time (s) | a (m/s²) | v (m/s) | p (m) |
|---|---|---|---|---|
| 0 | 0.0 | – | 0 | 0 |
| 1 | 0.1 | 10 | 1 | 0.1 |
| 2 | 0.2 | 10 | 2 | 0.3 |
| 3 | 0.3 | 10 | 3 | 0.6 |
| 4 | 0.4 | 10 | 4 | 1.0 |
| 5 | 0.5 | 10 | 5 | 1.5 |
| 6 | 0.6 | 0 | 5 | 2.0 |

Steps 1–3 are mine; steps 4–6 were filled in by Claude after question 3.

## 9. Comprehension questions

1. Trace A: positions after steps 1–3.
2. Trace B: velocity and position after steps 1–3.
3. Control asked for 5 m/s. At which step is 5 m/s reached, and what is `v` one step later?
4. *(Next session)* In your own words: what is the drone's state, and what stops its speed growing forever?

## 10. My answers

1. Asked first "0.04 = 0.2m?" (correct), then: "1 = 0.1m 3 = 0.3m". **Correct.**
2. Clarifying questions before answering: "does a - a_max which is 10/ms fixed?" (yes for these steps; unit is m/s²); "0+10*0.1=1m?" (the number is right but it is a velocity, 1 m/s); "is the 1 unit 1 m/s^2" (no, m/s); "for step 2 does v = 1m/s^2?" and "so a is constant?"; then "if time is 0.2 the value in v in v_new would derive from our answer prior which is velo = 1?" (yes).
   - Answer: "step 2 velo = 2 and position = 0.3, step 3 velo = 3 and position = 0.6". **Correct.**
   - *Note:* units (m vs m/s vs m/s²) needed several explanations; step 1's position needed a worked hint. Steps 2 and 3 done unaided.
3. "reaches 5m/s at step 5 and 6m/s for the step after"
   - *Feedback:* Step 5 correct. Step 6 is a misconception: once at the requested speed, `a = (5 − 5) ÷ 0.1 = 0`, so `v` stays 5 m/s.
   - *Second attempt:* "5+10x0.1?" (still applied `a_max`). Claude then explained with the cruise-control example and the `a` formula. **Status: not yet shown; revisit with question 4.**
4. Awaiting my answer.

## 11. Evidence

- Hand traces above, 2026-10-14.

## 12. Reflection

Awaiting my answer.

## 13. References

- Euler method (numerical integration): any introductory numerical methods text; to be added with the M1-3 code.

## 14. Date and milestone

2026-10-14, Milestone 1 (M1-2, in progress).

## What changed in my understanding?

Awaiting my answer.
