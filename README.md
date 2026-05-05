# Dungeon Match Heroes

**Dungeon Match Heroes** is a sample project created using Unity AI Assistant.

<img src="Assets/Sprites/UI/Title_Logo.png" width="200">

## ⚔️ The Tale of the Three Heroes
In the deep, forgotten corridors of the Ancient Dungeon, a trio of heroes—the stalwart Fighter, the mystical Mage, and the ironclad Tank—stands as the last line of defense against an endless tide of monsters. To survive, they must harness the power of the **Mystic Grid**. By breaking the very foundations of the grid, they trigger cascading reactions that empower their blades, shields, and spells. Will you guide them to victory, or will they be overwhelmed by the darkness?

Play it on [Unity Play] or [unityroom]!

[Unity Play]: https://play.unity.com/en/games/399e2e07-4d3f-462f-9a5c-6a8fb2c68c1a/dungeon-match-heroes
[unityroom]: https://unityroom.com/games/dungeonmatch

<img src="Assets/Sprites/UI/Title_Heroes.png" width="200">

---

## 🕹️ How to Play

### Click-Shatter to Start
The game is played on a **7x7 grid**. Unlike traditional match-3 games where you swap pieces, here you directly interact with the **Bottom Row**.
- **Click-Shatter** any block on the bottom row to destroy it.
- Blocks above will fall down (**Gravity**), and new blocks will refill the grid from the top.
- Falling blocks that create a line of 3 or more of the same type will **Match**.

---

## 📜 Rules of the Realm

### 💎 Item Blocks
Each block type corresponds to a specific action for your heroes:
- <img src="Assets/Sprites/Blocks/Icon_Sword.png" width="40"> **Sword (Red):** Commands the Fighter to perform a physical strike on the nearest enemy.
- <img src="Assets/Sprites/Blocks/Icon_Shield.png" width="40"> **Shield (Blue):** Adds to your party's Shield points, parrying incoming physical attacks.
- <img src="Assets/Sprites/Blocks/Icon_Magic.png" width="40"> **Magic (Purple):** Empowers the Mage to strike the entire enemy party at once.
- <img src="Assets/Sprites/Blocks/Icon_Heal.png" width="40"> **Heal (Green):** Restores your party's HP. If your HP is full, it grants bonus EXP!
- <img src="Assets/Sprites/Blocks/Icon_Gem.png" width="40"> **Gem (Cyan):** Sharpens your skills, granting Experience Points (EXP).
- <img src="Assets/Sprites/Blocks/Icon_Key.png" width="40"> **Key (Yellow):** Grants you a Key. Use it to open treasure chests found between waves.

### ❓ "?" Block
The grid contains special blocks marked with a question mark. These blocks follow a unique cycle and cannot be matched like normal item blocks.
- **Clearing:** These blocks must be cleared by **Click-Shattering** them directly or by a **Match Blast** from an adjacent match.
- **The Cycle:** Click-Shattering an item block always causes a "?" block to drop from above as a refill. Conversely, clearing a "?" block will result in a new item block dropping into the grid.
- **Strategy:** If there are no obvious matches available, focus on Click-Shattering "?" blocks to shift the grid and uncover new matches.

### 🛡️ Combat Mechanics
- **Real-time Battle:** Enemies don't wait for your turn! They have their own attack timers.
- **Formation:** Enemies in the front (Vanguard) attack faster than those in the back.
- **Physical vs. Magic:** Shields are effective against physical strikes but offer no protection against enemy sorcery.
- **Treasure Chests:** After clearing a wave, you may find a chest. You can only carry one Key at a time, so use it or lose it!
