# Project Overview: Match-Combat Prototype

This project is a hybrid puzzle-combat prototype that combines a match-3 style grid with turn-based RPG combat. Players interact with a 7x7 grid to trigger actions for a party of three characters (Fighter, Mage, Tank) against waves of enemies.

## 1. Project Description
The project is a technical prototype for a "Match-3 RPG" where grid interactions directly translate into combat actions. It is designed for mobile or desktop play, featuring a polished visual feedback system including screen shakes, sprite overlays, and particle effects. The core experience centers on strategic block destruction to clear "Ska" (empty) blocks and trigger powerful combos that automate party behavior.

## 2. Gameplay Flow / User Loop
1.  **Boot**: The game initializes the `GridManager` and `CombatManager` in the `Main` scene.
2.  **Wave Start**: `CombatManager` spawns a wave of enemies with random types (e.g., Skeleton, ZombieMage).
3.  **Player Input**: The player clicks blocks on the **bottom row** of the 7x7 grid.
4.  **Grid Resolution**:
    *   The clicked block is destroyed, causing blocks above to fall (`HandleGravityAndRefill`).
    *   Matches of 3 or more identical blocks are detected.
    *   Matched clusters, including adjacent "Ska" blocks, are sent to the `CombatManager`.
5.  **Combat Execution**: `CombatManager` queues actions (Attack, Heal, Shield, etc.). Actions are executed sequentially with synchronized animations.
6.  **Enemy Turn**: Enemies have internal timers; when a timer hits zero, an enemy attack is queued.
7.  **Loop**: The player continues clearing the grid until all enemies are defeated (triggering a new wave) or the party's HP reaches zero (resetting the prototype).

## 3. Architecture
The project follows a **Manager-driven pattern** with a focus on **event-based communication** and an **action queue** for combat.

*   **`GridManager`**: The central controller for the puzzle logic. It manages the 2D array of blocks, handles physics-less "gravity," and detects matches.
*   **`CombatManager` (Singleton)**: Acts as the orchestrator for the battle. It maintains a `LinkedList<CombatAction>` as a command queue to ensure animations and logic (like damage calculation) stay synchronized.
*   **`AudioManager` (Singleton)**: Provides a global sound effect pooling system.
*   **Visual Feedback**: Most entities use `CharacterVisuals` for high-performance sprite manipulation (using `MaterialPropertyBlocks`) and `CameraShake` for impact.

`Location: Assets/Scripts`

## 4. Game Systems & Domain Concepts

### Match-3 Grid System
The grid is a 7x7 matrix of `BlockType` enums.
*   **`GridManager`**: Handles `ProcessMove`, which is a coroutine-based sequence: Manual Destroy -> Gravity -> Refill -> Match Detection -> Repeat.
*   **Ska Blocks**: Non-matching filler blocks that can only be cleared by making a match adjacent to them.
*   **Weighted Randomization**: New blocks are spawned based on `typeWeights`, allowing for tuned difficulty.
`Location: Assets/Scripts/GridManager.cs`

### Combat Queue System
To prevent visual overlapping, all actions are queued.
*   **`CombatAction`**: A data class containing action type, value, and source.
*   **`QueueProcessor`**: A coroutine that continuously checks the `eventQueue` and executes `ExecuteAction`.
*   **Priority**: Player actions are inserted at the front (`AddFirst`) to ensure immediate responsiveness to grid matches.
`Location: Assets/Scripts/CombatManager.cs`

### Character & Enemy System
*   **`EnemyUnit`**: Manages individual enemy AI (timer-based) and stats.
*   **`CharacterVisuals`**: A reusable component for "Flash" and "Shake" effects. It uses a custom `SpriteOverlay.shader` to apply colors without creating new material instances.
`Location: Assets/Scripts/EnemyUnit.cs`, `Assets/Scripts/CharacterVisuals.cs`

## 5. Scene Overview
*   **`Main.unity`**: The primary gameplay scene. It contains the `GridManager` (parenting the blocks), the `CombatManager`, and the UI Document.
*   **Scene Flow**: The prototype currently operates in a single-scene loop. When a party wipes, the `CombatManager` resets stats and re-spawns the wave within the same scene.

## 6. UI System
The project uses **Unity Digital Toolkit (UITK)**.
*   **`Main.uxml`**: Defines the HUD (HP bars, Shield, EXP) and the `notification-layer`.
*   **`CombatManager.SetupUI`**: Binds visual elements to code using `root.Q<Label>("name")`.
*   **Dynamic Notifications**: `ShowActionNotification` spawns labels at world-to-panel coordinates, animating them upwards to provide immediate feedback on match values.
*   **Styling**: Controlled via `Main.uss`, utilizing the "Pirata One" font for a Gothic/Dungeon aesthetic.
`Location: Assets/UI`

## 7. Asset & Data Model
*   **Prefabs**: Enemies are stored as prefabs in `Assets/Prefabs/Characters/Monster` and assigned to the `CombatManager` via the Inspector.
*   **Visuals**: Uses URP with a 2D Renderer. Shaders (`PulseOverlay`, `SpriteOverlay`) are used for gameplay-critical feedback (e.g., enemy intending to attack).
*   **Block Definitions**: Defined in the `GridManager` via arrays for `iconSprites` and `blockColors`, indexed by the `BlockType` enum.

## 8. Notes, Caveats & Gotchas
*   **Bottom-Row Only**: Player interaction is restricted to `y=0`. Clicking any other row has no effect by design.
*   **Ska Logic**: `GridManager` detects Ska blocks by searching the 4-neighbors of a successful match cluster. Clearing 3 Ska blocks counts as 1 "Effective Match" for power calculations.
*   **Animation Timing**: The `CombatManager` uses `WaitForAnimation` which relies on `animator.GetCurrentAnimatorStateInfo(0)`. If an animation state name in the Animator Controller is changed, this script will hang until the safety timeout (2.0s) triggers.
*   **MaterialPropertyBlocks**: `CharacterVisuals` uses these for performance. If you change a character's base material at runtime, ensure the `overlayMaterial` (found in `Resources/EnemyOverlay`) is still compatible.