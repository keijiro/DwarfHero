# Project Overview: Dungeon Match-3 Combat Prototype

## 1. Project Description
This project is a 2D hybrid "Match-3 RPG" prototype where players interact with a 7x7 grid to trigger combat actions for a party of three heroes (Fighter, Mage, Tank) against waves of monsters. It is designed as a fast-paced, turn-based experience where matching different block types (Sword, Shield, Magic, etc.) directly executes attacks, healing, or defense in real-time. The game emphasizes visual juice and feedback, featuring camera shakes, particle effects, and dynamic UI notifications.

## 2. Gameplay Flow / User Loop
1.  **Title Screen**: The entry point where the player starts the game via a UI Toolkit-based menu.
2.  **Combat Initialization**: The `Main` scene loads, spawning a wave of 2-6 random enemies and a 7x7 grid of randomized blocks.
3.  **Player Interaction**: The player clicks on blocks in the **bottom row** (y=0) to destroy them.
4.  **Grid Resolution**: 
    *   Destroyed blocks trigger gravity; new blocks fall from above.
    *   Clusters of 3+ identical blocks are automatically matched.
    *   Matches trigger "Ska" blocks (gray) adjacent to them, boosting the effect.
5.  **Action Queueing**: Each match is converted into a `CombatAction` (Attack, Magic, Heal, etc.) and queued in the `CombatManager`.
6.  **Combat Execution**: The `CombatManager` processes the queue sequentially, playing hero animations and applying damage/effects to enemies.
7.  **Enemy Turn**: Enemies have individual timers; when a timer expires, an enemy attack action is queued.
8.  **Win/Loss**: When all enemies die, a new wave spawns. If the party's HP reaches 0, the game resets the prototype stats (soft game over).

## 3. Architecture
The project follows a **Manager-driven** architecture with a centralized event queue for combat synchronization.

*   **Entry Point**: `Title.unity` -> `Main.unity`.
*   **Grid System**: `GridManager` handles all Match-3 logic, including block generation, gravity, and match detection. It uses a 2D array of `BlockType` enums.
*   **Combat Orchestration**: `CombatManager` acts as the brain. It maintains a `LinkedList<CombatAction>` to ensure actions (player or enemy) play out one by one without overlapping animations.
*   **Feedback System**: Visual feedback is decoupled via `CharacterVisuals` (handling flashes/shakes) and `CameraShake`.
*   **Data Flow**: `GridManager` -> `CombatManager` (via `AddPlayerAction`) -> `EnemyUnit` / `Party Stats` -> `UIDocument` (HUD Update).

`Location: Assets/Scripts`

## 4. Game Systems & Domain Concepts

### Match-3 Grid System
A 7x7 grid where only the bottom row is interactable.
*   `GridManager`: Manages the lifecycle of blocks. Implements a weighted random generation system.
*   `BlockType`: Enum defining Sword (Attack), Shield (Defense), Magic (AOE), Heal, Gem (EXP), Key (Bonus), and Ska (Booster).
*   **Ska Mechanic**: Matching a cluster also consumes adjacent "Ska" blocks, increasing the "effective count" of the action.
`Location: Assets/Scripts/GridManager.cs`

### Combat Queue System
Ensures that animations and damage numbers are synchronized.
*   `CombatAction`: A class representing a command (Type, Value, Source).
*   `CombatManager`: Processes the `eventQueue` using a Coroutine-based processor to wait for animations to finish before starting the next action.
`Location: Assets/Scripts/CombatManager.cs`

### Enemy AI
Simple timer-based behavior.
*   `EnemyUnit`: Each enemy tracks its own HP and attack cooldown.
*   **Variation**: Supports standard attacks and Magic (ignores Shield, used by `ZombieMage`).
`Location: Assets/Scripts/EnemyUnit.cs`

### Visual Feedback System
*   `CharacterVisuals`: Handles sprite flashing (e.g., Red for damage, White for attack) and local shakes.
*   `CameraShake`: A simple utility for screen-wide impact feedback.
`Location: Assets/Scripts/CharacterVisuals.cs`

## 5. Scene Overview
*   **Title**: Contains the `TitleScreenController` and the main UI menu. Used for initial boot.
*   **Main**: The core gameplay scene. Contains the `GridManager`, `CombatManager`, and URP 2D Render Setup. It manages the party (Fighter, Mage, Tank) and enemy spawn points.

## 6. UI System
The project uses **UI Toolkit (UITK)** for both the Title and HUD.
*   **Structure**: Managed via `.uxml` and `.uss` files. The HUD includes an HP bar, shield/exp labels, and a `notification-layer`.
*   **Dynamic UI**: 
    *   `CombatManager` dynamically creates `Label` elements in the `notification-layer` for damage numbers and action text.
    *   **Animations**: Done via Coroutines that manually update `label.style.translate` and `label.style.opacity` for high-performance visual effects.
*   **Binding**: References are grabbed in `SetupUI()` using `root.Q<T>("element-name")`.
`Location: Assets/UI`

## 7. Asset & Data Model
*   **Prefabs**: Character units (Monsters) and FX (Shockwaves, Particles) are prefab-based for easy spawning.
*   **Materials/Shaders**: Custom `SpriteOverlay.shader` allows for the "Flash" effect used in combat feedback by manipulating color overlays.
*   **Audio**: Centralized `AudioManager` with a `SEType` enum mapping to `AudioClip` assets. It supports pitch randomization and a simple pooling system to prevent audio cutting.
*   **Sprites**: Organized by category (Blocks, Characters, UI). Blocks have a base sprite and an icon overlay.

## 8. Notes, Caveats & Gotchas
*   **Interaction Limitation**: Remember that blocks can only be clicked on the **Bottom Row** (y=0). Clicking higher rows will not trigger any destruction.
*   **Queue Priority**: Player actions are inserted at the **head** (`AddFirst`) of the combat queue to ensure immediate responsiveness, while enemy actions are added to the **tail** (`AddLast`).
*   **Ska Blocks**: These are "dead" blocks that don't match with each other but are highly valuable as boosters when adjacent to a match.
*   **Zombie Mage**: A hardcoded check in `EnemyUnit.Start` looks for "ZombieMage" in the GameObject name to toggle the `IsMagic` flag, which makes attacks bypass player shields.
*   **Soft Game Over**: There is no dedicated Game Over screen; the game resets HP and clears the wave to allow continuous prototype testing.