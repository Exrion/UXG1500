using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class CRTDialogueController : MonoBehaviour
{
    private VisualElement root;
    private VisualElement dialogueArea;
    private Label dialogueText;
    private Button option1;
    private Button option2;
    private VisualElement character;
    private VisualElement hillLarge;
    private VisualElement hillSmall;
    private VisualElement floppyDisk;
    private VisualElement door;

    private Font crtFont;

    private bool isWalking = false;
    private bool dialogueActive = false;

    private float characterX = 70f;
    private float walkTargetX = 450f;
    private float walkSpeed = 45f;

    private int dialogueStage = 0;
    private int floppyPage = 1;

    // Track playback for pages 1–3
    private bool[] hasPlayedPage = new bool[4];

    // ============================================================
    // ON ENABLE
    // ============================================================
    void OnEnable()
    {
        crtFont = Resources.Load<Font>("Fonts/VT323-Regular");

        root = GetComponent<UIDocument>().rootVisualElement;
        ApplyCRTFont(root);

        dialogueArea = root.Q<VisualElement>("dialogueArea");
        dialogueText = root.Q<Label>("dialogueText");
        option1 = root.Q<Button>("option1");
        option2 = root.Q<Button>("option2");

        character = root.Q<VisualElement>("characterBody");
        hillLarge = root.Q<VisualElement>("hillLarge");
        hillSmall = root.Q<VisualElement>("hillSmall");
        floppyDisk = root.Q<VisualElement>("floppyDisk");

        // Create Door Element
        door = new VisualElement();
        door.name = "door";
        door.style.position = Position.Absolute;
        door.style.width = 40;
        door.style.height = 70;
        door.style.borderTopWidth = 2;
        door.style.borderBottomWidth = 2;
        door.style.borderLeftWidth = 2;
        door.style.borderRightWidth = 2;

        Color greenCRT = new Color(0f, 1f, 0.4f, 1f);
        door.style.borderTopColor = greenCRT;
        door.style.borderBottomColor = greenCRT;
        door.style.borderLeftColor = greenCRT;
        door.style.borderRightColor = greenCRT;

        door.style.display = DisplayStyle.None;
        root.Q<VisualElement>("displayArea")?.Add(door);

        floppyDisk.style.display = DisplayStyle.None;
        dialogueArea.style.display = DisplayStyle.None;
        root.pickingMode = PickingMode.Ignore;
    }

    // ============================================================
    // APPLY CRT FONT TO UI TREE
    // ============================================================
    private void ApplyCRTFont(VisualElement ve)
    {
        if (crtFont != null)
        {
            ve.style.unityFontDefinition = FontDefinition.FromFont(crtFont);
            ve.style.fontSize = 18;
        }

        foreach (var child in ve.Children())
            ApplyCRTFont(child);
    }

    // ============================================================
    // START DIALOGUE
    // ============================================================
    public void StartDialogue()
    {
        if (dialogueActive) return;
        dialogueActive = true;

        dialogueArea.style.display = DisplayStyle.Flex;
        dialogueText.text = "Welcome! You’ve now entered the matrix!";
        dialogueText.style.fontSize = 18;

        option1.text = "> Let me go!";
        option2.text = "> (Let’s move on...)";

        option1.clicked += () => OnChoice(1);
        option2.clicked += () => OnChoice(2);
    }

    // ============================================================
    // CHOICE HANDLER
    // ============================================================
    private void OnChoice(int choice)
    {
        if (dialogueStage == 0)
        {
            if (choice == 1)
            {
                int p = dialogueText.text.Split('P').Length - 1;
                dialogueText.text = $"Nope! :{new string('P', p + 1)}";
            }
            else
            {
                dialogueText.text = "Walking...";
                DisableOptions();
                isWalking = true;
                dialogueStage = 1;
            }
            return;
        }

        if (dialogueStage == 2)
        {
            if (choice == 1) StartCoroutine(SikeProcessing());
            else StartCoroutine(InstantProcessing());
            return;
        }

        if (dialogueStage == 3)
        {
            HandleFloppyChoice(choice);
            return;
        }

        if (dialogueStage == 4)
        {
            StartCoroutine(WalkThroughDoor(choice));
        }
    }

    // ============================================================
    // FLOPPY CHOICE LOGIC
    // ============================================================
    private void HandleFloppyChoice(int choice)
    {
        if (floppyPage == 1)
        {
            StartCoroutine(DisplayFloppyPage(2)); return;
        }

        if (floppyPage == 2)
        {
            if (choice == 1) StartCoroutine(DisplayFloppyPage(3));
            else DisplayFloppyPage(1, true);
            return;
        }

        if (floppyPage == 3)
        {
            if (choice == 2) DisplayFloppyPage(2, true);
            else StartCoroutine(SpawnDoorSequence());
        }
    }

    // ============================================================
    // WALKING
    // ============================================================
    void Update()
    {
        if (!isWalking) return;

        characterX += Time.deltaTime * walkSpeed;
        character.style.left = characterX;

        hillLarge.style.left = Mathf.Lerp(160f, 100f, (characterX - 70f) / (walkTargetX - 70f));
        hillSmall.style.left = Mathf.Lerp(370f, 200f, (characterX - 70f) / (walkTargetX - 70f));

        if (characterX >= walkTargetX && dialogueStage == 1)
        {
            isWalking = false;

            floppyDisk.style.display = DisplayStyle.Flex;
            floppyDisk.style.left = characterX + 30;
            floppyDisk.style.bottom = 60;

            dialogueText.text = "Oh! A random floppy disk appeared! What should we do?";
            option1.text = "> Go around it";
            option2.text = "> Proceed with caution";

            EnableOptions();
            dialogueStage = 2;
        }
    }

    // ============================================================
    // PROCESSING SEQUENCES
    // ============================================================
    private IEnumerator SikeProcessing()
    {
        DisableOptions();
        dialogueText.text = "SIKE";
        yield return new WaitForSeconds(0.4f);

        floppyDisk.style.display = DisplayStyle.Flex;
        floppyDisk.style.left = characterX + 10;
        floppyDisk.style.bottom = 130;

        dialogueText.text = "Processing";
        yield return StartCoroutine(ProcessFloppy());
    }

    private IEnumerator InstantProcessing()
    {
        DisableOptions();

        floppyDisk.style.display = DisplayStyle.Flex;
        floppyDisk.style.left = characterX + 10;
        floppyDisk.style.bottom = 130;

        dialogueText.text = "Processing";
        yield return StartCoroutine(ProcessFloppy());
    }

    private IEnumerator ProcessFloppy()
    {
        for (int i = 0; i < 6; i++)
        {
            dialogueText.text = "Processing" + new string('.', i % 4);
            yield return new WaitForSeconds(1f);
        }

        yield return StartCoroutine(DisplayFloppyPage(1));
    }

    // ============================================================
    // FLOPPY PAGE DISPLAY
    // ============================================================
    private IEnumerator DisplayFloppyPage(int page)
    {
        floppyPage = page;
        dialogueStage = 3;

        string title = "Data in the Floppy Disk:\n";
        string text = GetFloppyText(page);

        DisableOptions();
        dialogueText.text = title;

        // Play animation only first time
        if (!hasPlayedPage[page])
        {
            hasPlayedPage[page] = true;

            foreach (char c in text)
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(0.02f);
            }
        }
        else
        {
            dialogueText.text = title + text;
        }

        ShowFloppyButtons(page);
    }

    private void DisplayFloppyPage(int page, bool instant)
    {
        floppyPage = page;
        dialogueStage = 3;

        string title = "Data in the Floppy Disk:\n";
        string text = GetFloppyText(page);

        dialogueText.text = title + text;
        ShowFloppyButtons(page);
    }

    private string GetFloppyText(int page)
    {
        return page switch
        {
            1 => "Did you know Dark Mode began not as a design style, but a practical design choice? The world’s first computer was made using Cathode-Ray-Technology (CRT).",
            2 => "The same technology used for radars during World-War II. This monochromatic display proved to be sharp, and inexpensive to manufacture at the time.",
            3 => "When colour was introduced into computers, mimicking pen and paper in real life. Creativity and entertainment software capabilities were further enhanced.",
            _ => ""
        };
    }

    private void ShowFloppyButtons(int page)
    {
        option1.style.fontSize = 20;
        option2.style.fontSize = 20;

        if (page == 1)
        {
            option1.text = "> Continue?";
            option1.style.display = DisplayStyle.Flex;
            option2.style.display = DisplayStyle.None;
            return;
        }

        option1.text = "> Continue?";
        option2.text = "> Back?";
        option1.style.display = DisplayStyle.Flex;
        option2.style.display = DisplayStyle.Flex;
    }

    // ============================================================
    // DOOR SEQUENCE (FIXED!)
    // ============================================================
    private IEnumerator SpawnDoorSequence()
    {
        dialogueStage = 4;

        // ⭐ Hide box using USS class (layout stays intact)
        dialogueArea.AddToClassList("dialogueAreaHidden");

        DisableOptions();

        // Show "Oh!" as raw text
        dialogueText.text = "Oh!";
        dialogueText.style.fontSize = 18;

        yield return new WaitForSeconds(0.6f);

        // ⭐ Restore original dialogue box style
        dialogueArea.RemoveFromClassList("dialogueAreaHidden");

        // Now show actual door line
        dialogueText.text = "Oh! A door appeared.";

        // Show door
        door.style.display = DisplayStyle.Flex;
        door.style.left = characterX + 150f;
        door.style.bottom = 15f;

        yield return new WaitForSeconds(0.6f);

        // Show options
        option1.text = "> Go around it";
        option2.text = "> Proceed with caution";

        option1.style.fontSize = 20;
        option2.style.fontSize = 20;

        EnableOptions();
    }


    // ============================================================
    // WALK THROUGH DOOR
    // ============================================================
    private IEnumerator WalkThroughDoor(int choice)
    {
        DisableOptions();

        dialogueText.text = choice == 1 ? "Let me out!" : "Walking...";
        yield return new WaitForSeconds(0.5f);

        isWalking = true;
        walkTargetX = characterX + 180f;

        while (characterX < walkTargetX)
        {
            characterX += Time.deltaTime * (walkSpeed * 1.5f);
            character.style.left = characterX;
            yield return null;
        }

        // Character disappears
        character.style.display = DisplayStyle.None;

        // ⭐ NEW: show goodbye message
        dialogueText.text = "Goodbye!";
        dialogueArea.style.display = DisplayStyle.Flex;

        yield return new WaitForSeconds(1.2f);

        // ⭐ NEW: transition out
        LoadNextScene();
    }


    // ============================================================
    // TO LOAD DA THE SCENE
    // ============================================================
    private void LoadNextScene()
    {
        // TODO: Replace this with your actual next scene
        // Example:
        // SceneManager.LoadScene("CRTGameScene_2");

        Debug.Log("Next scene loading triggered!");

        // If you want to fade out, you can put animation here
    }


    // ============================================================
    // BUTTON CONTROL
    // ============================================================
    private void DisableOptions()
    {
        option1.style.display = DisplayStyle.None;
        option2.style.display = DisplayStyle.None;
    }

    private void EnableOptions()
    {
        option1.style.display = DisplayStyle.Flex;
        option2.style.display = DisplayStyle.Flex;
    }
}
