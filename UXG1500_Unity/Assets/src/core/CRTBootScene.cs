using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UIDocument))]
public class TerminalBootController : MonoBehaviour
{
    [Header("Timings (seconds)")]
    public float bootStageDelay = 0.25f;
    public float lineRevealDelay = 0.12f;
    public float charTypingDelay = 0.008f;
    public float finalPromptPulse = 0.6f;

    private readonly string[] bootLines =
    {
        "DMODE1500 COMPUTER BY GROUP 6",
        "DEBUG ROM V0.6 - 281125",
        "001MB AVAILIABLE RAM",
        "==========================================================",
        "CARD: DANIELLE",
        "174952324 BYTES",
        "OFFSETS: F33 R524 D423",
        "MONITOR READY",
        "> profD hello continue.ex9",
        "Hello, Professor Danielle! Ready to proceed?",
        "BREAK AT 1211"
    };

    private VisualElement root;
    private VisualElement overlay;
    private VisualElement linesContainer;
    private Label pressAnyLabel;

    private Font crtFont;
    private bool finished = false;

    // ---------------------------------------------------------
    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        // Load retro CRT font
        crtFont = Resources.Load<Font>("Fonts/VT323-Regular");
        if (crtFont == null)
            Debug.LogWarning("⚠ Font not found at Resources/Fonts/VT323-Regular");

        ApplyCRTFont(root);

        overlay = root.Q<VisualElement>("screenOverlay");
        linesContainer = root.Q<VisualElement>("linesContainer");
        pressAnyLabel = root.Q<Label>("pressAny");

        linesContainer.Clear();

        foreach (var s in bootLines)
        {
            var lbl = new Label();
            lbl.text = s;
            lbl.AddToClassList("crt-line");
            lbl.style.fontSize = 22;   // ⭐ Boot line font size
            linesContainer.Add(lbl);
        }

        pressAnyLabel.style.fontSize = 26; // ⭐ Bigger for prompt

        StartCoroutine(BootSequence());
    }

    // ---------------------------------------------------------
    // Apply font to everything recursively
    // ---------------------------------------------------------
    private void ApplyCRTFont(VisualElement ve)
    {
        if (crtFont != null)
        {
            ve.style.unityFontDefinition = FontDefinition.FromFont(crtFont);
        }

        foreach (VisualElement child in ve.Children())
            ApplyCRTFont(child);
    }

    // ---------------------------------------------------------
    void Update()
    {
        if (finished && Input.anyKeyDown)
        {
            OnBootCompleteAnyKey();
        }
    }

    // ---------------------------------------------------------
    IEnumerator BootSequence()
    {
        overlay.style.opacity = 1f;

        // Screen warming effect
        int lightingFrames = 5;
        for (int i = 0; i < lightingFrames; i++)
        {
            float t = (i + 1f) / lightingFrames;
            float targetOpacity = Mathf.Lerp(1f, 0.2f, t);
            yield return StartCoroutine(FadeOverlayTo(targetOpacity, bootStageDelay));
        }

        yield return new WaitForSeconds(0.18f);

        // Type each boot line
        for (int i = 0; i < linesContainer.childCount; i++)
        {
            Label line = linesContainer.ElementAt(i) as Label;
            if (line == null) continue;

            yield return StartCoroutine(TypeLine(line, bootLines[i]));
            yield return new WaitForSeconds(lineRevealDelay);
        }

        pressAnyLabel.style.opacity = 1f;
        pressAnyLabel.RemoveFromClassList("hidden");

        finished = true;

        StartCoroutine(PulsePrompt(pressAnyLabel));
    }

    // ---------------------------------------------------------
    IEnumerator FadeOverlayTo(float targetOpacity, float duration)
    {
        float start = overlay.resolvedStyle.opacity;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            overlay.style.opacity = Mathf.Lerp(start, targetOpacity, t / duration);
            yield return null;
        }
        overlay.style.opacity = targetOpacity;
    }

    // ---------------------------------------------------------
    IEnumerator TypeLine(Label lineLabel, string fullText)
    {
        lineLabel.style.opacity = 1f;
        lineLabel.text = "";

        yield return null;

        if (fullText.StartsWith("==="))
        {
            lineLabel.text = fullText;
            yield break;
        }

        foreach (char c in fullText)
        {
            lineLabel.text += c;
            yield return new WaitForSeconds(charTypingDelay);
        }
    }

    // ---------------------------------------------------------
    IEnumerator PulsePrompt(Label prompt)
    {
        while (finished)
        {
            float t = (Mathf.Sin(Time.time * (2f / finalPromptPulse)) + 1f) * 0.5f;
            prompt.style.opacity = Mathf.Lerp(0.6f, 1f, t);
            yield return null;
        }
    }

    // ---------------------------------------------------------
    private void OnBootCompleteAnyKey()
    {
        finished = false;
        Debug.Log("Boot sequence finished. Starting next UI...");
        FindObjectOfType<CRTUIManager>().OnBootCompleteAnyKey();
    }
}
