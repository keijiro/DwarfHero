using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System.Collections.Generic;

/// <summary>
/// 7x7の盤面管理、マッチ判定、落下、補充、入力処理を統括する。
/// </summary>
public class GridManager : MonoBehaviour
{
    public enum BlockType
    {
        Sword = 0,   // Red
        Shield = 1,  // Blue
        Magic = 2,   // Purple
        Heal = 3,    // Green
        Gem = 4,     // Yellow
        Key = 5,     // White
        Ska = 6      // Gray (Empty effect)
    }

    private const int GridWidth = 7;
    private const int GridHeight = 7;
    private const float BlockSpacing = 1.1f;

    [Header("Assets")]
    [SerializeField] private Sprite whiteSprite;
    [SerializeField] private UIDocument uiDocument;

    [Header("Generation Settings")]
    [Tooltip("Weights for Sword, Shield, Magic, Heal, Gem, Key (Ska is handled separately)")]
    [SerializeField] private float[] typeWeights = new float[6] { 1.0f, 1.0f, 0.5f, 0.5f, 0.2f, 0.2f };

    private BlockType[,] grid = new BlockType[GridWidth, GridHeight];
    private SpriteRenderer[,] renderers = new SpriteRenderer[GridWidth, GridHeight];
    private int[] matchCounts = new int[7];

    private bool isProcessing = false;

    private void Awake()
    {
        Debug.Log("GridManager.Awake() called.");
    }

    private void Start()
    {
        Debug.Log("GridManager.Start() called.");
        if (uiDocument == null) uiDocument = FindFirstObjectByType<UIDocument>();
        if (whiteSprite == null) Debug.LogError("whiteSprite is NULL in GridManager!");
        
        InitializeGrid();
        UpdateUI();
        
        int count = 0;
        foreach (var r in renderers) if (r != null) count++;
        Debug.Log($"Grid initialized. {count} blocks created as children of {name}.");
    }

    private void InitializeGrid()
    {
        // Clear existing children first to avoid duplicates
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                BlockType type;
                do
                {
                    type = GetWeightedRandomType();
                } while (WouldMatch(x, y, type));
                
                grid[x, y] = type;
                renderers[x, y] = CreateBlockObject(x, y, type);
            }
        }
    }

    private BlockType GetWeightedRandomType()
    {
        float totalWeight = 0;
        foreach (float w in typeWeights) totalWeight += w;

        float r = Random.value * totalWeight;
        float currentWeight = 0;

        for (int i = 0; i < typeWeights.Length; i++)
        {
            currentWeight += typeWeights[i];
            if (r <= currentWeight) return (BlockType)i;
        }

        return BlockType.Sword;
    }

    private bool WouldMatch(int x, int y, BlockType type)
{
        if (x >= 2 && grid[x - 1, y] == type && grid[x - 2, y] == type) return true;
        if (y >= 2 && grid[x, y - 1] == type && grid[x, y - 2] == type) return true;
        return false;
    }

    private SpriteRenderer CreateBlockObject(int x, int y, BlockType type)
    {
        GameObject obj = new GameObject($"Block_{x}_{y}");
        obj.transform.parent = this.transform;
        obj.transform.position = GetWorldPosition(x, y);
        
        var renderer = obj.AddComponent<SpriteRenderer>();
        renderer.sprite = whiteSprite;
        renderer.color = GetColor(type);
        
        // クリック判定用にコライダーを追加
        var col = obj.AddComponent<BoxCollider2D>();
        col.size = Vector2.one;

        return renderer;
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x - (GridWidth - 1) / 2f, y - (GridHeight - 1) / 2f, 0) * BlockSpacing;
    }

    private Color GetColor(BlockType type)
    {
        return type switch
        {
            BlockType.Sword => Color.red,
            BlockType.Shield => Color.blue,
            BlockType.Magic => new Color(0.6f, 0f, 0.8f), // Purple
            BlockType.Heal => Color.green,
            BlockType.Gem => Color.cyan,   // Light Blue (Updated)
            BlockType.Key => Color.yellow, // Yellow (Updated)
            BlockType.Ska => Color.gray,
            _ => Color.white
        };
    }

    private void Update()
    {
        // 処理中は入力を受け付けない
        if (isProcessing) return;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleClick();
        }
    }

    private void HandleClick()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null)
        {
            // 最下段（y = 0）のみ干渉可能
            for (int x = 0; x < GridWidth; x++)
            {
                if (hit.collider.gameObject == renderers[x, 0]?.gameObject)
                {
                    StartCoroutine(ProcessMove(x, 0));
                    break;
                }
            }
        }
    }

    private System.Collections.IEnumerator ProcessMove(int x, int y)
    {
        isProcessing = true;

        // 指定ブロックを破壊
        DestroyBlock(x, y);

        // 落下と補充（手動破壊時はスカ発生率高）
        bool firstRefill = true;
        bool hasMatches;

        do
        {
            yield return StartCoroutine(HandleGravityAndRefill(firstRefill));
            firstRefill = false;
            
            // マッチ判定
            hasMatches = CheckAndApplyMatches();
            if (hasMatches)
            {
                UpdateUI();
                yield return new WaitForSeconds(0.2f);
            }
        } while (hasMatches);

        isProcessing = false;
    }

    private void DestroyBlock(int x, int y)
    {
        if (renderers[x, y] != null)
        {
            Destroy(renderers[x, y].gameObject);
            renderers[x, y] = null;
            // 論理的な値もリセット
            grid[x, y] = (BlockType)(-1); 
        }
    }

    private System.Collections.IEnumerator HandleGravityAndRefill(bool isManualRefill)
    {
        // 1. 落下
        for (int x = 0; x < GridWidth; x++)
        {
            int emptyCount = 0;
            for (int y = 0; y < GridHeight; y++)
            {
                if (renderers[x, y] == null)
                {
                    emptyCount++;
                }
                else if (emptyCount > 0)
                {
                    // 落下移動
                    grid[x, y - emptyCount] = grid[x, y];
                    renderers[x, y - emptyCount] = renderers[x, y];
                    renderers[x, y - emptyCount].gameObject.name = $"Block_{x}_{y - emptyCount}";
                    renderers[x, y - emptyCount].transform.position = GetWorldPosition(x, y - emptyCount);
                    
                    grid[x, y] = (BlockType)(-1);
                    renderers[x, y] = null;
                }
            }

            // 2. 補充
            for (int i = 0; i < emptyCount; i++)
            {
                int y = GridHeight - emptyCount + i;
                BlockType newType = DecideNewBlockType(isManualRefill);
                grid[x, y] = newType;
                renderers[x, y] = CreateBlockObject(x, y, newType);
            }
        }
        yield return null;
    }

    private BlockType DecideNewBlockType(bool isManualRefill)
    {
        if (isManualRefill)
        {
            // 手動破壊時の補充は「スカ」の発生率を高くする (例: 50%)
            if (Random.value < 0.5f) return BlockType.Ska;
            return GetWeightedRandomType();
        }
        else
        {
            // マッチ時の補充では効果のあるブロックだけが登場する
            return GetWeightedRandomType();
        }
    }

    private bool CheckAndApplyMatches()
    {
        HashSet<(int, int)> triggerSet = new HashSet<(int, int)>();

        // 横のマッチ判定（トリガーとなる3列以上の並びを探す）
        for (int y = 0; y < GridHeight; y++)
        {
            for (int x = 0; x < GridWidth - 2; x++)
            {
                BlockType type = grid[x, y];
                if ((int)type == -1 || type == BlockType.Ska) continue;
                if (grid[x + 1, y] == type && grid[x + 2, y] == type)
                {
                    triggerSet.Add((x, y));
                    triggerSet.Add((x + 1, y));
                    triggerSet.Add((x + 2, y));
                }
            }
        }

        // 縦のマッチ判定
        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight - 2; y++)
            {
                BlockType type = grid[x, y];
                if ((int)type == -1 || type == BlockType.Ska) continue;
                if (grid[x, y + 1] == type && grid[x, y + 2] == type)
                {
                    triggerSet.Add((x, y));
                    triggerSet.Add((x, y + 1));
                    triggerSet.Add((x, y + 2));
                }
            }
        }

        if (triggerSet.Count > 0)
        {
            HashSet<(int, int)> finalMatchedSet = new HashSet<(int, int)>();
            HashSet<(int, int)> visited = new HashSet<(int, int)>();

            foreach (var pos in triggerSet)
            {
                if (visited.Contains(pos)) continue;

                // このブロックを含む、同色の隣接した全ブロックを抽出
                List<(int, int)> cluster = new List<(int, int)>();
                FindCluster(pos.Item1, pos.Item2, grid[pos.Item1, pos.Item2], cluster, visited);
                foreach (var c in cluster) finalMatchedSet.Add(c);
            }

            HashSet<(int, int)> alreadyDestroyedSka = new HashSet<(int, int)>();
            int[] dx = { 1, -1, 0, 0 };
            int[] dy = { 0, 0, 1, -1 };

            foreach (var pos in finalMatchedSet)
            {
                BlockType triggerType = grid[pos.Item1, pos.Item2];
                matchCounts[(int)triggerType]++; // マッチしたブロック自身のポイント

                // 周囲のスカブロックを誘爆
                for (int i = 0; i < 4; i++)
                {
                    int nx = pos.Item1 + dx[i];
                    int ny = pos.Item2 + dy[i];

                    if (nx >= 0 && nx < GridWidth && ny >= 0 && ny < GridHeight)
                    {
                        if (grid[nx, ny] == BlockType.Ska && !alreadyDestroyedSka.Contains((nx, ny)))
                        {
                            // 誘爆元となったブロック種別のポイントに加算
                            matchCounts[(int)triggerType]++;
                            alreadyDestroyedSka.Add((nx, ny));
                        }
                    }
                }
            }

            // 論理・物理的な破壊の実行
            foreach (var pos in finalMatchedSet) DestroyBlock(pos.Item1, pos.Item2);
            foreach (var pos in alreadyDestroyedSka) DestroyBlock(pos.Item1, pos.Item2);

            return true;
        }
        return false;
        }

        private void FindCluster(int x, int y, BlockType type, List<(int, int)> cluster, HashSet<(int, int)> visited)
        {
        if (x < 0 || x >= GridWidth || y < 0 || y >= GridHeight) return;
        if (visited.Contains((x, y)) || grid[x, y] != type) return;

        visited.Add((x, y));
        cluster.Add((x, y));

        // 4方向を再帰的に探索
        FindCluster(x + 1, y, type, cluster, visited);
        FindCluster(x - 1, y, type, cluster, visited);
        FindCluster(x, y + 1, type, cluster, visited);
        FindCluster(x, y - 1, type, cluster, visited);
        }

        private void UpdateUI()
        {
        if (uiDocument == null) uiDocument = FindFirstObjectByType<UIDocument>();
        if (uiDocument == null || uiDocument.rootVisualElement == null) return;

        var root = uiDocument.rootVisualElement;
        
        UpdateLabel(root, "SwordCount", matchCounts[0]);
        UpdateLabel(root, "ShieldCount", matchCounts[1]);
        UpdateLabel(root, "MagicCount", matchCounts[2]);
        UpdateLabel(root, "HealCount", matchCounts[3]);
        UpdateLabel(root, "GemCount", matchCounts[4]);
        UpdateLabel(root, "KeyCount", matchCounts[5]);
        }

    private bool UpdateLabel(VisualElement root, string name, int value)
    {
        var label = root.Q<Label>(name);
        if (label != null)
        {
            label.text = value.ToString();
            return true;
        }
        return false;
    }
    }
