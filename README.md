# Alien Escape (Unity) 👾🔫

A fast-paced 2D top-down shooter built in Unity. Clear each level by eliminating all enemies as quickly as possible — your **best completion time per level** is saved locally as a personal best.

🎥 **Demo video:**

[![Alien Escape Demo](https://img.youtube.com/vi/5BVHF9wCfT0/0.jpg)](https://www.youtube.com/watch?v=5BVHF9wCfT0)


---

## 🎯 Objective

- Survive and **defeat every enemy in the level** (mobs + bosses)
- Your **run time** is tracked during gameplay
- When the level is completed (all enemies defeated), the timer stops and your **best time (PB)** is stored

---

## ✨ Key Features

### Player Controls & Combat
- **Top-down movement** using `Rigidbody2D`
- **Aiming / rotation**
  - Mouse aim (keyboard + mouse)
  - Right-stick aim (controller)
- **Shooting**
  - Fire rate / cooldown system
  - Projectile prefab spawned from a fire point
  - Sound effects played via temporary `AudioSource` objects (audio continues after shooter destruction)

### Enemy & Boss Systems
- **Detection-based AI**
  - Enemies use a detection radius to decide when to engage the player
  - Movement switches between chasing and wandering/patrolling based on visibility
- **Multiple enemy types**
  - Standard zombie-style enemies
  - Shooter-type enemies that rotate and fire at the player
  - Bosses (tagged as `Boss`) with unique behaviours (e.g. minion spawning)

### Health, Damage & Feedback
- Shared **Health System** approach (damage application + death events)
- Bullet hit detection via triggers/collisions (e.g. enemies detect `Bullet` tag hits)
- Visual feedback such as blood splats on death (prefab-based)

### Level Flow, HUD & UI Screens
- In-game HUD:
  - **Timer UI** (live time)
  - **Mob counter UI** (`killed / total`)
  - **Player health UI**
- End states:
  - **You Died** screen (HUD hidden, timer stopped)
  - **Level Complete** screen (HUD hidden, time displayed)

### Personal Best / Highscore Persistence
- Best times are saved **per level** into a JSON file:
  - Location: `Application.persistentDataPath/highscore.json`
- PB is shown in UI as:
  - `Personal Best (Level X): Ys` or `N/A` if no record exists
- Level number is derived from the scene name (e.g. `level1`, `level2`, `level3`)

---

## 🕹️ Controls

### Keyboard + Mouse
- **Move:** `W A S D`
- **Aim:** Mouse
- **Shoot:** Left Click (`Fire1`)

### Controller (Xbox-style mapping)
- **Move:** Left stick (`Horizontal`, `Vertical`)
- **Aim:** Right stick (`R_Horizontal`, `R_Vertical`)
- **Shoot:** Right Trigger (`RT` axis)

> Controller bindings depend on the Unity Input Manager mappings included in the project.

---

## 🗺️ Scenes

- `MainMenu`
- `level1`
- `level2`
- `level3`
- `Credits`

---

## 🛠️ Tech Stack

- **Engine:** Unity (2D)
- **Language:** C#
- **UI:** TextMeshPro
- **Target Platform:** PC

---

## 📄 License

See `LICENSE`
