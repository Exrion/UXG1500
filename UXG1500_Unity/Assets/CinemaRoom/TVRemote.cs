using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TVRemoteControl : IInteractable
{
    public GameObject tvRemote;

    public bool tvIsOn = false;


    public VideoPlayer videoPlayer;
    protected override void Start()
    {
        base.Start();
        TurnTvOff(); // Start with TV off
    }

    protected override void Update()
    {
        base.Update();
        
    }

    public void TurnTvOn()
    {
        tvIsOn = true;
        videoPlayer.Play();
    }

    public void TurnTvOff()
    {
        tvIsOn = false;
        videoPlayer.Stop();
    }

    public override void OnInteracted()
    {
        Debug.Log("TV is Interacted!");
        if (tvIsOn == false)
        {
            TurnTvOn();
        }
        else
        {
            TurnTvOff();
        }
    }
}

