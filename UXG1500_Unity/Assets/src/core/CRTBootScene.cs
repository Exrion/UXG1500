using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement; // optional if you want to switch scenes

[RequireComponent(typeof(UIDocument))]
public class TerminalBootController : MonoBehaviour
{
    // Configurable timings
    [Header("Timings (seconds)")]
    public float bootStageDelay = 0.25f;        // time between the 5 "frames" of lighting
    public float lineRevealDelay = 0.12f;      // delay between revealing each line
    public float charTypingDelay = 0.008f;     // per character typing delay (for typing effect)
    public float finalPromptPulse = 0.6f;

    // Lines to display (in the order you provided)
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

    // UI references
    private VisualElement root;
    private VisualElement overlay;
    private VisualElement linesContainer;
    private Label pressAnyLabel;

    // Internal state
    private bool finished = false;

    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        overlay = root.Q<VisualElement>("screenOverlay");
        linesContainer = root.Q<VisualElement>("linesContainer");
        pressAnyLabel = root.Q<Label>("pressAny");

        // make sure container is empty
        linesContainer.Clear();

        // create labels (hidden initially)
        foreach (var s in bootLines)
        {
            var lbl = new Label { text = s };
            lbl.AddToClassList("crt-line");
            linesContainer.Add(lbl);
        }

        // Start the boot sequence
        StartCoroutine(BootSequence());
    }

    void Update()
    {
        if (finished && Input.anyKeyDown)
        {
            OnBootCompleteAnyKey();
        }
    }

    IEnumerator BootSequence()
    {
        // Step 0: ensure overlay is fully black
        overlay.style.opacity = 1f;

        // 5 "frames" to light up the screen (you can tweak bootStageDelay)
        int lightingFrames = 5;
        for (int i = 0; i < lightingFrames; i++)
        {
            float t = (i + 1f) / lightingFrames; // 0->1
            // map t to overlay opacity (1 -> 0.2)
            float targetOpacity = Mathf.Lerp(1f, 0.2f, t);
            yield return StartCoroutine(FadeOverlayTo(targetOpacity, bootStageDelay));
        }

        // small pause
        yield return new WaitForSeconds(0.18f);

        // reveal lines one by one with typing effect
        for (int i = 0; i < linesContainer.childCount; i++)
        {
            var ve = linesContainer.ElementAt(i) as Label;
            if (ve == null) continue;

            // typing effect for that line
            yield return StartCoroutine(TypeLine(ve, bootLines[i]));
            // short pause between lines
            yield return new WaitForSeconds(lineRevealDelay);
        }

        // show the prompt (the line "<press any key...>" is already shown as a full line by above typing,
        // but we also make the prompt label visible and pulse it for emphasis)
        pressAnyLabel.style.opacity = 1f;
        pressAnyLabel.RemoveFromClassList("hidden");

        // pulse prompt (simple loop)
        finished = true;
        StartCoroutine(PulsePrompt(pressAnyLabel));
    }

    IEnumerator FadeOverlayTo(float targetOpacity, float duration)
    {
        float start = overlay.resolvedStyle.opacity;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float val = Mathf.Lerp(start, targetOpacity, t / duration);
            overlay.style.opacity = val;
            yield return null;
        }
        overlay.style.opacity = targetOpacity;
    }

    IEnumerator TypeLine(Label lineLabel, string fullText)
    {
        lineLabel.style.opacity = 1f;
        lineLabel.text = string.Empty;

        // Very small delay if you want a per-line short pause before typing
        yield return null;

        // For the special case of the separator row, reveal at once
        if (fullText.StartsWith("==="))
        {
            lineLabel.text = fullText;
            yield break;
        }

        for (int i = 0; i < fullText.Length; i++)
        {
            lineLabel.text += fullText[i];
            yield return new WaitForSeconds(charTypingDelay);
        }
    }

    IEnumerator PulsePrompt(Label prompt)
    {
        float elapsed = 0f;
        while (finished)
        {
            // pulse by toggling opacity slowly
            float t = (Mathf.Sin(Time.time * (2f / finalPromptPulse)) + 1f) * 0.5f; // 0..1
            prompt.style.opacity = Mathf.Lerp(0.6f, 1f, t);
            yield return null;
        }
    }

    // This function is called when the user presses a key after the boot sequence completes
    private void OnBootCompleteAnyKey()
    {
        // stop the prompt pulse
        finished = false;

        Debug.Log("Boot sequence complete. Loading next scene...");
        FindObjectOfType<CRTUIManager>().OnBootCompleteAnyKey();

    }
}
