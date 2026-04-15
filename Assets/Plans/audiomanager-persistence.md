# AudioManager の永続化および専用シーンへの移行プラン

## プロジェクト概要
- ゲームタイトル: Dungeon Match-3 Combat Prototype
- コンセプト: パーティのアクションをグリッドの消去でトリガーする、ハイブリッドなマッチ3 RPG。
- プレイヤー数: シングルプレイヤー
- ターゲットプラットフォーム: スタンドアロン (PC/Mac)
- レンダリングパイプライン: URP

## プロジェクトの現状と課題
現在、`AudioManager` は `Main` シーン内に配置されています。しかし、この構成では `Title` シーンから `AudioManager` にアクセスできず、またシーン遷移（Title から Main など）を跨いで音を再生し続けることが困難です。
これを解決するために、専用のシーンを用いた「ブートストラップ」方式を導入し、`AudioManager` を全シーンで共通して利用できるようにします。

## 構成の変更方針
`AudioManager` を `Main` シーンから切り出し、専用の `PersistentSystems` シーン（永続システムシーン）に移動します。このシーンをゲーム起動時（またはプレイモード開始時）に加算ロード（Additive Load）し、`AudioManager` 自体には `DontDestroyOnLoad` を付与することで、シーン遷移後も破棄されずに残り続けるようにします。

## 主要なアセットとコンテキスト
- `Assets/Scripts/AudioManager.cs`: SE のプーリングとシングルトンロジックを担当する既存スクリプト。
- `Assets/Scenes/Main.unity`: 現在 `AudioManager` オブジェクトが配置されているシーン。
- `Assets/Scenes/Title.unity`: `AudioManager` へのアクセスが必要なシーン。
- `Assets/Scripts/PersistentSystemsLoader.cs`: 専用シーンを自動的に加算ロードするための新規スクリプト。
- `Assets/Scenes/PersistentSystems.unity`: グローバルマネージャーを保持するための新規シーン。

## 実施ステップ

### 1. AudioManager プレハブの作成
- **作業内容**: `Main` シーンにある `AudioManager` ゲームオブジェクトを `Assets/Prefabs` フォルダにドラッグ＆ドロップしてプレハブ化します（フォルダがない場合は作成します）。
- **目的**: 既存の設定（SE クリップの割り当て、プールサイズなど）を保持したまま、新しい専用シーンへ配置できるようにするため。
- **依存関係**: なし。

### 2. 永続システム専用シーンの作成
- **作業内容**: `Assets/Scenes/PersistentSystems.unity` という名前の新しいシーンを作成します。
- **作業内容**: 作成したシーンを開き、ステップ 1 で作成した `AudioManager` プレハブを配置します。
- **目的**: ゲーム全体で共有されるマネージャー類を管理するための、独立したコンテナを用意するため。
- **依存関係**: ステップ 1。

### 3. システムローダー（Bootstrapper）スクリプトの作成
- **作業内容**: `Assets/Scripts/PersistentSystemsLoader.cs` を作成します。
- **実装内容**:
    ```csharp
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public static class PersistentSystemsLoader
    {
        private const string SceneName = "PersistentSystems";

        // シーンロード前に自動的に実行されるメソッド
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadSystems()
        {
            // 既にロードされていないか確認（エディタでの重複ロード防止）
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name == SceneName) return;
            }

            // シーンを加算ロードする。
            // 中のオブジェクト（AudioManager）は DontDestroyOnLoad を持っているため、
            // 元のシーン（Title/Main）が Single モードでロードされても破棄されない。
            SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
        }
    }
    ```
- **目的**: どのシーンから開始しても、必ず最初にマネージャーシーンがロードされるようにし、`AudioManager.Instance` が常に利用可能な状態にするため。
- **依存関係**: ステップ 2。

### 4. 既存シーンのクリーンアップ
- **作業内容**: `Main` シーンから `AudioManager` オブジェクトを削除します。
- **作業内容**: `Title` シーンに重複する `AudioManager` がないことを確認します。
- **目的**: インスタンスの重複を防ぎ、新しい共通ローダーに一元化するため。
- **依存関係**: ステップ 3。

### 5. Build Settings への追加
- **作業内容**: `Assets/Scenes/PersistentSystems.unity` を Build Settings のシーンリストに追加します。
- **目的**: 実行時に `SceneManager.LoadScene` でシーンを見つけられるようにするため。
- **依存関係**: ステップ 3。

## 検証およびテスト項目
1. **Title シーンからの開始テスト**: `Title` シーンからプレイを開始し、ヒエラルキー上で `AudioManager` が `DontDestroyOnLoad` シーンに移動していることを確認します。
2. **シーン遷移テスト**: タイトル画面からゲームを開始し、`Main` シーンに遷移しても音が正常に鳴り、`AudioManager` が残り続けていることを確認します。
3. **Main シーン単体での開始テスト**: 開発用に `Main` シーンから直接プレイを開始しても、ローダーによって `AudioManager` が自動的に読み込まれることを確認します。
4. **重複チェック**: コンソールに `AudioManager` のインスタンス重複に関するエラーや警告が出ていないことを確認します。
