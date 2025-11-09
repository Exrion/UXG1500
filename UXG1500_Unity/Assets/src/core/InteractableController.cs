using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableController : MonoBehaviour
{
    [SerializeField]
    private float m_InteractionDistance;
    [SerializeField]
    private Camera m_Camera;
    [SerializeField]
    private string m_InteractActionName;

    private InputAction m_InteractAction;
    private Stopwatch m_Stopwatch;

    private void Start()
    {
        m_InteractAction = InputSystem.actions.FindAction(m_InteractActionName);
    }

    private void Update()
    {
        if (m_InteractAction != null)
            CheckInteract();
    }

    private void CheckInteract()
    {
        if (m_InteractAction.WasReleasedThisFrame())
        {
            CastToInteractable();
            
        }
    }

    private void CastToInteractable()
    {

    }
}
