using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor;

class Interactable_Projector : IInteractable
{
    public S3Projector_Script m_ProjectorScript;

    public override void OnInteracted()
    {
        m_ProjectorScript.HandleInteracted();
    }
}