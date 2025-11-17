using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class S3Laptopetails_Script : MonoBehaviour
{
    enum PAGE
    {
        MAIN,
        READ,
        COMFORT,
        COLOURS
    }

    public UIDocument m_PreviousUIDDocument;
    public IInteractable m_Interactable;
    public FirstPersonController m_PlayerController;

    UIDocument m_UIDocument;
    VisualElement m_RootVisualElement;

    VisualElement m_LightDarkToggle;
    bool m_DarkMode = true;

    VisualElement m_ButtonQuit;
    VisualElement m_ButtonBack;

    public float m_OpacityDelay;
    bool m_OpacityChanged;

    List<VisualElement> m_Text = new();
    List<VisualElement> m_Fill = new();
    List<VisualElement> m_ContentMain = new();
    List<VisualElement> m_ContentRead = new();
    List<VisualElement> m_ContentComfort = new();
    List<VisualElement> m_ContentColours = new();
    List<VisualElement> m_ContentAll = new();

    PAGE currentPage = PAGE.MAIN;
    PAGE previousPage;

    void Start()
    {
        m_UIDocument = GetComponent<UIDocument>();
        m_RootVisualElement = m_UIDocument.rootVisualElement;
        m_RootVisualElement.style.display = DisplayStyle.None;

        m_LightDarkToggle = m_RootVisualElement.Q("LightDarkToggleParent");
        m_LightDarkToggle.RegisterCallback<ClickEvent>(OnToggleLightDark);

        m_ButtonQuit = m_RootVisualElement.Q("Quit");
        m_ButtonQuit.RegisterCallback<ClickEvent>(OnQuit);

        m_ButtonBack = m_RootVisualElement.Q("Back");
        m_ButtonBack.RegisterCallback<ClickEvent, PAGE>(OnPageChange, PAGE.MAIN);

        m_RootVisualElement.Q("Readability").RegisterCallback<ClickEvent, PAGE>(OnPageChange, PAGE.READ);
        m_RootVisualElement.Q("Comfort").RegisterCallback<ClickEvent, PAGE>(OnPageChange, PAGE.COMFORT);
        m_RootVisualElement.Q("Colours").RegisterCallback<ClickEvent, PAGE>(OnPageChange, PAGE.COLOURS);

        previousPage = currentPage;
        GenerateContent();
        GetContent();
        SetContent(m_ContentAll, DisplayStyle.None);
        SetContent(m_ContentMain, DisplayStyle.Flex);

        GetText();
        GetFill();
    }

    void Update()
    {
        if (m_RootVisualElement.style.display == DisplayStyle.Flex)
            OnEnter();

        if (!m_OpacityChanged) 
            StartCoroutine(EnableOpacity());

        if (currentPage != previousPage)
        {
            previousPage = currentPage;
            switch (currentPage)
            {
                case PAGE.MAIN:
                    SetContent(m_ContentAll, DisplayStyle.None);
                    SetContent(m_ContentMain, DisplayStyle.Flex);
                    break;
                case PAGE.READ:
                    SetContent(m_ContentAll, DisplayStyle.None);
                    SetContent(m_ContentRead, DisplayStyle.Flex);
                    break;
                case PAGE.COMFORT:
                    SetContent(m_ContentAll, DisplayStyle.None);
                    SetContent(m_ContentComfort, DisplayStyle.Flex);
                    break;
                case PAGE.COLOURS:
                    SetContent(m_ContentAll, DisplayStyle.None);
                    SetContent(m_ContentColours, DisplayStyle.Flex);
                    break;
            }
        }
    }

    public void SwitchDocuments()
    {
        m_PreviousUIDDocument.rootVisualElement.style.display = DisplayStyle.None;
        m_RootVisualElement.style.display = DisplayStyle.Flex;
        OnEnter();
    }

    void GenerateContent()
    {
        VisualElement content = m_RootVisualElement.Q("Content");
        VisualElement element;

        // Readability
        element = new Label("Readability");
        element.AddToClassList("Title");
        element.AddToClassList("TextDarkMode");
        element.AddToClassList("Identifier_Text");
        element.AddToClassList("ParagraphSpacing");
        element.AddToClassList("Identifier_TextContent");
        element.AddToClassList("Identifier_TextReadability");
        content.Add(element);

        element = new Label("In Dark Mode, content needs to be readable. Back before the introduction of modern Dark Mode, the emulation of paper was one of the inspirations behind light mode.");
        element.AddToClassList("Label");
        element.AddToClassList("TextDarkMode");
        element.AddToClassList("Identifier_Text");
        element.AddToClassList("ParagraphSpacing");
        element.AddToClassList("Identifier_TextContent");
        element.AddToClassList("Identifier_TextReadability");
        content.Add(element);

        element = new Label("Ink on paper has been readable for the longest time. But how has Dark Mode adapted? The answer is contrast.");
        element.AddToClassList("Label");
        element.AddToClassList("TextDarkMode");
        element.AddToClassList("Identifier_Text");
        element.AddToClassList("ParagraphSpacing");
        element.AddToClassList("Identifier_TextContent");
        element.AddToClassList("Identifier_TextReadability");
        content.Add(element);

        element = new Label("Stark differences in light and dark colours lead to visual fatigue and visual artifacts such as halation. Especially for users with astigmatism.");
        element.AddToClassList("Label");
        element.AddToClassList("TextDarkMode");
        element.AddToClassList("Identifier_Text");
        element.AddToClassList("ParagraphSpacing");
        element.AddToClassList("Identifier_TextContent");
        element.AddToClassList("Identifier_TextReadability");
        content.Add(element);

        element = new Label("On top of contrast and colour, font choice and text options like letter spacing and font size matters when creating a comfortable reading experience in Dark Mode.");
        element.AddToClassList("Label");
        element.AddToClassList("TextDarkMode");
        element.AddToClassList("Identifier_Text");
        element.AddToClassList("ParagraphSpacing");
        element.AddToClassList("Identifier_TextContent");
        element.AddToClassList("Identifier_TextReadability");
        content.Add(element);

        // Comfort
        element = new Label("Comfort");
        element.AddToClassList("Title");
        element.AddToClassList("TextDarkMode");
        element.AddToClassList("Identifier_Text");
        element.AddToClassList("ParagraphSpacing");
        element.AddToClassList("Identifier_TextContent");
        element.AddToClassList("Identifier_TextComfort");
        content.Add(element);

        element = new Label("Similar to readability, comfort shares many of the same aspects.");
        element.AddToClassList("Label");
        element.AddToClassList("TextDarkMode");
        element.AddToClassList("Identifier_Text");
        element.AddToClassList("ParagraphSpacing");
        element.AddToClassList("Identifier_TextContent");
        element.AddToClassList("Identifier_TextComfort");
        content.Add(element);

        element = new Label("In a study conducted in 2025 (Sengsoon & Intaruk, 2025), participants were found to suffer from visual fatigue by a greater factor when reading in Light Mode than Dark Mode.");
        element.AddToClassList("Label");
        element.AddToClassList("TextDarkMode");
        element.AddToClassList("Identifier_Text");
        element.AddToClassList("ParagraphSpacing");
        element.AddToClassList("Identifier_TextContent");
        element.AddToClassList("Identifier_TextComfort");
        content.Add(element);

        element = new Label("This however, came with caveats. Ambient lighting conditions still played a crucial role in determining visual fatigue, dry eyes and other factors.");
        element.AddToClassList("Label");
        element.AddToClassList("TextDarkMode");
        element.AddToClassList("Identifier_Text");
        element.AddToClassList("ParagraphSpacing");
        element.AddToClassList("Identifier_TextContent");
        element.AddToClassList("Identifier_TextComfort");
        content.Add(element);

        element = new Label("Dark Mode doesn't simply solve visual fatigue simply by adhering to the rules and principles discussed here. The user has a part to play as well by ensuring they use their devices in places with suffecient ambient lighting.");
        element.AddToClassList("Label");
        element.AddToClassList("TextDarkMode");
        element.AddToClassList("Identifier_Text");
        element.AddToClassList("ParagraphSpacing");
        element.AddToClassList("Identifier_TextContent");
        element.AddToClassList("Identifier_TextComfort");
        content.Add(element);

        // Colours
        element = new Label("Colours");
        element.AddToClassList("Title");
        element.AddToClassList("TextDarkMode");
        element.AddToClassList("Identifier_Text");
        element.AddToClassList("ParagraphSpacing");
        element.AddToClassList("Identifier_TextContent");
        element.AddToClassList("Identifier_TextColours");
        content.Add(element);

        element = new Label("Dark Modes' most defining feature by far has been its colour scheme.");
        element.AddToClassList("Label");
        element.AddToClassList("TextDarkMode");
        element.AddToClassList("Identifier_Text");
        element.AddToClassList("ParagraphSpacing");
        element.AddToClassList("Identifier_TextContent");
        element.AddToClassList("Identifier_TextColours");
        content.Add(element);

        element = new Label("With a dark background and light coloured text, it has been on the rise since its initial introduction to the world.");
        element.AddToClassList("Label");
        element.AddToClassList("TextDarkMode");
        element.AddToClassList("Identifier_Text");
        element.AddToClassList("ParagraphSpacing");
        element.AddToClassList("Identifier_TextContent");
        element.AddToClassList("Identifier_TextColours");
        content.Add(element);

        element = new Label("Though black and white aren't the only colours we need to concern ourselves with.");
        element.AddToClassList("Label");
        element.AddToClassList("TextDarkMode");
        element.AddToClassList("Identifier_Text");
        element.AddToClassList("ParagraphSpacing");
        element.AddToClassList("Identifier_TextContent");
        element.AddToClassList("Identifier_TextColours");
        content.Add(element);

        element = new Label("In order to accomodate for users with colour blindness, we must also consider colours that are visually distinct from each other when viewed by users with varying colour defeciencies.");
        element.AddToClassList("Label");
        element.AddToClassList("TextDarkMode");
        element.AddToClassList("Identifier_Text");
        element.AddToClassList("ParagraphSpacing");
        element.AddToClassList("Identifier_TextContent");
        element.AddToClassList("Identifier_TextColours");
        content.Add(element);

        element = new Label("The importance of additional colours in Dark Mode is amplified when designing applications.");
        element.AddToClassList("Label");
        element.AddToClassList("TextDarkMode");
        element.AddToClassList("Identifier_Text");
        element.AddToClassList("ParagraphSpacing");
        element.AddToClassList("Identifier_TextContent");
        element.AddToClassList("Identifier_TextColours");
        content.Add(element);

        element = new Label("Elements like call to actions, colour-coded menus and reactive elements like hover effects need to be designed with accessibility in mind to create an effective Dark Mode experience.");
        element.AddToClassList("Label");
        element.AddToClassList("TextDarkMode");
        element.AddToClassList("Identifier_Text");
        element.AddToClassList("ParagraphSpacing");
        element.AddToClassList("Identifier_TextContent");
        element.AddToClassList("Identifier_TextColours");
        content.Add(element);
    }

    void SetContent(List<VisualElement> content, DisplayStyle displayStyle)
    {
        for (int i = 0; i < content.Count; i++)
            content[i].style.display = displayStyle;
    }

    void GetContent()
    {
        m_ContentMain = m_RootVisualElement.Query(className: "Identifier_TextMain").ToList();
        m_ContentRead = m_RootVisualElement.Query(className: "Identifier_TextReadability").ToList();
        m_ContentComfort = m_RootVisualElement.Query(className: "Identifier_TextComfort").ToList();
        m_ContentColours = m_RootVisualElement.Query(className: "Identifier_TextColours").ToList();
        m_ContentAll = m_RootVisualElement.Query(className: "Identifier_TextContent").ToList();
    }

    void GetText()
    {
        m_Text = m_RootVisualElement.Query(className: "Identifier_Text").ToList();
    }

    void GetFill()
    {
        m_Fill = m_RootVisualElement.Query(className: "Identifier_Fill").ToList();
    }

    void OnPageChange(ClickEvent evt, PAGE page)
    {
        currentPage = page;
    }

    void OnEnter()
    {
        m_PlayerController.lockCursor = false;
    }

    void OnQuit(ClickEvent evt)
    {
        m_PlayerController.lockCursor = true;
        m_RootVisualElement.style.display = DisplayStyle.None;

        if (m_Interactable)
            ((Interactable_UITKDocument)m_Interactable).OnQuitUI(m_UIDocument);
    }

    void OnToggleLightDark(ClickEvent evt)
    {
        m_DarkMode = !m_DarkMode;
        switch (m_DarkMode)
        {
            case true:
                // Toggle
                m_LightDarkToggle.Q("SwitchBase").RemoveFromClassList("ModeToggle-Light");
                m_LightDarkToggle.Q("SliderBase").RemoveFromClassList("ModeToggleSliderBase-Light");
                m_LightDarkToggle.Q("Slider").RemoveFromClassList("ModeToggleSlider-Light");

                // Buttons
                m_ButtonQuit.Q("QuitIcon").RemoveFromClassList("ButtonQuit-Light");
                m_ButtonBack.Q("BackIcon").RemoveFromClassList("ButtonBack-Light");

                // Bg
                m_RootVisualElement.Q("Background").RemoveFromClassList("BackgroundOffWhiteGlow");

                // Text
                for (int i = 0; i < m_Text.Count; i++)
                    m_Text[i].RemoveFromClassList("TextLightMode");

                // Fill
                for (int i = 0; i < m_Fill.Count; i++)
                    m_Fill[i].RemoveFromClassList("FillLightMode");

                break;
            case false:
                // Toggle
                m_LightDarkToggle.Q("SwitchBase").AddToClassList("ModeToggle-Light");
                m_LightDarkToggle.Q("SliderBase").AddToClassList("ModeToggleSliderBase-Light");
                m_LightDarkToggle.Q("Slider").AddToClassList("ModeToggleSlider-Light");

                // Buttons
                m_ButtonQuit.Q("QuitIcon").AddToClassList("ButtonQuit-Light");
                m_ButtonBack.Q("BackIcon").AddToClassList("ButtonBack-Light");

                // Bg
                m_RootVisualElement.Q("Background").AddToClassList("BackgroundOffWhiteGlow");

                // Text
                for (int i = 0; i < m_Text.Count; i++)
                    m_Text[i].AddToClassList("TextLightMode");

                // Fill
                for (int i = 0; i < m_Fill.Count; i++)
                    m_Fill[i].AddToClassList("FillLightMode");

                break;
        }
    }

    IEnumerator EnableOpacity()
    {
        yield return new WaitForSeconds(m_OpacityDelay);
        m_RootVisualElement.Q("Container").AddToClassList("OpacityFull");
        m_OpacityChanged = true;
    }
}
