# Project Overview: Dungeon Match Combat

## 1. Project Description
Dungeon Match Combat is a hybrid puzzle-RPG prototype where player actions are determined by matching blocks on a 7x7 grid. The game is designed for players who enjoy "Match-3" mechanics combined with turn-based combat strategy. Core pillars include tactical grid management (only the bottom row is interactable), combo-based combat power, and character-specific abilities (Fighter, Mage, Tank).

## 2. Gameplay Flow / User Loop
1.  **Boot**: The game initializes the `GridManager` and `CombatManager` in the `Main` scene.
2.  **Interaction**: The player clicks on blocks in the **bottom row (y=0)**.
3.  **Grid Processing**:
    *   The clicked block is destroyed, triggering gravity and refilling the grid.
    *   Falling blocks create matches (3 or more identical types).
    *   Matches trigger `CombatAction` events which are sent to the `CombatManager`.
4.  **Combat Execution**: 
    *   Actions are queued (Player actions have priority).
    *   Characters (Fighter, Mage, Tank) perform animations and effects based on the matches.
    *   Enemies attack periodically based on individual timers.
5.  **Wave Progression**: When all enemies are defeated, a new wave spawns.
6.  **Game Over**: If player HP reaches 0, the session restarts (prototype reset).

## 3. Architecture
The project follows a **Manager-Pattern** architecture with an event-driven queue for combat.
*   **GridManager**: Central authority for the puzzle board. Handles input, matching logic, and "Ska" (empty) block generation.
*   **CombatManager**: Singleton manager that handles the game state (HP, XP, Wave), action queuing, and UI updates.
*   **Event Queue**: A `LinkedList<CombatAction>` ensures that animations and logic (Damage/Healing) are executed sequentially to maintain visual clarity.
*   **Visual-Logic Separation**: Logic is handled in Managers/Units, while `CharacterVisuals` handles juice (flash, shake) and `Animator` handles skeletal animation.

`Location: Assets/Scripts`

## 4. Game Systems & Domain Concepts
### Grid System
*   `GridManager`: Manages a 7x7 grid of `BlockType`. Uses `WouldMatch` to prevent matches during initialization.
*   **Bottom-Row Interaction**: Input is restricted to `y=0`. Clicking here triggers a "Ska" refill strategy.
*   **Combo Logic**: Multiple matches in one move increase the pitch of SFX and accumulate actions.

### Combat System
*   `CombatAction`: A data class representing a specific move (Attack, Heal, etc.).
*   `EnemyUnit`: Independent AI entities that attack on a timer (`AttackInterval`).
*   **Damage Types**: Standard physical damage is mitigated by `Shield`. "Magic" damage (e.g., from `ZombieMage`) bypasses shields.

### VFX & Juice
*   `CharacterVisuals`: Uses `MaterialPropertyBlock` to apply color overlays (flashes) without creating material instances.
*   `CameraShake`: Provides haptic visual feedback during matches and damage.

`Location: Assets/Scripts`

## 5. Scene Overview
*   **Main**: The primary gameplay scene. Contains the `GridManager` (as a transform parent for blocks), `CombatManager`, `AudioManager`, and the `UIDocument`.
*   **Environment**: Uses URP 2D Renderer with a static dungeon background and character prefabs positioned at specific spawn points.

`Location: Assets/`

## 6. UI System
The project uses **UI Toolkit (UITK)** for its interface.
*   **Main.uxml**: Defines the HUD (HP bar, EXP, Shield) and a `notification-layer`.
*   **Main.uss**: Contains styling for progress bars and the dynamic "Notification Label" used for damage numbers.
*   **Dynamic UI**: `CombatManager` uses `RuntimePanelUtils` to convert world-space positions (where matches happen) to screen-space for floating combat text.
*   **Extension**: New UI elements should be added to `Main.uxml` and bound in `CombatManager.SetupUI()`.

`Location: Assets/UI`

## 7. Asset & Data Model
*   **Blocks**: Defined by the `BlockType` enum. Visuals (Sprites/Colors) are assigned in the `GridManager` inspector.
*   **Enemies**: Prefabs with `EnemyUnit` and `Animator`. Stats (HP, Attack) are configured per-prefab.
*   **Audio**: Managed by `AudioManager` using a `SEType` enum and a dictionary of `AudioClip`. Uses an internal `AudioSource` pool for concurrency.
*   **Materials**: Uses custom shaders (`SpriteOverlay.shader`) for flash effects.

`Location: Assets/Sprites, Assets/Prefabs, Assets/Shaders`

## 8. Notes, Caveats & Gotchas
*   **Manual Refill**: When a player clicks a block, the refill has a high `manualSkaRate`. This is intended to prevent "infinite" free matches without deliberate player action.
*   **Animator States**: The `CombatManager` waits for specific state names (e.g., "Attack", "Magic"). Renaming states in `AnimatorControllers` will break the combat queue timing.
*   **Zombie Mage**: A special case in `EnemyUnit.Start()` checks for the string "ZombieMage" in the GameObject name to toggle `IsMagic` logic.
*   **Queue Priority**: Player actions are added via `AddFirst` to the event queue, effectively "interrupting" pending enemy actions to ensure the game feels responsive to player moves.