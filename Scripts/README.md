# 🌌 Atom Simulation — Emergent, Networked Evolution Simulator in Unity 3D

Welcome to the **Atom Simulation Project**: an ambitious, evolving Unity-based experiment where atoms come to life, evolve into creatures, and interact in a stylized underwater world. Built with **Photon Fusion**, this project explores **emergent life**, **environmental simulation**, and **player interaction** from the atomic scale up.

---

## ⚙️ Features

### 🔬 Atomic & Molecular Foundation
- Real-time atomic behavior: mass, energy, fusion/fission logic
- Modular atom structure: **protons**, **neutrons**, **electrons**
- Fusion into **molecules** and eventual transition to **living organisms**
- Energy, magnetism, and thermal states

### 🎮 Interactive & Physical Mechanics
- **Magnetic fields**, **heat/cold rays**, and orbital physics
- **Liquid simulation volume** with buoyancy and water drag
- **First-person controller** for exploration
- Interactive atom manipulation (click, raycast, field effects)
- Procedural spawning and dynamic **organism mutation system**

### 🧪 Evolution & Behavior System
- Atoms can transform into **LivingOrganism.cs** instances
- Organisms mutate into **fish**, **sharks**, or other life forms
- **FishBehavior.cs**: schooling, light-following, atom curiosity, player evasion
- **SharkBehavior.cs**: patrol, hunt, and kill fish using proximity checks
- Procedural aging, death, float-up system

### 🌿 Environment Dynamics
- Temperature/pressure system using depth-based **AnimationCurve**
- Comfortable zones for species: flee cold/hot zones
- Environmental stimuli: follow spotlights, flee FPS player, react to atoms
- Organisms prefer ideal depths and self-correct their swim altitude

### 🌊 Visual & Audio Artistry
- **Atom resonance** and glow
- **Music generation** from atom motion
- Morphing effects during mutation
- Bubble trails, underwater fog, seaweed motion, fish schooling
- Cinematic transitions, shader pulses, and energy distortion

### 🌐 Multiplayer-Ready (Photon Fusion)
- Networked spawning, mutation, and explosions
- Player sync and interactions (heat ray, magnet)
- Modular startup with `FusionBootstrapDebugGUI.cs`
- Scene persistence for player and atom states
🧬 SharkAtomSystem.cs — Molecular Composition of Creatures
This script simulates the molecular and atomic structure of a living organism, in this case a shark. It integrates with the Atom Simulation system to make even biological creatures scientifically inspectable and decomposable.

🔧 Features
Custom Molecular Composition:
Define a list of Molecule objects (name, atom count, molar mass) that represent the creature's body structure.

Total Mass Calculation:
Dynamically computes an approximate total molar mass of the shark based on its molecules.

Debug Output:
Use PrintComposition() to display a full breakdown of molecules and their respective atomic properties in the console.

Future Expansion:
This system is designed to work with:

Decomposition effects

Atom-based death visualizations

Real-time UI inspection via AtomInspector

Mutation/evolution events that change molecular content
---

## 📁 Key Scripts Overview

| Script                       | Purpose                                        |
|-----------------------------|------------------------------------------------|
| `Atom.cs`                   | Core logic: identity, energy, responses        |
| `LivingOrganism.cs`         | First stage of lifeform, can mutate           |
| `FishBehavior.cs`           | Schooling AI, stimuli response, temperature    |
| `SharkBehavior.cs`          | Predator AI: patrol, chase, attack             |
| `AtomMagnet.cs`             | Pull/push atoms with forces                    |
| `ThermalRay.cs`             | Raycast heat/cool atoms                       |
| `AtomMusicSynth.cs`         | Procedural audio from atom velocity           |
| `AtomSpawner.cs`            | Randomized atom instancing                    |
| `AtomVisualizer.cs`         | Atom spread visualization                     |
| `AtomCreature.cs`           | Morphs into animated creature from atom       |
| `AtomSimulator.cs`          | Controls reaction logic (fusion/fission)      |
| `PlayerSpawner.cs`          | Networked player instancing via Fusion        |
| `FusionBootstrapDebugGUI.cs`| Simple startup GUI and session launcher       |
| `CameraFollowAtom.cs`       | Follows atom or organism in-game              |

---

## 🚀 How to Run

1. Clone the repository
2. Open in **Unity 2022.3+**
3. Assign prefabs (atoms, organisms, fish, player)
4. Configure `FusionBootstrap`, `NetworkRunner`, and network prefab list
5. Play in **Single Player** or host a **Fusion Session**
6. Press **B** for bomb 💥 or use tools to interact with atoms

---

## 🤖 Simulation Flow Summary

1. `AtomSpawner` generates atoms randomly in space
2. Atoms pulse, gain energy, fuse, or become `LivingOrganism`
3. When energy depletes, the organism mutates into a random evolved prefab (e.g., `Fish` or `Shark`)
4. Fish group, respond to stimuli, and follow ecosystem rules
5. Sharks hunt fish via AI, resetting the life cycle

---

## 📅 Roadmap & Ideas

- ✨ Fish reproduction + eggs
- ❄️ Advanced water temperature shaders
- ✨ Atmospheric transitions (night, heat waves, storms)
- 🧕 Mutation into humanoid form with player control
- ⚖️ Chemistry reactions from nearby molecules
- 🎨 UI dashboard: inspect atoms, see structure, view mutation tree
- ✨ NPC jellyfish, plankton, or bio-luminescent entities
- 🌐 Networked player syncing and energy share

## 🧬 Creature Interaction: Communication & Reproduction (New)
Bioluminescent organisms (like jellyfish) now exhibit communication behaviors via pulsing lights during night cycles.

When two compatible creatures come close, they can emit synchronized pulses to simulate interaction.

If certain conditions are met (e.g., proximity + energy + time), a new offspring is instantiated in the simulation.

Includes:

Glow-based signaling system

Local reproduction logic with random spawn offset

Modular system for future: gender/pheromones/behaviors

This system opens doors to emergent ecosystems, where life spreads, signals, and adapts in your simulated ocean.

---

## 💼 License

MIT — Use, modify, and remix freely. Credit is appreciated but not mandatory.

> "What if atoms could dream? This simulation is their journey—from chaos to consciousness."

---

Developed with passion by **Maëlik "Light" Renaud** — Solo Developer, Systems Dreamer, Molecular Explorer.

