using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor;

class Interactable_Projector : IInteractable
{
    [SerializeField]
    List<string> m_SlidePaths = new();
    TemplateContainer[] m_Slides;
    int m_SlideCount = -1;

    public UIDocument m_UIDocument;
    VisualElement m_RootVisualElement;

    protected override void Start()
    {
        base.Start();

        m_RootVisualElement = m_UIDocument.rootVisualElement;

        InitSlides();
        GenerateSlides();
        UpdateSlides();
    }

    public override void OnInteracted()
    {
        AdvanceSlides();
        Debug.Log(m_SlideCount);
        UpdateSlides();
    }

    void AdvanceSlides() => m_SlideCount = m_SlideCount >= m_Slides.Length - 1 ? -1 : m_SlideCount + 1;
    void InitSlides() => m_Slides = new TemplateContainer[m_SlidePaths.Count > 0 ? m_SlidePaths.Count : 0];

    void GenerateSlides()
    {
        for (int i = 0; i < m_SlidePaths.Count; i++)
        {
            if (CreateSlide(m_SlidePaths[i], out TemplateContainer slide))
                m_Slides[i] = slide;
        }
    }

    bool CreateSlide(string path, out TemplateContainer slide)
    {
        slide = null;
        VisualTreeAsset va = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
        if (va != null)
        {
            slide = va.CloneTree();
            slide.style.width = Length.Percent(100);
            slide.style.height = Length.Percent(100);
            return true;
        }
        return false;
    }

    void UpdateSlides()
    {
        if (m_SlideCount == -1)
        {
            m_RootVisualElement.style.display = DisplayStyle.None;
            return;
        }

        m_RootVisualElement.style.display = DisplayStyle.Flex;

        for (int i = 0; i < m_Slides.Length; i++)
        {
            if (m_Slides[i] != null && i == m_SlideCount)
                m_RootVisualElement.Add(m_Slides[i]);
        }
    }
}