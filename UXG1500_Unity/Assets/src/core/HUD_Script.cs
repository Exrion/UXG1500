using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class HUD_Script : MonoBehaviour
{
    private void OnEnable()
    {
        UIDocument hud = GetComponent<UIDocument>();
        VisualElement root = hud.rootVisualElement;
    }
}
