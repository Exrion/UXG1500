using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CRTGameSceneController : MonoBehaviour
{
    private ScrollView scrollView;
    private VisualElement itemContainer;
    private Label detailsText;
    private int selectedIndex = 0;
    private readonly List<Label> itemLabels = new();
    private CRTDialogueController dialogueController;

    private bool inputEnabled = true;

    private readonly List<(string name, string stats)> characters = new()
    {
        ("caifan_enjoyer", "205/250 HP\n150 ATK\n50 SPD"),
        ("raptorz_z", "205/250 HP\n150 ATK\n50 SPD"),
        ("ghostijay", "??? HP\n??? ATK\n??? SPD"),
        ("clodies", "230/230 HP\n170 ATK\n40 SPD"),
        ("Merodiii", "190/200 HP\n120 ATK\n80 SPD")
    };

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        scrollView = root.Q<ScrollView>("characterScroll");
        itemContainer = root.Q<VisualElement>("characterItems");
        detailsText = root.Q<Label>("characterDetails");
        dialogueController = GetComponent<CRTDialogueController>();

        BuildList();
        UpdateSelection(0);
    }

    private void BuildList()
    {
        itemContainer.Clear();
        itemLabels.Clear();

        for (int i = 0; i < characters.Count; i++)
        {
            var label = new Label($"{i + 1}. {characters[i].name}");
            label.AddToClassList("character-item");
            itemContainer.Add(label);
            itemLabels.Add(label);
        }
    }

    void Update()
    {
        // Stop processing movement input if disabled
        if (!inputEnabled || itemLabels.Count == 0) return;

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex = Mathf.Min(selectedIndex + 1, itemLabels.Count - 1);
            UpdateSelection(selectedIndex);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex = Mathf.Max(selectedIndex - 1, 0);
            UpdateSelection(selectedIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log($"Confirmed {characters[selectedIndex].name}");

            // Don’t clear stats — keep them visible
            if (dialogueController != null)
            {
                dialogueController.StartDialogue();
            }

            // Only freeze Up/Down inputs, not dialogue clicks
            inputEnabled = false;
        }
    }

    private void UpdateSelection(int index)
    {
        for (int i = 0; i < itemLabels.Count; i++)
            itemLabels[i].EnableInClassList("selected", i == index);

        detailsText.text = $"{characters[index].name}\n\n{characters[index].stats}";
        scrollView.ScrollTo(itemLabels[index]);
    }

    public void EnableCharacterSelection()
    {
        inputEnabled = true;
    }

    public void DisableCharacterSelection()
    {
        inputEnabled = false;
    }
}
