using System;
using UnityEngine;

class Interactable_SceneSwitch : IInteractable
{
    [SerializeField]
    private int m_SceneIndex;

    public override void OnInteracted()
    {
        GameManager.Instance.PrepareScene(m_SceneIndex);
        GameManager.Instance.ArmSceneSwitch();
    }
}
