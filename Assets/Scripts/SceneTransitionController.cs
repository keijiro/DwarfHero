using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class SceneTransitionController : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    private void Start()
    {
        if (uiDocument == null)
            uiDocument = GetComponent<UIDocument>();

        if (uiDocument != null)
        {
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeIn()
    {
        var root = uiDocument.rootVisualElement;
        var blackout = root.Q("blackout");

        if (blackout != null)
        {
            // Ensure the blackout is active at start
            blackout.AddToClassList("blackout--active");
            
            // Wait a small amount of time to ensure layout/draw call
            yield return new WaitForSeconds(0.1f);
            
            // Start the transition to height 0
            blackout.RemoveFromClassList("blackout--active");
        }
    }
}
