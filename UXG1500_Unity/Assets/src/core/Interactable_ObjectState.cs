using System.Collections;
using UnityEngine;

class Interactable_ObjectState : IInteractable
{
    enum TargetState
    {
        On,
        Off,
        Toggle
    }

    [SerializeField]
    private GameObject m_gameObject;
    [SerializeField]
    private TargetState m_targetState;

    public override void OnInteracted()
    {
        switch (m_targetState)
        {
            case TargetState.On:
                m_gameObject.SetActive(true);
                break;
            case TargetState.Off:
                m_gameObject.SetActive(false);
                break;
            case TargetState.Toggle:
                m_gameObject.SetActive(!m_gameObject.activeSelf);
                break;
        }

    }
}