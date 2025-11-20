using System.Collections.Generic;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;

public class S3_Projector_Script : MonoBehaviour
{
    [SerializeField]
    List<VisualTreeAsset> m_SlideVisualTreeAssets = new();
    TemplateContainer[] m_Slides;
    int m_SlideCount = -1;

    UIDocument m_UIDocument;
    VisualElement m_RootVisualElement;
    VisualElement m_RootContainer;

    private void OnEnable()
    {
        m_UIDocument = GetComponent<UIDocument>();
        m_RootVisualElement = m_UIDocument.rootVisualElement;
        m_RootContainer = m_RootVisualElement.Q("Root");

        InitSlides();
        GenerateSlides();
        UpdateSlides();
    }

    void AdvanceSlides() => m_SlideCount = m_SlideCount >= m_Slides.Length - 1 ? -1 : m_SlideCount + 1;
    void InitSlides() => m_Slides = new TemplateContainer[m_SlideVisualTreeAssets.Count > 0 ? m_SlideVisualTreeAssets.Count : 0];

    public void HandleInteracted()
    {
        AdvanceSlides();
        UpdateSlides();
    }

    void GenerateSlides()
    {
        for (int i = 0; i < m_SlideVisualTreeAssets.Count; i++)
        {
            if (CreateSlide(m_SlideVisualTreeAssets[i], out TemplateContainer slide))
                m_Slides[i] = slide;
        }
    }

    bool CreateSlide(VisualTreeAsset asset, out TemplateContainer slide)
    {
        slide = asset.CloneTree();
        if (slide != null)
        {
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
            m_RootContainer.style.display = DisplayStyle.None;
            return;
        }

        m_RootContainer.style.display = DisplayStyle.Flex;
        m_RootContainer.Clear();

        for (int i = 0; i < m_Slides.Length; i++)
        {
            if (m_Slides[i] != null && i == m_SlideCount)
                m_RootContainer.Add(m_Slides[i]);
        }
    }
}
