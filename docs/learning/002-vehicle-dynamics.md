# 002: Vehicle dynamics

**Date and milestone:** 2026-10-14, Milestone 1 (complete: question 4 answered with help in session 6; code and first test in M1-3)

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

Built in session 6 (M1-3, 1D only), in `Core/Simulation` (namespace `AegisSim.Simulation`, the first Core module folder):

- `VehicleState`: a `readonly struct` holding `Position` (m) and `Velocity` (m/s). Read-only so a state can't be changed by accident; a step returns a **new** state instead.
- `VehicleDynamics.Step(state, requestedVelocity, maxAcceleration, dt)`: a static method doing the three lines in section 4. It throws `ArgumentOutOfRangeException` if `dt <= 0` or `maxAcceleration < 0`, so a bad input fails loudly instead of producing nonsense.
- It never reads Unity's clock: `dt` is passed in. That is what lets an Edit Mode test step it directly (lesson 001).

Test: `Tests/EditMode/VehicleDynamicsTests.cs`, `Step_FromRest_MatchesHandTrace`, checks steps 1 to 3 of Trace B. I filled in steps 2 and 3.

**Session 7 (2026-10-15): limits and 3D (M1-3 complete).**

- `Core/Maths/Vec3.cs` (namespace `AegisSim.Maths`, new shared folder): a `readonly struct` with `X`, `Y`, `Z` as `double`, `+`, `-`, `* number`, `/ number`, `Magnitude` (I wrote it) and `ClampMagnitude(maxLength)` (I wrote the body). Not Unity's `Vector3`: Core has No Engine References, and `Vector3` uses `float`. It lives outside `Simulation` so other modules can use vectors without reaching into the module that owns ground truth.
- `VehicleState` now holds a `Vec3 Position` and `Vec3 Velocity`.
- `Step(state, requestedVelocity, maxSpeed, maxAcceleration, dt)`:
  1. `allowedVelocity = requestedVelocity.ClampMagnitude(maxSpeed)`: Control only asks; Simulation enforces the top speed.
  2. `neededAcceleration = (allowedVelocity − v) ÷ dt`, then `ClampMagnitude(maxAcceleration)`: the 3D version of clamping between −a_max and +a_max (keep the direction, limit the length).
  3. Velocity then position, as before. Throws on `dt <= 0`, or a negative `maxSpeed` or `maxAcceleration`.
- Tests: `VehicleDynamicsTests` (hand trace, braking, holding, bad dt, top speed) and `Vec3Tests`. The 1D tests were rewritten to fly along x with the same expected numbers, which is how the refactor was shown not to change behaviour.
- Known weakness: `maxSpeed` and `maxAcceleration` are two plain numbers side by side and could be swapped without a compiler error. Candidate fix later: a small `VehicleLimits` struct.

## 6. Alternatives and trade-offs

Our model is a **point-mass kinematic model with bounded acceleration**: the drone is a dot (no shape or rotation), and motion is described directly rather than by simulating forces.

| Real quadcopter | Our model |
|---|---|
| Speeds up by changing prop speeds and tilting | Velocity changes directly; no props, no tilt |
| Tilting takes time, so acceleration is delayed | Acceleration is instant |
| Wind pushes it | No wind (could be added later) |
| Air drag slows it and sets top speed | No drag |
| Thrust must also hold it up against gravity | No gravity |
| Mass, battery and motor limits | All folded into `a_max` |

Why it is acceptable: M1–M9 study navigation, separation, sensing and tracking, which need position and velocity, not flight dynamics. Not modelling real flight control is also deliberate under the safe scope: AegisSim is never a flight controller.

**2D/3D:** the same rules apply per axis (`x_new = x + v_x × dt`, `y_new = y + v_y × dt`). Speed is the vector's **magnitude**, `√(v_x² + v_y²)`. A speed limit must cap the magnitude (shrink the arrow, keep its direction), not each axis separately.

## 7. Common failure modes

- Applying `a_max` every step regardless of the target, so speed climbs forever (see question 3).
- Mixing up units (m, m/s, m/s²).
- Capping each axis separately: velocity (5, 5) passes a per-axis cap of 5 but moves at √50 ≈ 7.07 m/s (the classic game bug of faster diagonal movement).
- Confusing `a_max` (limits how fast speed changes) with a top speed (limits speed itself).
- Comparing decimals exactly. A `double` stores 0.1 slightly off (binary can't write 0.1 exactly, as decimal can't write 1/3), so 0.1 + 0.2 = 0.30000000000000004. Decimal checks use a tolerance: `Assert.AreEqual(0.3, actual, 1e-9)`. This is correct practice, not a weaker test.

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

**Code check (session 6, 2026-10-14):** the Edit Mode test runs steps 1–3 of Trace B through `VehicleDynamics.Step`.

- *Prediction 1:* "it should pass assuming its doing the same calculations". *Result:* failed. I had written the step 2 and 3 checks but not the `Step` calls, so it was still checking step 1's state. I spotted the missing line with a hint ("state line?").
- *Prediction 2:* "They should pass". *Result:* failed: "the values are different by a fraction of a decimal point" (floating-point error, section 7).
- *Fix:* asked how to fix it, I first said "use a double as a variable so it unifies the variable" (misconception: it already uses `double`, which is the source of the error). After the ruler explanation I asked how to write the gap; Claude gave line 21, and I changed the 0.6 check myself. *Result:* all green.
- *Comprehension check, "isn't adding `1e-9` cheating?":* "I would say the margin was minimal and its just a nuance of the mathematics. It does not change the result of what happens it just closes the miscommunication between the tested values and the answer."
  - *Feedback:* Correct. Added: the test still catches real bugs, because a real mistake (for example 0.1 m out) is millions of times bigger than the 1e-9 gap. **Status: understood (own words).**

**Code checks (session 7, 2026-10-15, M1-3):**

- *Hold speed:* started at 1.5 m and 5 m/s, requested 5 m/s. Positions "2.0m, 2.5m and 3.0m" correct; first said velocity 0 ("within size of a_max"), then "5m/s for all". Correction: `a` is 0 because the gap is 0; `a_max` plays no part. I wrote the whole test myself, correct first time.
- *Bad dt, design:* first suggested "a default value of dt if dt=0" and thought dt=0 makes everything 0 (it divides in the first line). Claude showed `double` gives `Infinity`, so the drone silently freezes. Asked A (quiet default) or B (throw): "option B since we might not notice a quiet swap during test." **Correct.**
- *Bad dt, test:* wrote it with `Assert.Throws` in a separate file and class; predicted red, result green. Why green: "Assert checks if it fails which resulted in a positive failure". Experiment, guard commented out: predicted green, result red (`But was: null`). After explanation (only the `throw` line creates an error): "so Assert.Throws only works if throw new ArgumentOutOfRangeException(...) tells it that its an error?" **Correct.**
- *Where tests go:* "it would be hard to keep track of all the tests relating to VehicleDynamics if they are separated amongst other test categories". Moved into `VehicleDynamicsTests`. Convention: one test class per class, methods `Method_Situation_ExpectedResult`.
- *Where `Vec3` goes:* proposed "another read only struct like VehicleState but provide 3 parameters" (the chosen design). On putting it in `Simulation`: "we would have too many references for the same vec3 value across different modules" (partly: one shared definition, yes; the main reason, not exposing ground truth to other modules, was explained by Claude).
- *Magnitude, test-first:* (3, 4, 0) → 5 correct. Predicted all four `Vec3` tests green; the two magnitude tests were red (`NotImplementedException`). Why: "magnitude was not implemented and instead threw an error". Wrote `Math.Sqrt(X * X + Y * Y + Z * Z)`; green.
- *ClampMagnitude:* (6, 8, 0) capped at 5: said "5" (the length), then "3,4,0". **Correct.** Wrote the method body from the step comments; 11 green.
- *3D acceleration cap:* "I am unsure, convert them all to magnitudes?" (would lose direction). Claude gave `ClampMagnitude(maxAcceleration)`.
- *Refactor:* converted three tests to `Vec3` myself; predicted all green, 12 green. What it proves: "we validated that 1d tests work with refactoring it after we can prove that the results are repeatable with the same 1D elements in a 3D structure". Correct for behaviour preserved; Claude added why doing it without tests would have been risky.
- *Top speed:* request (30, 0, 40), top speed 10, a_max 100, dt 0.1. Speed asked: "1600" (forgot brackets), then "50". Capped request "6,0,8". `a`: "10" (forgot ÷ dt, the same dt slip as the practice sheet), then "100". Velocity "10m/s" (the speed; as a vector (6, 0, 8)). First test version expected 10 on every axis; fixed to (6, 0, 8) plus magnitude 10. Predicted green; 13 green.
- *Why cap the request in Simulation?* "because the drone will have limits. and human error may occur which will alter results. Control may ask and want a high speed but it must be bound to its limits. capping the steps will simulate this."
  - *Feedback:* Correct. Added: the limit belongs to the vehicle, so Simulation enforces it whoever is asking. A buggy Control, or later a trained ML policy, can't get a physically impossible speed, so results stay honest. **Status: understood (own words).**

## 9. Comprehension questions

1. Trace A: positions after steps 1–3.
2. Trace B: velocity and position after steps 1–3.
3. Control asked for 5 m/s. At which step is 5 m/s reached, and what is `v` one step later?
4. In your own words: what is the drone's state, and what stops its speed growing forever?
5. What does our model leave out compared with a real quadcopter?
6. 2D: from `p = (0, 0)` with `v = (3, 4)` m/s and `dt = 0.1 s`, where is the drone after one step?
7. If each axis is capped at 5 m/s, velocity (5, 5) is allowed. Faster or slower than 5 m/s?

## 10. My answers

1. Asked first "0.04 = 0.2m?" (correct), then: "1 = 0.1m 3 = 0.3m". **Correct.**
2. Clarifying questions before answering: "does a - a_max which is 10/ms fixed?" (yes for these steps; unit is m/s²); "0+10*0.1=1m?" (the number is right but it is a velocity, 1 m/s); "is the 1 unit 1 m/s^2" (no, m/s); "for step 2 does v = 1m/s^2?" and "so a is constant?"; then "if time is 0.2 the value in v in v_new would derive from our answer prior which is velo = 1?" (yes).
   - Answer: "step 2 velo = 2 and position = 0.3, step 3 velo = 3 and position = 0.6". **Correct.**
   - *Note:* units (m vs m/s vs m/s²) needed several explanations; step 1's position needed a worked hint. Steps 2 and 3 done unaided.
3. "reaches 5m/s at step 5 and 6m/s for the step after"
   - *Feedback:* Step 5 correct. Step 6 is a misconception: once at the requested speed, `a = (5 − 5) ÷ 0.1 = 0`, so `v` stays 5 m/s.
   - *Second attempt:* "5+10x0.1?" (still applied `a_max`). Claude then explained with the cruise-control example and the `a` formula. 
   - *Follow-up:* asked for another explanation ("why does requested speed change our a value"); Claude explained it as closing a speed **gap** (`gap ÷ dt`, capped at `a_max`; negative gap means braking).
   - *My own example:* "so if the requested speed is 8m/s but our current speed is at 5m/s the values would account for the 3m/s difference in the request by accelerating but is capped at how hard it can push?"
   - *Feedback:* Correct: needs 30 m/s², capped at 10, so 6, 7, then 8 m/s and holds. **Status: understood (own example, after re-explanation). Recheck with the retrieval question.**
   - *Retrieval check (2026-10-14, session 5): "already at the requested 5 m/s; what is a?":* "a  would be: (requested speed - current speed) / dt so (5m/s-5m/s)/0.1 so a = 0". **Correct; confirms question 3.**
4. Clarified "drones state regarding what point?" (any single moment). Answer: "step will determine our state and a_max caps the speed from going up forever"
   - *Feedback:* Two mix-ups. The step *changes* the state; the state is **position and velocity**. `a_max` limits how quickly speed changes, not the speed itself; the requested speed (gap → 0) stops it, plus a magnitude limit in 2D. **Status: not yet shown; ask again at the start of next session (answered late in a long evening).**
   - *Asked again (session 6, 2026-10-14):* "Velocity & position, the value a" (state: position and velocity correct; `a` is worked out each step, not part of the state). Then "maximum acceleration value caps the speed" (same misconception).
   - *Worked check, Trace B continued:* "step 3 = 3m/s and step 100 should be 0 because its already reached its max a value at this point", then "10", "2", and "based on 100+10*0.1 = 101". Arithmetic slip corrected: multiply before adding, so `10 + 10 × 0.1 = 11`, not 2. Claude explained with the bucket-and-tap analogy (`a_max` is how far the tap opens; the requested speed is the fill line).
   - *Final answers:* "a_max caps acceleration" and "requested speed alters a value". **Correct, after substantial scaffolding. Status: accepted with help; revisit with a new example (a braking case) in M1-3's limit tests.**
5. "To speed up it would have to increase prop speed and adjust the different props to angle itself to drive forward. Turning and direction is dictated by what prop is at what speed. This can be effected by Wind which i assume our model ignores unless we program it"
   - *Feedback:* Correct (props, tilt, wind). Added: tilt delay, drag, gravity, mass and motor limits. **Status: understood for the parts named.**
6. Asked for the formulas again, then "x and y = 0?", then given the substituted lines: "0.3 and 0.4". **Correct, with help substituting values.**
7. "5^2+5^2=50 so root of 50 would be around 7 and 7 > 5 so faster". **Correct, unaided.**

## 11. Evidence

- Hand traces above, 2026-10-14.
- `VehicleDynamicsTests.Step_FromRest_MatchesHandTrace` passes in the Unity Test Runner on my PC (session 6, 2026-10-14).
- 13 Edit Mode tests pass in the Unity Test Runner on my PC (session 7, 2026-10-15): all 1D limits, `Vec3`, and the 3D top-speed cap.

## 12. Reflection

Awaiting my answer.

## 13. References

- Euler method (numerical integration): any introductory numerical methods text.
- NUnit `Assert.AreEqual(expected, actual, delta)`: the tolerance overload for `double`.

## 14. Date and milestone

2026-10-14, Milestone 1 (M1-2 complete; first code in M1-3). M1-3 completed 2026-10-15 (session 7).

## What changed in my understanding?

Awaiting my answer.
