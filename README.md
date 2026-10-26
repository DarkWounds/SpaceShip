# SpaceShip (Space Survival)

A 2D survival arcade game made in **Unity** with **C#**, my high-school Computer Science certification project (atestat).

Fly a small rocket with the mouse and survive as long as possible. Asteroids bounce around the arena and get faster every time they hit something. Touch an asteroid or the edge of the arena and the ship explodes.

<!-- Add a gameplay GIF here, e.g. ![gameplay](docs/gameplay.gif) -->

## How to play

- **Hold left click**: the ship turns towards the cursor and thrusts in that direction (movement keeps its inertia).
- **Score**: grows with survival time (`time × 10`).
- **Esc**: pause menu with **Resume**, **Restart** and **Exit**.
- **Game over**: hitting an asteroid or the barrier destroys the ship; press **Restart** to play again.

## How it works

- **Ship** (`PlayerController.cs`): `Rigidbody2D` physics. While the button is held it points at the mouse (`ScreenToWorldPoint`) and adds a thrust force, with the speed capped at `maxSpeed`. On collision it spawns a particle explosion, hides the sprite, disables the colliders and stops the physics simulation, then shows the restart button.
- **Asteroids** (`Obstacle.cs`): each one gets a random size, direction and spin at start. Smaller asteroids get proportionally more speed (speed / size).
- **Rising difficulty**: asteroids use a 2D physics material with bounciness above 1, so every collision adds energy and the game gets harder on its own.
- **Pause**: Esc sets `Time.timeScale` to 0, pauses the music and shows the pause menu; resuming or restarting sets it back to 1.
- **Music** (`MusicManager.cs`): a singleton kept alive across scene reloads (`DontDestroyOnLoad`), so the background music loops without restarting when you replay.
- **UI**: built with **UI Toolkit** (`UI Layout.uxml`): a score label updated every frame and Resume / Restart / Exit buttons that stay hidden (`DisplayStyle.None`) until they are needed.
- **Input**: the new Unity Input System.

## Project structure

```
Assets/
  Scenes/Game.unity        main game scene
  Scripts/PlayerController.cs
  Scripts/Obstacle.cs
  Scripts/MusicManager.cs
  Prefabs/                 Obstacle, ExplosionEffect
  Materials/               PhysMaterial_Bounce (bouncy 2D physics material)
  UI/UI Layout.uxml        score label + pause / restart buttons
  BackGroundMusic/         background music
```

## Run it

1. Install **Unity 6** (project version `6000.4.0f1`) from Unity Hub.
2. Unity Hub → **Add** → select this folder.
3. Open `Assets/Scenes/Game.unity` and press **Play**.

## Tech

Unity 6 · C# · Universal Render Pipeline (2D) · Rigidbody2D physics · Particle System · UI Toolkit · Input System · AudioSource
