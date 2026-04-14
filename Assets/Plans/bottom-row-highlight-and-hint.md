# プロジェクト概要
- ゲームタイトル: マッチ3ダンジョンクローラー・プロトタイプ
- ハイレベルコンセプト: パズルアクションがダンジョン探索と戦闘を駆動する戦略的なマッチ3コンバット。
- プレイヤー: シングルプレイヤー
- 参考ゲーム: Dungeon Raid, Puzzle & Dragons
- トーン / アートディレクション: 2Dスプライトベース、ダークファンタジー / ダンジョンクローラーテーマ
- ターゲットプラットフォーム: PC (Standalone)
- レンダーパイプライン: URP

# ゲームメカニクス
## コアゲームプレイループ
プレイヤーは最下段（y=0）のブロックをクリックして破壊します。破壊されたブロックは「Ska」（死んだ）ブロックになります。重力によってブロックが落下し、3つ以上揃うとキャラクターのアクション（攻撃、魔法、回復、防御）が発動します。
## コントロールと入力方法
7x7グリッドの最下段をマウスクリックで操作します。

# UI
## 新規要素
- **スタートヒント**: ゲーム開始時に表示される「Click bottom row to destroy blocks!」という英語の内容のバルーンテキスト要素。
- **最下段の発光点滅**: 最下段のブロックがグリッド内の唯一の操作可能な部分であることを示すための視覚効果。

# 主要アセットとコンテキスト
- `Assets/Shaders/PulseOverlay.shader`: 発光点滅効果のためのカスタムURPシェーダー。
- `Assets/Scripts/RowHighlighter.cs`: 最下段の視覚的なパルスを管理します。
- `Assets/Scripts/StartHintController.cs`: 説明用のUIヒントの表示を処理します。
- `Assets/UI/Main.uxml` / `Main.uss`: ヒント表示用のUI Toolkitファイル。

# 実施ステップ
1. **シェーダー開発**:
    - `Assets/Shaders/PulseOverlay.shader` を作成。
    - 入力テクスチャのアルファ値を乗算しつつ、指定された `_PulseColor` でスプライトを塗りつぶすフラグメントシェーダーを実装。
    - URP互換性と適切なアルファブレンディング（Premultiplied Alpha）を確保。

2. **UI Toolkit の更新**:
    - `Assets/UI/Main.uxml` にスタートヒント用の `VisualElement` と `Label` を追加。
    - `Assets/UI/Main.uss` でヒントのスタイルを設定（水平中央、HUDの少し上、角丸の背景など）。
    - フェードアウトアニメーションのクラスを追加、または C# で制御。

3. **ハイライター・スクリプト**:
    - `Assets/Scripts/RowHighlighter.cs` を作成。
    - `GridManager` の最下段（y=0）を定期的にスキャン。
    - 最下段の各ブロックに対して、`PulseOverlay` マテリアルを使用したオーバーレイスプライトを生成（または再利用）。
    - サイン波や `Mathf.PingPong` を使用して `_PulseColor` のアルファ値を更新し、パルス効果を作成。
    - *依存関係*: `GridManager.cs` で `renderers` 配列を公開するか、最下段のレンダラーを取得するメソッドを追加する必要があります。

4. **ヒント・コントローラー**:
    - `Assets/Scripts/StartHintController.cs` を作成。
    - `Main.uxml` から UI 要素を参照。
    - `Start` でヒントを表示し（英語テキスト: "Click bottom row to destroy blocks!"）、数秒後に `Coroutine` を使用してフェードアウト・削除。

5. **統合**:
    - `RowHighlighter` と `StartHintController` を `Main` シーンのオブジェクト（おそらく `GridManager` と同じオブジェクト）にアタッチ。
    - `GridManager.cs` を更新して `renderers` を公開、またはゲッターを追加。

# 検証とテスト
- **視覚確認**: 最下段のブロックが指定した色で正しくパルス点滅することを確認。
- **UI確認**: ゲーム起動時に英語の説明ヒントが表示され、3〜5秒後に消えることを確認。
- **インタラクション確認**: ブロックが落下して入れ替わっても、パルス効果が常に最下段に維持されることを確認。
