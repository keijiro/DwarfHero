This technical documentation provides a comprehensive overview of the Unity project, covering its architecture, core systems, and implementation details.

## 1. Project Description
This project is a Match-3 Combat Hybrid game where players manage a 7x7 grid of elemental blocks to perform actions in real-time combat against approaching monsters. The core experience centers on a "Bottom-Row Interaction" mechanic, where players manually destroy blocks on the bottom row to trigger gravity, refills, and automated matches. 
- **Target Audience:** Fans of puzzle-RPG hybrids and casual strategy games.
- **Core Pillars:** 
    - **Reactive Match-3:** Matching is triggered by gravity and refills rather than direct swapping.
    - **Real-time Combat:** Enemies attack on independent timers, requiring the player to balance offense and defense.
    - **Resource Management:** Matches provide HP (Heal), Shields, Magic, Attack, and EXP.

## 2. Gameplay Flow / User Loop
1.  **Boot & Title:** The game starts in the `Title` scene. The `PersistentSystems` scene is loaded additively automatically via `PersistentSystemsLoader`.
2.  **Initialization:** Upon entering the `Main` scene, the `GridManager` initializes the 7x7 board, and the `CombatManager` begins spawning enemy waves.
3.  **Active Loop:**
    - Player clicks a block in the bottom row of the grid.
    - The block is destroyed, causing blocks above to fall (`HandleGravityAndRefill`).
    - Falling blocks trigger automated matches.
    - Matches generate `CombatAction` events (Attack, Heal, Shield, etc.).
4.  **Combat Resolution:** The `CombatManager` processes an event queue. Player actions are prioritized and executed with animations, followed by enemy attacks.
5.  **Game Over/Success:** If player HP reaches zero, the `GameOver` scene is loaded. Progress (EXP) is tracked.

## 3. Architecture
The project follows a **Manager-Pattern** architecture with a centralized event-driven combat system.
- **Entry Point:** The `PersistentSystemsLoader` uses `[RuntimeInitializeOnLoadMethod]` to ensure core managers (like `AudioManager`) are always present.
- **Singleton Pattern:** `CombatManager` and `AudioManager` use the Singleton pattern for easy global access.
- **Event Queue:** The combat system uses a `LinkedList<CombatAction>` to sequence animations and logic, ensuring visual clarity during chaotic match-3 cascades.
- **Separation of Concerns:** 
    - `GridManager`: Handles puzzle logic (gravity, matching, input).
    - `CombatManager`: Handles game state, stats, and action execution.
    - `CharacterVisuals`: Handles low-level sprite effects (flashing, shaking).

`Location: Assets/Scripts`

## 4. Game Systems & Domain Concepts

### Puzzle & Grid System
Manages the 7x7 grid. It uses a weighted random distribution for block generation and a recursive clustering algorithm for match detection.
- `GridManager`: Main controller for grid state and "Ska" (junk) block generation.
- `RowHighlighter`: Visual feedback for valid interaction zones (bottom row).
- **Extension:** New block types can be added to the `BlockType` enum and `typeWeights` array in the inspector.
`Location: Assets/Scripts/GridManager.cs`

### Combat & Action System
A queue-based system that translates grid matches into gameplay effects.
- `CombatManager`: Processes `CombatAction` objects.
- `EnemyUnit`: Independent AI that queues "EnemyAttack" actions based on a timer.
- `CombatAction`: Data container for action type, value, and source.
- **Extension:** Add new action types to `CombatActionType` and implement their logic in `CombatManager.ExecuteAction`.
`Location: Assets/Scripts/CombatManager.cs`

### Audio System
A pooled audio system supporting spatial/reverb routing via the `AudioMixer`.
- `AudioManager`: Manages a pool of `AudioSource` components and handles `SEType` mapping.
- **Pattern:** Object Pooling for audio sources to prevent allocation spikes.
`Location: Assets/Scripts/AudioManager.cs`

## 5. Scene Overview
- **Title:** The entry scene. Handles initial loading and transitions.
- **PersistentSystems:** A utility scene containing the `AudioManager` and other "DontDestroyOnLoad" objects. Never unloaded.
- **Main:** The primary gameplay scene. Contains the `GridManager`, `CombatManager`, and URP environment.
- **GameOver:** Displayed upon player defeat. Shows final stats and provides a return path to Title.

## 6. UI System
The project uses **UI Toolkit (UITK)** for its interface, leveraging UXML for structure and USS for styling.
- **Framework:** `UIDocument` components are used in `Main` and `GameOver` scenes.
- **Binding:** `CombatManager` manually queries the root visual element (e.g., `root.Q<Label>("hp-text")`) to update the HUD.
- **Animations:** UI animations (notifications, combat numbers) are handled via Coroutines that manipulate USS properties or `transform` values.
- **Screen Flow:** Managed by `SceneTransitionController` and scene-specific controllers (`TitleScreenController`).

`Location: Assets/UI`

## 7. Asset & Data Model
- **Prefabs:** Characters (Players/Monsters) and FX (Shockwaves, Matches) are stored in `Assets/Prefabs`.
- **Materials/Shaders:** Custom shaders like `SpriteOverlay.shader` are used for damage flashes and "Pulse" effects.
- **Animations:** Uses `AnimatorController` for character states (Idle, Attack).
- **Naming Convention:** 
    - Sprites: `Icon_[Name].png`, `Block_[Name].png`
    - Audio: `SE_[Name].wav`
- **Data Storage:** Game state (HP, EXP) is currently volatile (resides in `CombatManager` instance). Persistent save data is not yet implemented.

## 8. Notes, Caveats & Gotchas
- **Input Constraint:** Interaction is hardcoded to the bottom row (`y = 0`) in `GridManager.HandleClick`. Modifying this requires updating both `HandleClick` and `RowHighlighter`.
- **Ska Blocks:** These are "junk" blocks that don't match with each other but are cleared when adjacent to a valid match.
- **Queue Priority:** `CombatManager` inserts player actions at the *head* of the queue (`AddFirst`) to ensure immediate responsiveness, while enemy actions are added to the *tail* (`AddLast`).
- **Timing:** Match-3 cascades can generate many events quickly; the `isProcessingQueue` flag in `CombatManager` ensures they play out sequentially without overlapping animations overlapping.