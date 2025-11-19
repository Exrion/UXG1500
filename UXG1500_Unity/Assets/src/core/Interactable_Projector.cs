using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor;

class Interactable_Projector : IInteractable
{
    [SerializeField]
    List<string> m_SlidePaths = new();
    TemplateContainer[] m_Slides;
    int m_SlideCount;

    protected override void Start()
    {
        base.Start();
        InitSlides();
        UpdateSlides();
    }

    public override void OnInteracted()
    {
        AdvanceSlides();
        UpdateSlides();
    }

    void AdvanceSlides() => m_SlideCount = m_SlideCount >= m_Slides.Length - 1 ? 0 : m_SlideCount + 1;
    void InitSlides() => m_Slides = new TemplateContainer[m_SlidePaths.Count];

    bool CreateSlide(string path, out TemplateContainer slide)
    {
        slide = null;
        VisualTreeAsset va = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
        if (va != null)
        {
            slide = va.CloneTree();
            return true;
        }
        return false;
    }

    void UpdateSlides()
    {

    }
}