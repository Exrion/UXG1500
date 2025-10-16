using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DebugGUI : Singleton<DebugGUI>
{
    private static float ConvertRelativeWidth(float pc) => Screen.width * pc;
    public static Func<float, float> RelW => ConvertRelativeWidth;
    private static float ConvertRelativeHeight(float pc) => Screen.height * pc;
    public static Func<float, float> RelH => ConvertRelativeHeight;

    private void DrawDebug()
    {
        GUI.Box(new Rect(10, 10, 100, 150), "Debug Menu");

        for (int i = 0; i < GameManager.Instance.GetSceneList().Count; i++)
        {
            if (GUI.Button(new Rect(20, 40 + i * 30, 80, 20), (i + 1).ToString()))
            {
                GameManager.Instance.PrepareScene(i);
                GameManager.Instance.ArmSceneSwitch();
            }
        }
    }

    private void OnGUI()
    {
#if UNITY_EDITOR
        DrawDebug();
#endif
    }
}
