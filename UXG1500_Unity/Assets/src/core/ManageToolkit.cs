using UnityEngine;
using UnityEngine.UIElements;

public class CRTUIManager : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public UIDocument bootDocument;
    public UIDocument gameDocument;

    private bool bootActive = true;

    void Start()
    {
        // Start with only the boot UI visible
        ShowBootUI(true);
    }

    public void OnBootCompleteAnyKey()
    {
        Debug.Log("Boot sequence complete. Switching to game UI...");

        ShowBootUI(false); // Hide boot, show game
    }

    private void ShowBootUI(bool show)
    {
        bootActive = show;

        if (bootDocument != null)
            bootDocument.rootVisualElement.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;

        if (gameDocument != null)
            gameDocument.rootVisualElement.style.display = show ? DisplayStyle.None : DisplayStyle.Flex;
    }
}
