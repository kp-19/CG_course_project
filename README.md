# 🌊 Ocean and Island Exploration Game

A 3D exploration and survival game built using Unity where players navigate an open ocean, explore procedurally generated islands, battle enemies, and collect resources to survive and progress.

## 🎮 Demo & Repository

- 📹 [Watch Demo Video](https://youtu.be/sampledemo123)  
- 💻 [GitHub Repository](https://github.com/teamOceanExplorers/OceanIslandGame)

---

## 🧭 Features

- 🌴 **Procedural Island Generation** using Perlin Noise for varied environments  
- 🚢 **Realistic Ship Physics** with buoyancy and collision response  
- ⚔️ **Enemy AI** that detects, chases, and attacks players  
- 🎒 **Loot & Inventory System** for resource collection and survival  
- 🗺️ **Player-Ship Interaction** allowing seamless transitions between sailing and on-foot exploration  
- 🖥️ **Minimal UI System** to display health, inventory, and game state

---

## 🛠️ Scripts Overview

- `BuoyancyObject.cs` – Simulates floating behavior of objects on water  
- `FallOffGenerator.cs` – Adds smooth edges to islands using falloff maps  
- `IslandGenerator.cs` – Procedurally places and shapes islands in the world  
- `IslandDisplay.cs` – Renders generated island terrain in the scene  
- `MapGenerator.cs` – Generates heightmaps and noise maps for island creation  
- `Noise.cs` – Core noise functions for procedural generation  
- `ShipController.cs` – Handles ship movement, rotation, and docking logic  
- `MountingMechanics.cs` – Manages transitions between ship control and player movement  
- `TextureGenerator.cs` – Converts terrain data into textures for islands  
- `MeshRenderer.cs` – Builds mesh from height data to shape island geometry

---

## 🎮 Controls

### Ship Mode
- Arrow Keys – Move/Rotate Ship  
- `Space` – Switch to Player Mode  

### On-Foot Mode
- `WASD` – Move  
- `Space` – Jump  
- `Left Click` – Attack  
- `F` – Collect Loot  
- `Shift` – Sprint  

---

## 👨‍💻 Team Members

- **Vighnesh Mandavkar (B22CS061)** – Ship mechanics, procedural generation, game flow  
- **Krishna Balaji Patil (B22CS078)** – UI design, player control, interaction system  
- **Vinay Kumar (B22CS062)** – Enemy AI, combat system, loot and items  

---

## 📦 Future Improvements

- 🌦️ Dynamic weather and day-night cycle  
- 🧙‍♂️ Boss enemies and advanced AI behavior  
- 🔗 Multiplayer support for co-op exploration  
- ⚒️ Crafting and building system

---

## 📜 License

This project is for educational purposes. Feel free to fork and experiment!

