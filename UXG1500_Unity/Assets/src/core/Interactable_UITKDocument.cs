using UnityEngine;
using UnityEngine.UIElements;

class Interactable_UITKDocument : IInteractable
{
    [SerializeField]
    private UIDocument m_UIDocument;

    protected override void Start()
    {
        base.Start();

        if (m_UIDocument != null)
            m_UIDocument.rootVisualElement.style.display = DisplayStyle.None;
    }

    public override void OnInteracted()
    {
        if (m_UIDocument == null)
        {
            Logger.Log("UIDocument not assigned in gameobject!",
                Logger.SEVERITY_LEVEL.ERROR,
                Logger.LOGGER_OPTIONS.VERBOSE,
                System.Reflection.MethodBase.GetCurrentMethod());
            return;
        }

        if (m_UIDocument.rootVisualElement.style.display == DisplayStyle.Flex)
            m_UIDocument.rootVisualElement.style.display = DisplayStyle.None;
        else
            m_UIDocument.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    public void OnQuitUI(UIDocument newDocument = null)
    {
        ToggleFPSController();
        ToggleHUD();
        m_UIDocument.rootVisualElement.style.display = DisplayStyle.None;
        if (newDocument != null)
            m_UIDocument = newDocument;
    }
}
