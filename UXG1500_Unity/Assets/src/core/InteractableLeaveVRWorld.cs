using System;
using UnityEngine;
using UnityEngine.SceneManagement;

class InteractableLeaveVRWorld : IInteractable
{
    public override void OnInteracted()
    {
        GameManager.Instance.PrepareScene(2);
        GameManager.Instance.ArmSceneSwitch();
    }
}
