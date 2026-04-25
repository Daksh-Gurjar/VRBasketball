# 🏀 VRBasketball

A third-person basketball game built with **Unity 2022.3 LTS**, featuring player-controlled shooting, a dribbling mechanic, an AI opponent, and a score tracker.

---

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Project Structure](#project-structure)
- [Scripts](#scripts)
- [Getting Started](#getting-started)
- [Controls](#controls)
- [Requirements](#requirements)
- [License](#license)

---

## Overview

VRBasketball is a Unity-based basketball game where the player moves around a court, dribbles a ball, and shoots it into a hoop to score points. An AI opponent chases the player; if it gets too close, the player is sent back to their starting position.

---

## Features

- 🕹️ **Player movement** – WASD / arrow-key movement with automatic look direction
- 🏀 **Ball dribbling** – animated bounce while the ball is held
- 🎯 **Shooting** – arc-based throw toward the hoop target
- 🤖 **AI Opponent** – chases the player within a configurable reaction radius and resets the player on contact
- 🧮 **Score system** – points are awarded when the ball passes through the hoop trigger
- ♻️ **Player reset** – physics-safe teleport back to spawn point when tagged by the opponent

---

## Project Structure

```
Assets/
├── Code/
│   ├── BasketballController.cs   # Player movement, dribble & shoot logic
│   ├── HoopTriger.cs             # Hoop trigger – detects ball and awards points
│   ├── OpponentAI.cs             # AI chases player and triggers reset on contact
│   ├── PlayerController.cs       # Player spawn/reset helper
│   └── ScoreManager.cs           # Tracks and displays the current score
├── Materials/                    # Ball, floor, character, and court materials
├── Prefabs/                      # Ball, Hoop, Floor, and Character prefabs
├── Scenes/
│   └── SampleScene.unity         # Main game scene
└── BouncyBall.physicMaterial     # Physics material for the basketball
```

---

## Scripts

### `BasketballController.cs`
Attached to the player character. Handles:
- WASD / axis-based movement
- Dribbling animation (ball bounces at hip height while held)
- Hold **Space** to raise the ball overhead and aim at the hoop; release to shoot along an arc

### `PlayerController.cs`
Lightweight reset component attached to the player. Exposes a `ResetToStart()` method called by `OpponentAI` when the opponent catches the player.

### `ScoreManager.cs`
Manages the UI score counter. Provides `AddScore(int points)` and `ResetScore()` methods. Requires a `UnityEngine.UI.Text` reference assigned in the Inspector.

### `HoopTriger.cs`
Trigger collider placed inside the basketball hoop. Calls `ScoreManager.AddScore(1)` whenever a collider tagged `"Ball"` enters the zone.

### `OpponentAI.cs`
Simple chasing AI. Key Inspector fields:

| Field | Default | Description |
|---|---|---|
| `moveSpeed` | 3 | Units per second |
| `reactionDistance` | 10 | Start chasing within this radius |
| `stopDistance` | 1.5 | Stop closing in when this close |
| `resetRadius` | 1.0 | Trigger player reset within this radius |
| `resetDelay` | 0.25 s | Delay before the reset fires |

---

## Getting Started

1. **Clone the repository**
   ```bash
   git clone https://github.com/Daksh-Gurjar/VRBasketball.git
   ```

2. **Open in Unity**  
   Open Unity Hub, click **Add → Add project from disk**, and select the cloned folder.  
   Unity version: **2022.3.62f1** (or any 2022.3 LTS patch).

3. **Open the scene**  
   In the Project window navigate to `Assets/Scenes/` and double-click **SampleScene**.

4. **Press Play** – the game runs in the Editor viewport.

---

## Controls

| Key | Action |
|---|---|
| `W / A / S / D` (or Arrow Keys) | Move player |
| `Space` (hold) | Raise ball overhead and aim at hoop |
| `Space` (release) | Shoot the ball |

---

## Requirements

- [Unity 2022.3 LTS](https://unity.com/releases/editor/whats-new/2022.3.0) (tested on 2022.3.62f1)
- No additional SDKs required – built-in Unity Physics and UI modules only

---

## License

This project is licensed under the **MIT License** – see [LICENSE](LICENSE) for details.  
© 2026 Daksh Gurjar
