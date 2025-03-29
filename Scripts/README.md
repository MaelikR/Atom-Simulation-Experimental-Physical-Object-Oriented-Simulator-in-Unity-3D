# 🌌 Atom Simulation — Interactive, Networked, and Visual Particle Simulator in Unity 3D

Welcome to the **Atom Simulation Project**: an experimental Unity-based simulation that merges science, art, sound, and interactivity. Built with **Photon Fusion**, this project visualizes atoms and their behavior in a networked 3D world — with explosions, transformations, and living particles.

---

## ⚙️ Features

### 🔬 Scientific Core
- Real-time calculation of atoms in a liquid volume (mass, molar mass, density)
- Modular atom structure: **protons**, **neutrons**, **electrons** (with orbital motion)
- Dynamic energy system — atoms react to heat, cool, and magnetic forces

### 🎮 Interactive Mechanics
- **Thermal and cooling rays**: modify atoms' energy on click
- **Magnetic fields**: pull/push atoms with physics
- **Zoom System**: scale from puddle to nucleus in real-time
- **Camera follow**: track individual atoms or humanoids
- **Swimming + underwater mode** with floating particle behavior
- **First person player** with networked controls

### 🧪 Reactions & Evolution
- Fuse atoms to create **molecules**
- Agglomerate atoms into a **humanoid**
- Transform atoms into living **creatures** with behaviors
- Atoms can **mutate**, evolve, or explode based on energy buildup

### 💣 Nuclear Simulation
- Full **atomic bomb explosion** under water
- Shockwaves, energy bursts, atom destabilization
- Visual + sound feedback
- Trigger via interaction or chain reaction

### 🌌 Artistic Touch
- Atom glow + pulse based on energy
- Dynamic music generation based on atom motion
- Creatures born from atoms that swim and wander
- Stylized visuals (VFX, shaders, trails, underwater fog, distortion)

### 🌐 Multiplayer Ready (Photon Fusion)
- Fully networked atom spawning, interaction, and transformation
- RPC-synchronized music & explosions
- Shared energy events and magnet fields between players
- Modular `FusionBootstrap`, `PlayerSpawner`, and player prefab system

---

## 📁 Key Scripts Overview

| Script                     | Purpose                                 |
|----------------------------|-----------------------------------------|
| `Atom.cs`                 | Core atom data + energy system          |
| `AtomStructure.cs`        | Builds atomic structure (proton/neutron/electron) |
| `AtomMagnet.cs`           | Attracts atoms using magnetic fields    |
| `ThermalRay.cs`           | Adds heat to atoms                      |
| `AtomMusicSynth.cs`       | Generates audio based on atom speed     |
| `AtomCreature.cs`         | Turns atom into a small living entity   |
| `AtomSpawner.cs`          | Spawns atoms in space                   |
| `AtomicExplosionTrigger.cs`| Simulates an atomic bomb underwater     |
| `CameraFollowAtom.cs`     | Makes the camera follow an atom         |
| `FirstPersonCamera.cs`    | Full player controller with swim support |
| `PlayerSpawner.cs`        | Handles player instantiation in Fusion |
| `FusionBootstrapDebugGUI.cs`| Simple GUI to enter the world and launch Fusion |

---

## 🚀 How to Run

1. Clone the repository
2. Open in Unity (recommended version: 2022.3+)
3. Add your prefabs (atoms, electrons, creatures, humanoids)
4. Make sure the `NetworkRunner`, `PlayerSpawner`, and your `playerPrefab` are configured
5. Run in Play Mode or with Fusion Simulation Server
6. Use key **B** to trigger bomb 💥, or click on atoms to interact!

---

## 🤖 Credits & Vision

This simulation is developed as a **solo indie project** by a creator passionate about science, immersion, and stylized interactivity.  
It merges scientific curiosity with poetic representation of matter.

> “You're like a candle. You shine. The one with the atoms.”

---

## 📌 Roadmap (Coming Soon)
- Multiplayer co-op exploration
- Chemical bonding system
- Periodic table integration
- Advanced liquid deformation shaders
- Cinematic spawn intro + teleport particles
- Organism that forms from self-organizing atom clouds

---

## 📜 License

MIT — Feel free to fork, extend, remix (with credit appreciated).
