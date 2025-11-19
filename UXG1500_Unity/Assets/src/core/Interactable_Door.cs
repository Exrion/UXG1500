using System;
using System.Collections;
using UnityEngine;


public class Interactable_Door : IInteractable
{
    [SerializeField]
    OpenDoor m_openDoor;

    protected override void Start()
    {
        base.Start();
        m_openDoor.Init();
    }

    //Update is called once per frame

    public override void OnInteracted()
    {
        m_openDoor.HandleInteraction();
    }
}
    