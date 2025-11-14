using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[RequireComponent(typeof(Collider), typeof(Outline))]
public abstract class IInteractable : MonoBehaviour
{
    [SerializeField]
    private Interactable_Script m_InteractableScript;
    [SerializeField]
    private UIDocument m_InteractableDocument;
    [SerializeField]
    [Tooltip("Only required if Disable HUD On Interact is checked.")]
    private UIDocument m_HUDDocument;
    [SerializeField]
    [Tooltip("Only required if Disable FPS Controller On Interact is checked.")]
    private FirstPersonController m_FPSController;
    [SerializeField]
    private bool m_disableHUDOnInteract;
    [SerializeField]
    private bool m_disableFPSControllerOnInteract;

    private Outline m_OutlineScript;
    private float m_InteractHoldTime;
    private float m_InteractHoldTimeCurrent;
    private bool m_InProgress;

    protected virtual void Start()
    {
        m_OutlineScript = GetComponent<Outline>();
        m_OutlineScript.enabled = false;

        if (m_InteractableScript == null)
            Logger.Log("Interactable_Script not found in child of gameobject!",
                Logger.SEVERITY_LEVEL.ERROR,
                Logger.LOGGER_OPTIONS.VERBOSE,
                MethodBase.GetCurrentMethod());
        if (m_InteractableDocument == null)
            Logger.Log("UIDocument not found in child of gameobject!",
                Logger.SEVERITY_LEVEL.ERROR,
                Logger.LOGGER_OPTIONS.VERBOSE,
                MethodBase.GetCurrentMethod());
    }

    private void Update()
    {
        // Progress Radial
        if (m_InProgress && m_InteractableScript != null)
            m_InteractableScript.CalculateAndSetProgress(
                m_InteractHoldTimeCurrent += Time.deltaTime,
                m_InteractHoldTime);

        // Hide and Show Radial
        if (!m_InProgress && m_InteractableDocument != null)
            m_InteractableDocument.rootVisualElement.style.display = DisplayStyle.None;
        else if (m_InProgress && m_InteractableDocument != null)
            m_InteractableDocument.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    public virtual void HandleOutline(bool state)
    {
        m_OutlineScript.enabled = state;
    }

    public virtual void HandleInteractionStarted(float inputHoldTime)
    {
        m_InteractHoldTime = inputHoldTime;
        m_InProgress = true;
    }

    public void HandleInteractionPerformed()
    {
        ResetProgress();
        OnInteracted();
        if (m_disableHUDOnInteract && m_HUDDocument != null)
            ToggleHUD();
        if (m_disableFPSControllerOnInteract && m_FPSController != null)
            ToggleFPSController();
    }

    protected void ToggleHUD()
    {
        m_HUDDocument.rootVisualElement.style.display = DisplayStyle.None;
    }

    protected void ToggleFPSController()
    {
        m_FPSController.cameraCanMove = !m_FPSController.cameraCanMove;
        m_FPSController.playerCanMove = !m_FPSController.playerCanMove;
    }

    public virtual void HandleInteractionCancelled()
    {
        ResetProgress();
    }

    private void ResetProgress()
    {
        m_InProgress = false;
        m_InteractableScript.ResetProgress();
        m_InteractHoldTimeCurrent = 0f;
    }

    public abstract void OnInteracted();
}
