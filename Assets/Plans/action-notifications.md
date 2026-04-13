# プロジェクト概要
- ゲームタイトル: 3-Match RPG Prototype
- コンセプト: 7x7 グリッドの最下段（y=0）のみを操作する戦略的3マッチRPG。マッチしたブロックのクラスターに応じて、攻撃、防御、魔法などのリソースを生成し、戦闘イベントキューに追加します。
- プレイヤー数: 1人（シングルプレイヤー）
- インスピレーション: 10000000, You Must Build A Boat.
- トーン / アートディレクション: ゴシック調のフォント（Pirata One）とドット絵を組み合わせたRPG/ファンタジー。
- ターゲットプラットフォーム: スタンドアロン (PC/Mac)
- 画面方向 / 解像度: 横画面（ランドスケープ）
- レンダーパイプライン: URP

# ゲームメカニクス
## コアゲームプレイ・ループ
1. **手動操作**: プレイヤーが最下段のブロックをクリックして破壊し、重力による落下を誘発する。
2. **連鎖とマッチ**: 落下により、3つ以上の同種ブロックが縦または横に揃う（マッチ）。
3. **クラスターと Ska の誘爆**: マッチしたブロックの周囲にある同種ブロックを探索（クラスター）。隣接する「Ska（死のブロック）」も同時に消去される。
4. **アクションのキューイング**: 見つかったクラスターは `CombatAction`（攻撃、回復など）に変換され、`CombatManager` のイベントキューに追加される。
5. **ターンの実行**: `CombatManager` がキューを順番に処理し、キャラクターアニメーションの再生とステータス（HP、Shield、EXP）の更新を行う。

## 操作方法
- **マウス左クリック (New Input System)**: グリッドの最下段（y=0）のみ操作可能。

# UI
- **HUD**: UI Toolkit を使用し、HPバー、Shield、EXP、鍵の所持状態を画面下部/端に表示。
- **アクション通知 (新機能)**: アクションがキューに追加された際に表示される動的なフローティングテキスト。
    - 表示位置: 画面左端から 25%、上端から 50% の位置を基準に、中央寄せで表示。
    - アニメーション: 少し下から ease-out で出現し、ゆっくり上に移動しながらフェードアウトして消える。
    - スタイル: Pirata One フォントを使用。アクションごとに異なる色を適用（例：攻撃は赤）。

# 主要アセットと文脈
- `Assets/UI/Main.uxml`: UI 構造定義。
- `Assets/UI/Main.uss`: UI スタイル定義。
- `Assets/Scripts/CombatManager.cs`: ステータス管理、イベントキューの処理、および通知の生成。
- `Assets/UI/Pirata_One/PirataOne-Regular SDF.asset`: 使用するフォント。

# 実装ステップ
1. **`Main.uxml` の更新**:
    - `root` コンテナ内に、通知ラベルの親となる `notification-layer` という名前の `VisualElement` を追加。
    - クリックを阻害しないよう `picking-mode="Ignore"` を設定。
2. **`Main.uss` の更新**:
    - `.notification-layer`: 画面全体を覆う絶対配置を設定 (`position: absolute; width: 100%; height: 100%;`)。
    - `.notification-label`: フローティングテキストの基本スタイルを定義。
        - `position: absolute; left: 25%; top: 50%;`
        - `translate: -50% -50%;` (基準位置で中央寄せ)
        - `-unity-text-align: middle-center;`
        - `font-size: 36px;`
        - `-unity-font-style: bold;`
        - `color: white;` (ベース色)
        - `text-shadow: 1px 1px 2px rgba(0,0,0,0.8);`
3. **`CombatManager.cs` の更新**:
    - `VisualElement notificationLayer` フィールドを追加し、`SetupUI()` 内で初期化。
    - 通知の出現とフェードアウトのアニメーションを制御する `IEnumerator AnimateNotification(Label label, Color color)` を実装。
    - アクションの種類と値を基にラベルを生成・初期化し、アニメーションを開始する `ShowActionNotification(CombatActionType type, int value)` メソッドを実装。
    - アクションタイプごとのカラーマッピングを定義（例：Attack=赤、Shield=水色、Heal=緑、Magic=紫、Exp=シアン、Key=黄色）。
    - `AddPlayerAction(...)` 内で `ShowActionNotification` を呼び出し、アクションがキューに積まれる瞬間に通知が表示されるように統合。

# 検証とテスト
1. **手動プレイ**: 最下段のブロックをクリックしてマッチを発生させる。
2. **通知の確認**: "Attack! 10 pts." などのテキストが画面左中央付近（25%/50%）に正しく表示されるか確認。
3. **アニメーションの確認**:
    - 少し下から ease-out で「フワッ」と出現するか？
    - ゆっくり上に移動しながら消えていくか？
    - 階層から正しく削除（RemoveFromHierarchy）されているか？
4. **色とテキストの確認**: 攻撃は赤、回復は緑など、アクションに応じた適切な表示がなされているか。
5. **同時連鎖テスト**: 複数のマッチが連続して発生した際に、通知が重なりすぎず、読み取り可能であるかを確認。
