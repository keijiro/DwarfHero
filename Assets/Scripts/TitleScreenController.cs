using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScreenController : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private string nextSceneName = "Main";

    private Label startMessage;
    private VisualElement root;

    private void OnEnable()
    {
        if (uiDocument == null)
            uiDocument = GetComponent<UIDocument>();

        if (uiDocument == null) return;

        root = uiDocument.rootVisualElement;
        startMessage = root.Q<Label>("start-message");

        // Make root clickable to start the adventure
        root.RegisterCallback<PointerDownEvent>(OnRootClicked);

        // Start blinking effect
        StartCoroutine(BlinkMessage());
    }

    private void OnDisable()
    {
        if (root != null)
        {
            root.UnregisterCallback<PointerDownEvent>(OnRootClicked);
        }
    }

    private IEnumerator BlinkMessage()
    {
        if (startMessage == null) yield break;

        while (true)
        {
            startMessage.ToggleInClassList("start-message--hidden");
            yield return new WaitForSeconds(0.8f);
        }
    }

    private void OnRootClicked(PointerDownEvent evt)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
