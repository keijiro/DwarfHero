# Project Overview: 3-Match RPG Prototype

## 1. Project Description
This project is a technical prototype for a **3-Match RPG** that blends traditional match-3 puzzle mechanics with RPG-style resource gathering. The core experience centers around a tactical puzzle board where players interact exclusively with the bottom row of a 7x7 grid. By strategically destroying blocks to trigger cascades, players generate resources (represented by block types like Swords, Shields, and Magic) while navigating a unique "Ska" (dead block) mechanic that rewards chain reactions and cluster detonations.

## 2. Gameplay Flow / User Loop
1.  **Initialization**: The `GridManager` generates a 7x7 board using weighted random distribution, ensuring no matches exist at the start.
2.  **Manual Interaction**: The player clicks a block on the **bottom row (y=0)**.
3.  **Destruction & Refill**: The clicked block is destroyed, triggering a "Ska" block refill at the top. Gravity pulls existing blocks down to fill the gap.
4.  **Match Detection**: The system checks for horizontal or vertical matches of 3 or more.
5.  **Cluster & Detonation**: Matches trigger a recursive search for all adjacent blocks of the same type (Clusters). Additionally, any adjacent "Ska" blocks are "detonated" and added to the match count.
6.  **Cascading**: New blocks refill the grid (using weighted logic without "Ska" blocks for match-triggered refills). The process repeats until no more matches are found.
7.  **Resource Collection**: The UI (intended via UI Toolkit) tracks the count of each block type destroyed to simulate RPG actions (Attack, Defend, Heal, etc.).

## 3. Architecture
The project follows a centralized manager pattern with a focus on coroutine-based sequencing for visual animations and logic synchronization.

*   **Main Entry Point**: The `Main.unity` scene contains the `GridManager`, which acts as the "Brain" of the simulation.
*   **Input Handling**: Uses the **New Input System** to detect mouse clicks. `GridManager.Update` performs a 2D raycast to identify the clicked block.
*   **State Management**: Uses a boolean `isProcessing` flag to lock input during animations (falling, matching, exploding).
*   **Data Flow**:
    *   **Logic**: `BlockType[,] grid` stores the current state of the board.
    *   **Visuals**: `SpriteRenderer[,] renderers` maintains references to the GameObjects representing the blocks.
    *   **Communication**: `GridManager` directly calls `CameraShake.Shake` for tactile feedback.

## 4. Game Systems & Domain Concepts

### Grid & Match System
Handles the 7x7 logic, cluster detection, and the "Ska" detonation rules.
*   `GridManager`: The core class managing the grid array, match logic, and gravity.
*   `BlockType`: Enum defining the 7 types: `Sword`, `Shield`, `Magic`, `Heal`, `Gem`, `Key`, and `Ska`.
*   **Extension**: To add new block types, update the `BlockType` enum and the `iconSprites`/`blockColors` arrays in the `GridManager` inspector.

### Visual Effects (VFX) & Feedback
Provides visual polish for interactions.
*   `CameraShake`: Provides micro-impacts when blocks are destroyed or land.
*   **Coroutines**: `AnimateManualDestroy` and `AnimateMatchesAndDestroy` handle scaling, color shifting, and particle instantiation (`manualDestroyFX`, `matchDestroyFX`).
*   **Extension**: Visual tweaks (shake intensity, animation timing) are exposed as serialized fields in `GridManager`.

`Location: Assets/Scripts`

## 5. Scene Overview
*   **Main.unity**: The primary testing environment.
    *   **Background**: Static sprite rendering for the dungeon environment.
    *   **Fighter/Mage/Tank**: Placeholder animated characters using `Animator` and `Rigidbody2D`.
    *   **GridManager Object**: Holds the board logic and serves as the parent for all dynamically generated blocks.
    *   **UI Object**: Contains the `UIDocument` for the HUD.

## 6. UI System
*   **Framework**: **UI Toolkit (UITK)**.
*   **Structure**: Uses `Main.uxml` for the visual layout and `Main.uss` for styling.
*   **Binding**: The `GridManager` maintains a `matchCounts` array. Note: In the current prototype, the UI logic for updating these counts from the `GridManager` is planned for the `Main.uxml` "container" element.
*   **Adding Screens**: Create new `.uxml` files and add them to the `UIDocument` component or manage them via a UI Manager script (to be implemented).

`Location: Assets/UI`

## 7. Asset & Data Model
*   **ScriptableObjects**: Not currently used for data; settings are stored directly on the `GridManager` component.
*   **Prefabs**:
    *   `FX/ManualDestroyFX`, `MatchDestroyFX`, `ShockwaveFX`: Particle systems for feedback.
    *   `Characters/Monster`: Prefabs for enemy entities.
*   **Sprites**: Organized by category (`Blocks`, `Characters`, `FX`, `Backgrounds`). Block icons are assigned to the `GridManager` via the inspector.
*   **Naming Convention**: `Icon_[Type]` for block icons and `[Type]_Idle` for animations.

## 8. Notes, Caveats & Gotchas
*   **Bottom Row Only**: Players can only click blocks where `y = 0`. This is a strict design constraint of the prototype.
*   **Ska Logic**: "Ska" blocks (Type 6) never form matches themselves. They only disappear if they are adjacent to a match of another type (Sword, Shield, etc.).
*   **Manual vs. Automatic Refill**: Manual destruction (clicking) has a high probability (`manualSkaRate`) of spawning a "Ska" block. Automatic refills (from matches) use `typeWeights` and generally do not spawn "Ska" blocks.
*   **Object Lifecycle**: Blocks are instantiated as new GameObjects and destroyed physically. No object pooling is currently implemented. If performance becomes an issue with large chains, a pool for `SpriteRenderer` objects should be the first optimization.