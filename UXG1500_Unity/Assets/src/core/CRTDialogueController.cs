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

    private bool isWalking = false;
    private bool dialogueActive = false;
    private float characterX = 70f;
    private float walkTargetX = 450f;
    private float walkSpeed = 45f;
    private int dialogueStage = 0;
    private int floppyPage = 1;

    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        dialogueArea = root.Q<VisualElement>("dialogueArea");
        dialogueText = root.Q<Label>("dialogueText");
        option1 = root.Q<Button>("option1");
        option2 = root.Q<Button>("option2");
        character = root.Q<VisualElement>("characterBody");
        hillLarge = root.Q<VisualElement>("hillLarge");
        hillSmall = root.Q<VisualElement>("hillSmall");
        floppyDisk = root.Q<VisualElement>("floppyDisk");

        // Create door dynamically
        door = new VisualElement();
        door.name = "door";
        door.style.position = Position.Absolute;
        door.style.width = 40;
        door.style.height = 70;
        door.style.borderTopWidth = 2;
        door.style.borderLeftWidth = 2;
        door.style.borderRightWidth = 2;
        door.style.borderBottomWidth = 2;
       Color greenCRT = new Color(0f, 1f, 0.4f, 1f);
door.style.borderTopColor = greenCRT;
door.style.borderRightColor = greenCRT;
door.style.borderBottomColor = greenCRT;
door.style.borderLeftColor = greenCRT;

        door.style.display = DisplayStyle.None;
        root.Q<VisualElement>("displayArea")?.Add(door);

        floppyDisk.style.display = DisplayStyle.None;
        dialogueArea.style.display = DisplayStyle.None;

        // Disable hover/mouse interaction globally
        root.pickingMode = PickingMode.Ignore;
    }

    public void StartDialogue()
    {
        if (dialogueActive) return;
        dialogueActive = true;

        dialogueArea.style.display = DisplayStyle.Flex;
        dialogueText.text = "Welcome! You’ve now entered the matrix!";
        option1.text = "> Let me go!";
        option2.text = "> (Let’s move on...)";

        option1.clicked += () => OnChoice(1);
        option2.clicked += () => OnChoice(2);
    }

    private void OnChoice(int choice)
    {
        if (dialogueStage == 0)
        {
            if (choice == 1)
            {
                string existing = dialogueText.text;
                int pCount = existing.Split('P').Length - 1;
                dialogueText.text = $"Nope! :{new string('P', pCount + 1)}";
            }
            else if (choice == 2)
            {
                dialogueText.text = "Walking...";
                DisableOptions();
                isWalking = true;
                dialogueStage = 1;
            }
        }
        else if (dialogueStage == 2)
        {
            if (choice == 1) StartCoroutine(SikeProcessing());
            else if (choice == 2) StartCoroutine(InstantProcessing());
        }
        else if (dialogueStage == 3) // Floppy facts
        {
            if (floppyPage == 1 && choice == 1)
                StartCoroutine(DisplayFloppyPage(2));
            else if (floppyPage == 2)
            {
                if (choice == 1) StartCoroutine(DisplayFloppyPage(3));
                else if (choice == 2) DisplayFloppyPage(1, false);
            }
            else if (floppyPage == 3)
            {
                if (choice == 2) DisplayFloppyPage(2, false);
                else StartCoroutine(SpawnDoorSequence());
            }
        }
        else if (dialogueStage == 4) // Door interaction
        {
            StartCoroutine(WalkThroughDoor(choice));
        }
    }

    void Update()
    {
        if (isWalking)
        {
            float delta = Time.deltaTime * walkSpeed;
            characterX += delta;
            character.style.left = characterX;

            float hillLargeX = Mathf.Lerp(160f, 100f, (characterX - 70f) / (walkTargetX - 70f));
            float hillSmallX = Mathf.Lerp(370f, 200f, (characterX - 70f) / (walkTargetX - 70f));
            hillLarge.style.left = hillLargeX;
            hillSmall.style.left = hillSmallX;

            if (characterX >= walkTargetX && dialogueStage == 1)
            {
                isWalking = false;
                floppyDisk.style.display = DisplayStyle.Flex;
                floppyDisk.style.left = characterX + 30f;
                floppyDisk.style.bottom = 60f;

                dialogueText.text = "Oh! A random floppy disk appeared! What should we do?";
                option1.text = "> Go around it";
                option2.text = "> Proceed with caution";
                EnableOptions();
                dialogueStage = 2;
            }
        }
    }

    private IEnumerator SikeProcessing()
    {
        DisableOptions();
        dialogueText.text = "SIKE";
        yield return new WaitForSeconds(0.4f);
        yield return StartCoroutine(ProcessFloppy());
    }

    private IEnumerator InstantProcessing()
    {
        DisableOptions();
        floppyDisk.style.display = DisplayStyle.Flex;
        floppyDisk.style.left = characterX + 8f;
        floppyDisk.style.bottom = 130f;

        dialogueText.text = "Processing";
        yield return StartCoroutine(ProcessFloppy());
    }

    private IEnumerator ProcessFloppy()
    {
        // Animate “Processing...” for ~6 seconds
        for (int i = 0; i < 6; i++)
        {
            dialogueText.text = "Processing" + new string('.', i % 4);
            yield return new WaitForSeconds(1f);
        }

        yield return StartCoroutine(DisplayFloppyPage(1));
    }

  private IEnumerator DisplayFloppyPage(int page)
{
    floppyPage = page;
    dialogueStage = 3;
    string title = "Data in the Floppy Disk:\n";
    string text = "";

    switch (page)
    {
        case 1:
            text = "Did you know Dark Mode began not as a design style, but a practical design choice? The world’s first computer was made using Cathode-Ray-Technology (CRT).";
            break;
        case 2:
            text = "The same technology used for radars during World-War II. This monochromatic display proved to be sharp, and inexpensive to manufacture at the time.";
            break;
        case 3:
            text = "When colour was introduced into computers, mimicking pen and paper in real life. Creativity and entertainment software capabilities were further enhanced.";
            break;
    }

    // ✅ Hide buttons while typing
    DisableOptions();

    // ✅ Reset text & ensure scaling stays within container
    dialogueText.text = title;
    dialogueText.style.whiteSpace = WhiteSpace.Normal;
    dialogueText.style.overflow = Overflow.Hidden;
    dialogueText.style.maxWidth = Length.Percent(100);
    dialogueText.style.unityTextAlign = TextAnchor.UpperLeft;
    dialogueText.style.unityFontDefinition = FontDefinition.FromFont(Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"));
    dialogueText.style.fontSize = 13; // ✅ Slightly smaller to fit more lines

    yield return new WaitForSeconds(0.2f);

    // ✅ Typewriter effect
    foreach (char c in text)
    {
        dialogueText.text += c;
        yield return new WaitForSeconds(0.02f);
    }

    yield return new WaitForSeconds(0.8f);

    // ✅ Buttons appear only after text finishes typing
    if (page == 1)
    {
        option1.text = "> Continue?";
        option1.style.display = DisplayStyle.Flex;
        option2.style.display = DisplayStyle.None; // ❌ No Proceed with caution
    }
    else if (page == 2 || page == 3)
    {
        option1.text = "> Continue?";
        option2.text = "> Back?";
        option1.style.display = DisplayStyle.Flex;
        option2.style.display = DisplayStyle.Flex;
    }

    EnableOptions();
}

// ✅ Instant version for Back navigation
private void DisplayFloppyPage(int page, bool instant)
{
    floppyPage = page;
    dialogueStage = 3;
    string title = "Data in the Floppy Disk:\n";
    string text = "";

    switch (page)
    {
        case 1:
            text = "Did you know Dark Mode began not as a design style, but a practical design choice? The world’s first computer was made using Cathode-Ray-Technology (CRT).";
            break;
        case 2:
            text = "The same technology used for radars during World-War II. This monochromatic display proved to be sharp, and inexpensive to manufacture at the time.";
            break;
        case 3:
            text = "When colour was introduced into computers, mimicking pen and paper in real life. Creativity and entertainment software capabilities were further enhanced.";
            break;
    }

    // ✅ Prevent overflow
    dialogueText.style.whiteSpace = WhiteSpace.Normal;
    dialogueText.style.overflow = Overflow.Hidden;
    dialogueText.style.maxWidth = Length.Percent(100);
    dialogueText.style.fontSize = 13;

    dialogueText.text = title + text;

    // ✅ Show buttons cleanly
    if (page == 1)
    {
        option1.text = "> Continue?";
        option1.style.display = DisplayStyle.Flex;
        option2.style.display = DisplayStyle.None;
    }
    else
    {
        option1.text = "> Continue?";
        option2.text = "> Back?";
        option1.style.display = DisplayStyle.Flex;
        option2.style.display = DisplayStyle.Flex;
    }

    EnableOptions();
}



    private IEnumerator SpawnDoorSequence()
    {
        dialogueStage = 4;
        dialogueText.text = "Oh!";
        yield return new WaitForSeconds(0.6f);

        dialogueText.text = "Oh! A door appeared.";
        door.style.display = DisplayStyle.Flex;
        door.style.left = characterX + 150f;
        door.style.bottom = 15f;

        yield return new WaitForSeconds(0.6f);
        option1.text = "> Go around it";
        option2.text = "> Proceed with caution";
        EnableOptions();
    }

    private IEnumerator WalkThroughDoor(int choice)
    {
        DisableOptions();
        dialogueText.text = choice == 1 ? "LEMME OUT" : "Walking...";
        yield return new WaitForSeconds(0.5f);

        isWalking = true;
        walkTargetX = characterX + 180f;

        while (characterX < walkTargetX)
        {
            float delta = Time.deltaTime * (walkSpeed * 1.5f);
            characterX += delta;
            character.style.left = characterX;
            yield return null;
        }

        character.style.display = DisplayStyle.None;
    }

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
