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
        //m_RootVisualElement.style.display = DisplayStyle.None;

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

        element = new Label("Stark differences in light and dark colours lead to visual fatigue and visual artifacts such as halation.");
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

        // Colours
        element = new Label("Colours");
        element.AddToClassList("Title");
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

    void OnQuit(ClickEvent evt)
    {
        if (m_Interactable)
            ((Interactable_UITKDocument)m_Interactable).OnQuitUI();
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
