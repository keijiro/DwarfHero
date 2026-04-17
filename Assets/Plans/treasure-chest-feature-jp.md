# プロジェクト概要
- ゲームタイトル: マッチ3コンバットハイブリッド（エレメンタルバトル）
- ハイレベルコンセプト: 7x7グリッドの最下段をプレイヤーが操作し、重力と連鎖で発生するマッチを利用して敵とリアルタイムで戦うパズルRPG。
- プレイヤー数: 1人
- インスピレーション: パズドラ、マッチ3 RPG
- トーン / アートディレクション: ファンタジー2D、カートゥーン調
- ターゲットプラットフォーム: スタンドアロン
- 画面方向: 横向き（Landscape）
- レンダーパイプライン: URP

# ゲームメカニクス
## コアゲームプレイループ
プレイヤーは7x7グリッドの最下段にあるブロックを消去します。ブロックを消去すると上が降りてきて連鎖（カスケード）が発生し、各種アクション（攻撃、回復、シールド、経験値、鍵）が実行されます。敵は独自のタイマーで攻撃してきます。ウェーブを全滅させるか、プレイヤーのHPが0になるまでループが続きます。

## 宝箱機能（新規）
敵のウェーブをクリアし、「VICTORY」メッセージが消えた後、50%の確率で宝箱イベントが発生します。
- **宝箱の登場:** 宝箱のスプライトが表示され、RPG風のメッセージウィンドウに「You found a treasure chest!」（宝箱を見つけた！）と表示されます。
- **開封ロジック:**
  - **成功:** プレイヤーが鍵を所有している場合（`HasKey == true`）、宝箱が開き（スプライトが開封状態に変化）、ボーナスEXPが付与され、鍵が消費されます。成功メッセージが表示されます。
  - **失敗:** 鍵を持っていない場合、失敗メッセージ（「鍵がないので開けられなかった...」）が1秒間表示され、イベントが終了します。
- **ウェーブ遷移:** イベント終了後、次のモンスターのウェーブが開始されます。

# UI
## 宝箱オーバーレイ (UITK)
- **treasure-overlay:** イベントUIを保持するためのフルスクリーンコンテナ（picking-mode: Ignore）。
- **treasure-chest-image:** 画面中央に配置され、宝箱のスプライトを表示するVisualElement。
- **dialogue-box:** 画面下部中央に配置される装飾的なパネル。クラシックなRPGのメッセージウィンドウ風のスタイル（半透明の背景、境界線、パディング）。
- **dialogue-text:** `dialogue-box` 内のラベル。メッセージを表示します。

# キーアセット & コンテキスト
- **スプライト:**
  - `Assets/Sprites/Chest_Closed.png`: 閉じている宝箱。
  - `Assets/Sprites/Chest_Open.png`: 開いている宝箱。
- **スクリプト:**
  - `Assets/Scripts/CombatManager.cs`: ウェーブ遷移とステータス管理のメインロジック。
- **UI:**
  - `Assets/UI/Main.uxml`: 宝箱オーバーレイの構造を追加。
  - `Assets/UI/Main.uss`: ダイアログボックスと宝箱画像のスタイルを追加。

# 実装ステップ

## 1. アセット生成
- `generate-asset` ツールを使用して、プロジェクトのアートスタイルに合わせた `Chest_Closed.png` と `Chest_Open.png` を生成します。
- `Assets/Sprites/UI/` に保存します。

## 2. UIレイアウトとスタイリング
- **`Assets/UI/Main.uxml` の修正**:
  - `treasure-overlay`、`treasure-chest-image`、`dialogue-box`（中に `treasure-message` という `Label` を含む）を追加。
- **`Assets/UI/Main.uss` の修正**:
  - `.treasure-overlay`（絶対配置、100%サイズ、デフォルトは非表示）を定義。
  - `.treasure-chest-image`（中央配置、固定サイズ、背景画像）。
  - `.dialogue-box`（下部中央、半透明の背景、装飾的な枠線、フォント指定）。
  - フェードイン/アウト効果のための遷移（Transition）クラスを追加。

## 3. コンバットロジックの更新
- **`Assets/Scripts/CombatManager.cs` の修正**:
  - 新しいUI要素のためのフィールドを追加: `treasureOverlay` (VisualElement), `treasureMessage` (Label)。
  - `chestClosedSprite` と `chestOpenSprite` のフィールドを追加。
  - `SetupUI()` でこれらの新しい要素を取得するように更新。
  - `HandleWaveClear()` を修正:
    - 「VICTORY」メッセージのルーチン終了後、`Random.value < 0.5f` をチェック。
    - 真であれば、`yield return StartCoroutine(TreasureChestEventRoutine())` を実行。
  - `TreasureChestEventRoutine()` の実装:
    1. `treasureImage` を `chestClosedSprite` にリセット。
    2. `treasureMessage.text = "You found a treasure chest!"`。
    3. `treasureOverlay` をフェードイン。
    4. 1秒待機。
    5. `HasKey` をチェック。
    6. **鍵がある場合**:
       - `HasKey = false;`
       - `Experience += KeyBonusExp;`
       - `UpdateUI();`
       - `treasureImage` を `chestOpenSprite` に変更。
       - `treasureMessage.text = "The chest opened! You got bonus EXP!";`
       - 効果音（`SEType.Key` または `SEType.Victory`）を再生。
       - 2秒待機。
    7. **鍵がない場合**:
       - `treasureMessage.text = "You don't have a key to open it...";`
       - 1.5秒待機。
    8. `treasureOverlay` をフェードアウト。
    9. 遷移のために0.5秒待機。

# 検証とテスト
- **確率テスト:** ウェーブを複数回クリアし、約50%の確率で宝箱が出現することを確認。
- **鍵のロジックテスト:**
  - 鍵ブロックをマッチさせた後に敵を倒し、宝箱が開いてEXPが増えることを確認。
  - 鍵を持たずに敵を倒し、失敗メッセージが表示されて宝箱が消えることを確認。
- **UIのアライメント:** ダイアログボックスが下部中央に正しく配置され、テキストが読みやすいことを確認。
- **状態のリセット:** 宝箱を開けた後、`HasKey` が "NO" に戻ることを確認。
