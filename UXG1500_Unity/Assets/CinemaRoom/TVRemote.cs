using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class Interactable_TVRemote : IInteractable
{
    public GameObject tvRemote;
    public OpenDoor openDoor;

    public bool tvIsOn = false;
    public bool touchMEbABYONEMORETIME;

    public UnityEvent OpenTheFuckUp;

    public VideoPlayer videoPlayer;
    protected override void Start()
    {
        base.Start();
        openDoor.Init();
        TurnTvOff(); // Start with TV off
    }

    protected override void Update()
    {
        base.Update();
        Debug.Log(videoPlayer.frame.ToString() + ", " + (videoPlayer.frameCount - 1).ToString());
        if (videoPlayer.frameCount - 1 == (ulong)videoPlayer.frame)
        {
            OpenTheFuckUp?.Invoke();
            TurnTvOff();
            Debug.Log("Video Ended");
        }
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

