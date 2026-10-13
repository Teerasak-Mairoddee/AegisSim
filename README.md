# AegisSim

**Autonomous Counter-UAS and Swarm Intelligence Simulator.** Simulate. Detect. Coordinate. Learn.

AegisSim is an educational, simulation-first research platform built with Unity (C#) and Python. It simulates drone-like vehicles, detects, classifies and tracks them from simulated sensors, and studies safe navigation and multi-agent coordination, ending in repeatable swarm-versus-swarm experiments.

> **Simulation only.** AegisSim is a learning and research demonstrator. It does not connect to real aircraft, flight controllers or live sensor feeds. An "intercept" in AegisSim is only a virtual tag (proximity plus dwell time). Collisions are always counted as failures. Results from this simulator are not evidence about real-world systems.

## Status

Milestone 0 (requirements and repository foundation) in progress. No simulation code yet.

## Layout

| Path | Purpose |
|---|---|
| `unity/AegisSim/` | Unity 6000.6.5f1 (URP) project |
| `ml/` | Python dataset, training, evaluation and inference tools (from Milestone 3) |
| `tools/` | Helper scripts |
| `results/` | Experiment outputs (small summaries only; raw data stays out of Git) |
| `docs/learning/` | Learning journal |
| `docs/architecture/` | Architecture and data-flow docs |
| `docs/decisions/` | Architecture decision records |
| `docs/experiments/` | Experiment reports |
| `docs/testing/` | Test strategy and results |

## Requirements

- Unity **6000.6.5f1** with the Universal Render Pipeline
- Visual Studio 2022 or later with the Unity workload
- Git

## Documentation

- [Architecture overview](docs/architecture/overview.md)
- [Learning journal](docs/learning/README.md)
- [Backlog](docs/BACKLOG.md)
- [Test strategy](docs/testing/test-strategy.md)
