using UnityEngine;

public class RowHighlighter : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Material pulseMaterial;
    [SerializeField] private Color pulseColor = Color.white;
    [SerializeField, Range(1f, 10f)] private float pulseSpeed = 4f;
    [SerializeField, Range(0f, 1f)] private float maxAlpha = 0.5f;

    private SpriteRenderer[] highlightRenderers = new SpriteRenderer[GridManager.GridWidth];
    private static readonly int PulseColorID = Shader.PropertyToID("_PulseColor");

    private void Start()
    {
        if (gridManager == null) gridManager = GetComponent<GridManager>();
        
        // Create a separate container that is NOT a child of this.transform (GridManager)
        // to avoid being destroyed by GridManager.InitializeGrid()
        GameObject container = new GameObject("HighlightContainer");
        // No parent assignment or choose a different parent if necessary

        // Pre-create highlight objects for each column
        for (int x = 0; x < GridManager.GridWidth; x++)
        {
            GameObject obj = new GameObject($"Highlight_{x}");
            obj.transform.parent = container.transform;
            
            highlightRenderers[x] = obj.AddComponent<SpriteRenderer>();
            highlightRenderers[x].material = pulseMaterial;
            highlightRenderers[x].sortingOrder = 5; // Top layer
            highlightRenderers[x].enabled = false;
        }
    }

    private void Update()
    {
        if (gridManager == null) return;

        float alpha = (Mathf.Sin(Time.time * pulseSpeed) * 0.5f + 0.5f) * maxAlpha;
        Color c = pulseColor;
        c.a = alpha;

        for (int x = 0; x < GridManager.GridWidth; x++)
        {
            SpriteRenderer targetRenderer = gridManager.GetRenderer(x, 0);
            SpriteRenderer highlightSR = highlightRenderers[x];

            // Safety check in case objects were destroyed
            if (highlightSR == null) continue;
            
            if (targetRenderer != null && targetRenderer.gameObject.activeInHierarchy)
            {
                highlightSR.enabled = true;
                highlightSR.sprite = targetRenderer.sprite;
                highlightSR.transform.position = targetRenderer.transform.position;
                highlightSR.transform.localScale = targetRenderer.transform.localScale;
                highlightSR.material.SetColor(PulseColorID, c);
            }
            else
            {
                highlightSR.enabled = false;
            }
        }
    }
}
