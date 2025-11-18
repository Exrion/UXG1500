using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;

class Interactable_Projector : IInteractable
{
    [SerializeField]
    private List<UIDocument> m_Slides = new();


    public override void OnInteracted()
    {
        throw new System.NotImplementedException();
    }
}