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
        var blackout = root.Q("blackout-container");

        if (blackout != null)
        {
            // 1. Make visible and closed
            blackout.AddToClassList("blackout-container--visible");
            blackout.AddToClassList("blackout-container--active");
            
            // 2. Wait a small amount of time to ensure layout/draw call is registered
            yield return new WaitForSeconds(0.1f);
            
            // 3. Start the transition by removing 'active'
            blackout.RemoveFromClassList("blackout-container--active");

            // 4. Wait for the transition duration
            yield return new WaitForSeconds(0.4f);

            // 5. Finally hide completely
            blackout.RemoveFromClassList("blackout-container--visible");
        }
    }
}
