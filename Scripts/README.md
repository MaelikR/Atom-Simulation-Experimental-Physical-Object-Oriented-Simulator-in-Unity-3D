# 🌌 Atom Simulation — Emergent, Networked Evolution Simulator in Unity 3D

Welcome to the **Atom Simulation Project**: an ambitious, evolving Unity-based experiment where atoms come to life, evolve into creatures, and interact in a stylized underwater and open-world environment. Built with **Photon Fusion**, this project explores **emergent life**, **environmental simulation**, **networking**, and **quantum traversal** from atomic chaos to conscious evolution.

---

## ⚙️ Features

### 🔬 Atomic & Molecular Foundation
- Real-time atomic behavior: mass, energy, fusion/fission logic
- Modular atom structure: **protons**, **neutrons**, **electrons**
- Fusion into **molecules** and eventual transition to **living organisms**
- Energy, magnetism, and thermal states

### 🎮 Interactive & Physical Mechanics
- **Magnetic fields**, **heat/cold rays**, and orbital physics
- **Liquid simulation** volume with buoyancy and water drag
- **First-person controller** for exploration
- Interactive atom manipulation (click, raycast, field effects)
- Procedural spawning and dynamic **organism mutation system**
  
### 🧪 Evolution & Behavior System
- Atoms can transform into **LivingOrganism.cs** instances
- Organisms mutate into **fish**, **sharks**, or other life forms
- **FishBehavior.cs**: schooling, light-following, atom curiosity, player evasion
- **SharkBehavior.cs**: patrol, hunt, and kill fish using proximity checks
- Procedural aging, death, float-up system
- New: **ButterflyBehavior.cs**, **ScarabBehavior.cs** with particle trails

### 🌿 Environment Dynamics
- Temperature/pressure system using depth-based **AnimationCurve**
- Comfortable zones for species: flee cold/hot zones
- Environmental stimuli: follow spotlights, flee player, react to atoms
- Organisms prefer ideal depths and self-correct altitude
- Insect life adapts to terrain height, humidity zones, and sunlight

### 🌊 Visual & Audio Artistry
- Atom resonance, glow, and **floating particles** in the air around butterflies
- **Music generation** from atom motion (procedural synth)
- Morphing effects during mutation
- Bubble trails, underwater fog, seaweed motion, particle ambiance
- Cinematic transitions, shader pulses, and energy distortion
- Full **voice intro sequence** with camera travelling & poetic narration

### 🌐 Multiplayer-Ready (Photon Fusion)
- Networked spawning, mutation, destruction
- Player sync and interactions (heat ray, magnet, scan)
- Modular startup with `FusionBootstrapDebugGUI.cs`
- Scene persistence for player/atom/organism states

---

## ⚡️ Intro & Preloading System
- Full **IntroCinematic.cs** handling camera travelling, text fades, and music
- `VoiceOnControl.cs`: triggers voice narration at player controller activation
- Preloading system for all heavy assets: fish, butterfly, scarab, molecules
- Built-in memory unload after intro for performance optimization

---

## 🐻 Land & Air Lifeforms

### 🐜 ButterflyBehavior.cs
- Flies within bounded zone
- Reacts to light and nearby atoms
- Leaves glowing particle trails (atom particles)
- Avoids player & strong wind zones

### 🦏 ScarabBehavior.cs
- Walks on terrain using raycasts
- Patrols, reacts to player & edible atoms
- Joins other scarabs in swarm patterns
- Burrows or dies over time

---

## ☢️ Atomic Bomb Module (Experimental)
- `AtomMagnet.cs`: attract atoms via physical magnetic field
- `AtomicExplosionTrigger.cs`: triggers explosion after overload
- Shockwave, glow, particle burst, radius heat
- Syncs across all clients via Fusion networking

---

## 🔮 Non-Euclidean Quantum Portal System

### 🚀 Features
- Linked portals with circular space folding
- Axis rotation offset to bend space
- Re-entry loops, recursive geometry
- Designed to break traditional Euclidean travel

---

## 📁 Key Scripts Overview

| Script                       | Purpose                                        |
|-----------------------------|------------------------------------------------|
| `Atom.cs`                   | Core identity, mass, charge, behavior          |
| `LivingOrganism.cs`         | Base class for emergent life                  |
| `FishBehavior.cs`           | Underwater AI: schooling, stimuli, depth logic |
| `ButterflyBehavior.cs`      | Flying insect AI with trail particles         |
| `ScarabBehavior.cs`         | Ground insect AI with terrain adherence       |
| `SharkBehavior.cs`          | Predator AI: hunt & kill logic                |
| `VoiceOnControl.cs`         | Triggers voice narration                      |
| `IntroCinematic.cs`         | Camera travel + text system                   |
| `AtomMagnet.cs`             | Magnetic field on atoms                       |
| `AtomicExplosionTrigger.cs` | Massive explosion trigger                     |
| `QuantumPortal.cs`          | Non-Euclidean teleportation                   |
| `FusionBootstrapDebugGUI.cs`| Network startup UI                            |
| `AtomMusicSynth.cs`         | Synth music from atom movement                |

---

## 🚀 How to Run

1. Clone the repository
2. Open in **Unity 2022.3+** (URP compatible)
3. Assign prefabs in `SimulationManager`
4. Setup `NetworkRunner`, player prefab, and `FusionBootstrap`
5. Press Play (Solo or Network Session)
6. Watch the intro, then control the player
7. Interact with atoms, portals, or fire the atomic bomb 💥

---

## 🌇 Roadmap & Wishlist

- ✨ Dynamic day/night + seasonal biome logic
- 💡 Insect reproduction and swarming logic
- 🚀 Underwater drone scanner tool
- 🛠️ Atom crafting UI (combine & create molecules)
- ✨ Procedural terrain + atom caves
- 🌿 Algae, plankton, and bio-luminescence
- ✨ Mutation into humanoids with symbolic intelligence
- 🚀 Genetic tree and trait evolution via UI

---
☢️ Quantum Anomaly Zone — Time Distortion Field
This module introduces localized time dilation and compression based on atomic mass. It simulates quantum anomalies in specific zones of the simulation, affecting organisms' behavior, aging, and audio perception.

🌀 Features
Localized TimeScale Control: Slows down or speeds up time only for organisms inside the anomaly.

Atomic Mass Sensitivity: Organisms experience different rates of time based on their molecular composition (e.g., heavier = slower aging).

Underwater-style Sound Filter: Automatically applies an AudioLowPassFilter to creatures entering the anomaly, creating an immersive muffled audio effect.

Modular and Extensible: Easily plug into any collider zone. Customizable with AnimationCurve to tailor how mass influences time.

🎮 Gameplay Use Cases
Slow Motion Zones: Simulate temporal anomalies, black holes, stasis fields.

Evolution Pacing: Some organisms evolve faster/slower based on their atomic makeup.

Narrative Portals: Passing through the anomaly can act as a cinematic effect or a puzzle mechanic.

Sound Design: Adds realism and a dreamy atmosphere to slowed-down areas.

🧪 Technical Summary
Component	Description
QuantumAnomalyZone.cs	Trigger zone that applies time and audio distortion based on atom mass.
LivingOrganism.cs	Must expose SetLocalTimeScale() and GetAtomicMass() methods.
AudioLowPassFilter	Unity built-in component to simulate underwater or dreamlike sound.
AnimationCurve	Optional curve to map atomic mass to time factor (e.g., light = fast, heavy = slow).
⚙️ How to Use
Create a GameObject with a Trigger Collider (e.g., Sphere or Box).

Attach the QuantumAnomalyZone.cs script.

Assign a timeScale value (< 1 to slow down, > 1 to speed up).

Optionally assign a distortion curve to modulate time effect per atomic mass.

Ensure your creatures have a LivingOrganism script with SetLocalTimeScale() and a valid GetAtomicMass().

💡 Tips & Expansion Ideas
Combine with post-processing effects (Chromatic Aberration, Blur) for full quantum immersion.

Use with Timeline for slowed intros or boss sequences.

Add visual feedback: swirling particles, refractive distortion.

Extend QuantumAnomalyZone to affect atoms individually (e.g., vibrate faster).

“Time bends here... as if the atoms themselves are dreaming.”


## 🌍 License
**MIT** — Free to use, remix, and publish. Credit appreciated.

> "What if atoms could dream? This simulation is their journey—from chaos to consciousness."

---
Created with 💡 by Maëlik "Light" Renaud
— Temporal Designer & Molecular Dreamweaver
