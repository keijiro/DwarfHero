# Project Overview: Match-3 Combat Prototype

This project is a hybrid Match-3 and Turn-Based Combat prototype. Players interact with a 7x7 grid of blocks to trigger actions for a party of three heroes (Fighter, Mage, Tank) against waves of enemies. The game features a reactive combat queue, procedural grid refills, and a robust visual feedback system using UITK and custom shaders.

## 1. Project Description
The project is a technical vertical slice of a puzzle-RPG. It targets players who enjoy strategic tile-matching paired with RPG progression.
- **Core Pillars:**
    - **Reactive Puzzle Combat:** Matches directly translate to hero actions (Attack, Magic, Heal, Shield).
    - **Resource Management:** Managing Health, Shields, and EXP while dealing with "Ska" (junk) blocks.
    - **Visual Feedback:** High-impact animations, screen shakes, and shader-based overlays for combat feedback.

## 2. Gameplay Flow / User Loop
1. **Boot:** The game initializes the `Main.unity` scene, setting up the `CombatManager` and `GridManager`.
2. **Setup:** A 7x7 grid is procedurally generated without initial matches. A wave of enemies is spawned.
3. **Player Phase (Active):** The player clicks blocks in the bottom row to destroy them. This triggers gravity and refills, creating opportunities for automatic matches.
4. **Matching:** When 3+ identical blocks align, a `CombatAction` is sent to the `CombatManager`.
5. **Combat Execution:** The `CombatManager` processes actions via a queue, triggering hero animations, enemy damage, and UI updates.
6. **Enemy Phase (Passive):** Enemies have individual timers. When a timer expires, they add an attack action to the queue.
7. **Progression:** Defeating enemies grants EXP. Clearing the board triggers new waves.
8. **Shutdown:** The game persists until the party's HP reaches zero, at which point stats are reset for the prototype loop.

## 3. Architecture
The project follows a Manager-centric pattern with a decoupled event-like queue system for combat.
- **Entry Point:** The `Main.unity` scene containing the `CombatManager` and `GridManager` MonoBehaviours.
- **Combat Queue:** A `LinkedList<CombatAction>` in `CombatManager` acts as the central coordinator, ensuring animations and logic resolve sequentially.
- **Input:** Uses the **New Input System** to detect clicks on the grid.
- **Data Flow:** `GridManager` (Matches) -> `CombatManager` (Action Queue) -> `EnemyUnit`/`CharacterVisuals` (Execution).

## 4. Game Systems & Domain Concepts

### Grid & Match System
Handles the 7x7 game board logic including gravity, clusters, and Ska block generation.
- `GridManager`: The core controller for grid state, matching logic, and block instantiation.
- `BlockType`: Enum defining Sword (Attack), Magic, Heal, Shield, Gem (EXP), Key, and Ska (Junk).
- **Extension:** Add new block types by extending the `BlockType` enum and updating `GetWeightedRandomType` and `DecideNewBlockType`.
- **Pattern:** Cluster-based recursive matching.
- Location: `Assets/Scripts/GridManager.cs`

### Combat & Action System
Sequences all gameplay events to prevent visual overlap and ensure logic consistency.
- `CombatManager`: Singleton managing party stats (HP, Shield, EXP) and the action queue.
- `CombatAction`: A data class carrying action type, value, and source references.
- `EnemyUnit`: Individual enemy logic with independent attack timers and stat tracking.
- **Extension:** Implement new hero types by adding `Animator` references to `CombatManager` and creating new `CombatActionType` handlers.
- **Pattern:** Producer-Consumer Queue (Coroutine-based).
- Location: `Assets/Scripts/CombatManager.cs`, `Assets/Scripts/EnemyUnit.cs`

### Visual Feedback System
Manages the aesthetic "juice" of the game, including flashes, shakes, and UI notifications.
- `CharacterVisuals`: Handles sprite-based effects like Red/White flashes and horizontal shaking using MaterialPropertyBlocks.
- `CameraShake`: Provides global screen shake effects triggered by grid interactions or damage.
- **Extension:** Modify the `SpriteOverlay.shader` or create new `CharacterVisuals` routines for status effects (e.g., Poison, Burn).
- Location: `Assets/Scripts/CharacterVisuals.cs`, `Assets/Scripts/CameraShake.cs`

## 5. Scene Overview
- **Main.unity**: The primary gameplay scene. It contains the environment (Dungeon BG), the UI Document (HUD), and the spawn points for enemies and the grid.
- **Scene Flow**: Currently a single-scene loop. Waves are re-spawned within the same scene context upon victory or defeat.

## 6. UI System
The project uses **UI Toolkit (UITK)** for its interface.
- **Structure:** Defined in `Main.uxml` with styles in `Main.uss`.
- **Binding:** `CombatManager` queries visual elements (HP bar, labels) via `rootVisualElement.Q<T>()` and updates them during the `UpdateUI` call.
- **Notifications:** World-space to UI-space converted labels (`notification-layer`) that animate upwards and fade out when actions occur.
- **Modification:** Edit `Main.uxml` for layout changes and `Main.uss` for visual styling (e.g., the `hp-fill` class).

## 7. Asset & Data Model
- **Prefabs:** 
    - `Characters/Monster`: Contains enemy variants like ZombieMage.
    - `FX`: Particle systems for `ManualDestroyFX` and `MatchDestroyFX`.
- **ScriptableObjects:** None currently; stats are primarily defined as public fields on `CombatManager` and `EnemyUnit`.
- **Materials:** 
    - `EnemyOverlay`: A specialized material using `SpriteOverlay.shader` for flash effects.
- **Naming Convention:** 
    - Icons: `Icon_[Type].png`
    - Animators: `[Role]Controller.controller`

## 8. Notes, Caveats & Gotchas
- **Manual Destruction:** Only blocks in the bottom row (`y=0`) can be clicked.
- **Ska Blocks:** These are "junk" blocks that don't match with anything. They are generated with a higher probability during manual destruction (`manualSkaRate`).
- **Queue Interrupts:** Player actions (from matches) are inserted at the **head** of the combat queue (`AddFirst`) to ensure immediate responsiveness over enemy actions.
- **Zombie Mage:** This specific enemy type sets `IsMagic = true`, which allows its attacks to bypass the party's `Shield` and deal direct HP damage.
- **Animator Sync:** The `CombatManager` uses a `WaitForAnimation` helper. If you rename animation states in the `AnimatorController`, you must update the string references in `CombatManager.cs`.