# Project Overview: Match-3 RPG Prototype

This document provides a technical overview of the Unity Match-3 RPG project, detailing its architecture, core systems, and asset organization.

## 1. Project Description
This project is a Match-3 RPG prototype developed in Unity 6, utilizing the Universal Render Pipeline (URP) for 2D visuals. It targets a "tactical puzzle" experience where players interact with a 7x7 grid to trigger combat and resource effects. The core gameplay loop focuses on clicking the bottom row of the grid to clear blocks, which causes gravity-based refills and potential chain reactions (matches) to accumulate points or trigger actions.

**Core Pillars:**
- **Tactical Match-3:** Match-3 mechanics combined with turn-based RPG resource management.
- **Dynamic Feedback:** Heavy emphasis on visual polish through camera shakes, particles, and sprite animations.
- **Chain Reactions:** "Ska" (empty) blocks serve as catalysts for larger detonations when adjacent matches occur.

## 2. Gameplay Flow / User Loop
1.  **Boot/Initialization:** The `Main.unity` scene loads, and `GridManager` initializes a 7x7 grid without pre-existing matches.
2.  **Input Phase:** The player clicks a block specifically in the **bottom row** (y=0).
3.  **Destruction Phase:** The clicked block is destroyed, triggering a "Ska" block refill to descend.
4.  **Gravity & Refill:** Blocks fall to fill gaps. New blocks are spawned at the top.
5.  **Match Detection:** The system checks for clusters of 3 or more identical blocks (Sword, Shield, Magic, etc.).
6.  **Detonation:** Matches trigger visual effects. If a match is adjacent to a "Ska" (Gray) block, the Ska block also detonates, increasing the match count.
7.  **Resolution:** The loop repeats if new matches are formed from the refill; otherwise, control returns to the player.

## 3. Architecture
The project follows a centralized manager pattern with a focus on Coroutine-based sequencing for animations and state transitions.

### Grid & Match Management
- `GridManager`: The central brain. It handles the 2D array representation of the board, physical spawning of block GameObjects, input raycasting, and the logic for gravity and match-3 detection.
- **State Management:** Uses a simple `isProcessing` boolean to lock input during animations.
- **Data Flow:** Logic uses a `BlockType` enum; visuals are updated via `SpriteRenderer` references stored in a parallel 2D array.
- **Location:** `Assets/Scripts/`

### Visual Feedback System
- `CameraShake`: A utility component attached to the Main Camera. It provides micro-shaking effects for impacts like block destruction and landing.
- **Pattern:** Command-based. `GridManager` calls `Shake()` on the camera instance when specific events occur.
- **Location:** `Assets/Scripts/`

## 4. Game Systems & Domain Concepts

### Match-3 Logic
- `GridManager.CheckMatches`: Uses a two-pass approach. First, it identifies horizontal and vertical lines of 3+. Second, it uses a recursive `FindCluster` (flood-fill) to group connected matches and detect adjacent "Ska" blocks for detonation.
- **Extension:** New block types can be added to the `BlockType` enum and the `typeWeights` array to adjust spawn rates.

### Block Types (Domain)
- **Sword (Red), Shield (Blue), Magic (Purple), Heal (Green), Gem (Cyan), Key (Yellow):** Standard matching blocks.
- **Ska (Gray):** A special "Empty" or "Catalyst" block. It cannot be matched with itself but detonates when a match occurs in an adjacent cell.
- **Location:** `GridManager.cs` (Enum `BlockType`)

## 5. Scene Overview
- **Main.unity:** The primary gameplay scene. It contains the `GridManager` entity, the main camera with `CameraShake`, and the `UIDocument` for the interface.
- **Scene Flow:** Currently a single-scene prototype. The grid is procedural, generated on `Start()`.

## 6. UI System
- **Framework:** Unity UI Toolkit (UITK).
- **Structure:**
    - `Main.uxml`: Defines the visual hierarchy (currently a container for future RPG elements).
    - `Main.uss`: Contains styling for the UI elements.
    - `DefaultPanel.asset`: UI Toolkit panel settings for resolution scaling.
- **Binding:** The UI is currently a layout shell (`root` element) meant to be populated with RPG stats (Health, Mana, etc.) as the prototype evolves.
- **Location:** `Assets/UI/`

## 7. Asset & Data Model
- **Prefabs:**
    - `ShockwaveFX`, `MatchDestroyFX`, `ManualDestroyFX`: Particle-based visual feedback triggered by the `GridManager`.
- **Sprites:**
    - Organized by `Blocks` (base frames and icons) and `FX` (textures for particles).
    - Icons use a layered approach: A `Block_Base` sprite for the color background and an `Icon_X` sprite for the symbol.
- **Rendering:** Uses URP 2D Renderer. Materials for FX use specialized shaders (Shockwave, Sparkle).
- **Location:** `Assets/Prefabs/FX/`, `Assets/Sprites/Blocks/`

## 8. Notes, Caveats & Gotchas
- **Input Constraint:** Interaction is hardcoded to only allow clicks on the **bottom row** (`y = 0`). This is a design choice for the specific prototype mechanic and not a bug.
- **Ska Generation:** When a player manually destroys a block, the refill logic is weighted heavily toward spawning "Ska" blocks (`manualSkaRate`). Match-triggered refills do not spawn Ska blocks by default.
- **No Object Pooling:** Currently, blocks are `Destroy()`ed and `Instantiate()`d dynamically. For mobile optimization or larger grids, an object pooler should be implemented within `GridManager`.
- **Scaling:** Block spacing and grid size are constants (`7x7`, `1.1f` spacing). Changing these requires updating both the logic and the camera view.