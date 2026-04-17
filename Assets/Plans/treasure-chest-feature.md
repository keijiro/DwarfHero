# Project Overview
- Game Title: Match-3 Combat Hybrid (Elemental Battle)
- High-Level Concept: A real-time Match-3 RPG where player interactions on the bottom row trigger gravity and matches to attack enemies.
- Players: Single player
- Inspiration: Puzzle & Dragons, Match-3 RPGs
- Tone / Art Direction: Fantasy 2D, Cartoonish
- Target Platform: Standalone
- Screen Orientation: Landscape
- Render Pipeline: URP

# Game Mechanics
## Core Gameplay Loop
The player clears blocks on the bottom row of a 7x7 grid. Clearing blocks causes refills and cascades, which generate actions (Attack, Heal, Shield, EXP, Key). Enemies attack on timers. The loop continues until the wave is cleared or the player's HP reaches zero.

## Treasure Chest Feature (New)
After defeating all enemies in a wave and the "VICTORY" message fades, there is a 50% chance a Treasure Chest event occurs.
- **Treasure Chest Appearance:** A chest sprite appears with an RPG-style message window saying "You found a treasure chest!".
- **Opening Logic:**
  - **Success:** If the player has a Key (`HasKey == true`), the chest opens (sprite changes), bonus EXP is granted, the key is consumed, and a success message is shown.
  - **Failure:** If the player lacks a key, a failure message ("No key...") is shown for 1 second before the event ends.
- **Wave Transition:** After the event, the next wave of monsters approaches.

# UI
## Treasure Overlay (UITK)
- **treasure-overlay:** A full-screen container (picking-mode: Ignore) to hold the event UI.
- **treasure-chest-image:** A large VisualElement centered on the screen to display the chest sprite.
- **dialogue-box:** A decorative panel at the bottom center of the screen, styled like a classic RPG message window.
- **dialogue-text:** A Label inside the dialogue-box for messages.

# Key Asset & Context
- **Sprites:**
  - `Assets/Sprites/Chest_Closed.png`: Closed treasure chest.
  - `Assets/Sprites/Chest_Open.png`: Opened treasure chest.
- **Scripts:**
  - `Assets/Scripts/CombatManager.cs`: Main logic for wave transitions and stat management.
- **UI:**
  - `Assets/UI/Main.uxml`: Add the treasure overlay structure.
  - `Assets/UI/Main.uss`: Add styles for the dialogue box and chest image.

# Implementation Steps

## 1. Asset Generation
- Generate `Chest_Closed.png` and `Chest_Open.png` matching the project's art style using `generate-asset`.
- Save them in `Assets/Sprites/UI/`.

## 2. UI Layout & Styling
- **Modify `Assets/UI/Main.uxml`**:
  - Add a new `VisualElement` named `treasure-overlay` with a `treasure-chest-image` and a `dialogue-box` (containing a `Label` named `treasure-message`).
- **Modify `Assets/UI/Main.uss`**:
  - Create `.treasure-overlay` (absolute, 100%, hidden by default).
  - Create `.treasure-chest-image` (centered, fixed size, background-image).
  - Create `.dialogue-box` (bottom-center, semi-opaque background, decorative border, padding).
  - Add transition classes for fade-in/out effects.

## 3. Combat Logic Update
- **Modify `Assets/Scripts/CombatManager.cs`**:
  - Add fields for the new UI elements: `treasureOverlay`, `treasureImage`, `treasureMessage`.
  - Add `Sprite` fields for `chestClosedSprite` and `chestOpenSprite`.
  - Update `SetupUI()` to query these new elements.
  - Modify `HandleWaveClear()`:
    - After the "VICTORY" message routine finishes, check `Random.value < 0.5f`.
    - If true, `yield return StartCoroutine(TreasureChestEventRoutine())`.
  - Implement `TreasureChestEventRoutine()`:
    1. Reset `treasureImage` to `chestClosedSprite`.
    2. Set `treasureMessage.text = "You found a treasure chest!"`.
    3. Fade in `treasureOverlay`.
    4. Wait 1 second.
    5. Check `HasKey`.
    6. If `HasKey`:
       - `HasKey = false;`
       - `Experience += KeyBonusExp;`
       - Update UI (`UpdateUI()`).
       - Set `treasureImage` to `chestOpenSprite`.
       - `treasureMessage.text = "The chest was opened! You gained bonus EXP!";`
       - Play SE (`SEType.Key` or `SEType.Victory`).
       - Wait 2 seconds.
    7. Else:
       - `treasureMessage.text = "You don't have a key to open it...";`
       - Wait 1.5 seconds.
    8. Fade out `treasureOverlay`.
    9. Wait 0.5 seconds for transition.

# Verification & Testing
- **Probability Test:** Clear waves multiple times and verify that the chest appears roughly 50% of the time.
- **Key Logic Test:**
  - Defeat enemies after matching a Key block. Verify the chest opens and EXP increases.
  - Defeat enemies without matching a Key block. Verify the failure message appears and the chest disappears.
- **UI Alignment:** Ensure the dialogue box is properly centered at the bottom and the text is readable.
- **State Reset:** Confirm that `HasKey` becomes "NO" after opening the chest.
