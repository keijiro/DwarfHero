# Listing Current Implementation TIPS Contents

- **Date:** 2026-05-01 22:12
- **Task ID:** `5dcde7de-e0e6-40f0-8932-1ab34080acd9`
- **Messages:** 2

---

## 🧑 User

現在の実装において TIPS として表示されている内容を列挙してください。

## 🤖 Assistant

現在、`Assets/Scripts/CombatManager.cs` 内の `TIPS` 配列に定義されている内容は以下の通りです。

1.  **"Ska" (?) blocks cannot be matched. Shatter or Blast them.**（「Ska」ブロックはマッチさせることができません。破壊（Shatter）するか爆破（Blast）してください。）
2.  **Shattering an elemental block drops a "Ska" block from above.**（属性ブロックを破壊すると、上から「Ska」ブロックが落ちてきます。）
3.  **Shattering or Blasting a "Ska" block drops an elemental block from above.**（「Ska」ブロックを破壊または爆破すると、上から属性ブロックが落ちてきます。）
4.  **If stuck, focus on shattering "Ska" blocks to find new matches.**（手詰まりになったら、「Ska」ブロックを破壊して新しいマッチを見つけることに集中しましょう。）
5.  **Match Shields to block incoming physical attacks.**（シールドをマッチさせて、敵の物理攻撃を防ぎましょう。）
6.  **Magic matches hit the entire enemy party.**（魔法のマッチは敵パーティ全体にダメージを与えます。）
7.  **Shields cannot exceed your Max HP.**（シールドの値は最大HPを超えることはありません。）
8.  **Shields cannot block enemy magic attacks.**（シールドで敵の魔法攻撃を防ぐことはできません。）
9.  **A Key is needed for chests, but you can only carry one.**（宝箱を開けるには鍵が必要ですが、持てるのは1つだけです。）
10. **Match Gems to gain EXP.**（ジェムをマッチさせて経験値を獲得しましょう。）
11. **Matching Heal blocks at full HP grants EXP.**（HPが満タンの時に回復ブロックをマッチさせると、経験値が獲得できます。）
12. **Matching Keys while holding one grants EXP.**（既に鍵を持っている時に鍵をマッチさせると、経験値が獲得できます。）
13. **Enemies in the back row attack more slowly.**（後列の敵は攻撃速度が遅くなります。）
14. **Blasting "Ska" blocks near your matches slightly increases their power.**（マッチの近くで「Ska」ブロックを爆破すると、わずかに威力が上昇します。）

<details>
<summary>🔧 ツール: Unity.Grep (CoreAgent)</summary>

**引数**

```json
{
  "args": "-i \"Tips\""
}
```

**結果**

```
Assets/Scripts/CombatManager.cs
94:    private static readonly string[] TIPS = new string[]
795:        if (nextTipIndex >= TIPS.Length) yield break;
798:        string currentTip = TIPS[nextTipIndex];

```

</details>

<details>
<summary>🔧 ツール: Unity.GetFileContent (CoreAgent)</summary>

**引数**

```json
{
  "endLine": 110,
  "filePath": "Assets/Scripts/CombatManager.cs",
  "startLine": 90
}
```

**結果**

```
90:     private VisualElement tipIcon;
91:     private VisualElement dialogueBox;
92:     private Label treasureMessage;
93: 
94:     private static readonly string[] TIPS = new string[]
95:     {
96:         "\"Ska\" (?) blocks cannot be matched. Shatter or Blast them.",
97:         "Shattering an elemental block drops a \"Ska\" block from above.",
98:         "Shattering or Blasting a \"Ska\" block drops an elemental block from above.",
99:         "If stuck, focus on shattering \"Ska\" blocks to find new matches.",
100:         "Match Shields to block incoming physical attacks.",
101:         "Magic matches hit the entire enemy party.",
102:         "Shields cannot exceed your Max HP.",
103:         "Shields cannot block enemy magic attacks.",
104:         "A Key is needed for chests, but you can only carry one.",
105:         "Match Gems to gain EXP.",
106:         "Matching Heal blocks at full HP grants EXP.",
107:         "Matching Keys while holding one grants EXP.",
108:         "Enemies in the back row attack more slowly.",
109:         "Blasting \"Ska\" blocks near your matches slightly increases their power."
110:     };

```

</details>