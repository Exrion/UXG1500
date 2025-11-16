using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class S3Laptop_Script : MonoBehaviour
{
    int stage = 1;
    int stageIncrement = -1;
    bool incrementFlip;

    InputAction incrementAction;

    UIDocument m_UIDocument;
    VisualElement m_RootVisualElement;
    VisualElement m_RootContainer;

    private void Start()
    {
        incrementAction = InputSystem.actions.FindAction("UITK_Down");
        incrementAction.performed += HandlePerformed;

        m_UIDocument = GetComponent<UIDocument>();
        m_RootVisualElement = m_UIDocument.rootVisualElement;
        m_RootContainer = m_RootVisualElement.Q("Container");
    }

    private void Update()
    {
        switch (stage)
        {
            case 1:
                Stage1();
                break;
            case 2:
                Stage2();
                break;
        }
    }

    private void OnDisable()
    {
        incrementAction.performed -= HandlePerformed;
    }

    private void Stage2()
    {
        int incrementMax = 6;

        if (stageIncrement == 0 && incrementFlip)
        {
            var label = new Label("Contrast");
            label.name = "1";
            label.AddToClassList("Title1_Dark");
            label.AddToClassList("AlphaWhite255");
            m_RootContainer.Add(label);

            incrementFlip = false;
        }
        else if (stageIncrement == 1)
        {
            if (incrementFlip)
            {
                var bg = m_RootVisualElement.Q("Background");
                var l1 = m_RootContainer.Q("1");
                bg.AddToClassList("BackgroundSlate");
                l1.AddToClassList("AlphaOffWhite255");
            }
            incrementFlip = false;
        }
        else if (stageIncrement == 2)
        {
            if (incrementFlip)
            {
                var label = new Label("That's better, isn't it?");
                label.name = "2";
                label.AddToClassList("Title2_Light");
                label.AddToClassList("AlphaOffWhite255");
                m_RootContainer.Add(label);
            }
            incrementFlip = false;
        }

        if (stageIncrement >= incrementMax)
        {
            stageIncrement = -1;
            stage++;
            m_RootContainer.Clear();
        }
    }

    private void Stage1()
    {
        int incrementMax = 6;

        if (stageIncrement == 0)
        {
            if (incrementFlip)
            {
                var label = new Label("Bright isn't it?");
                label.name = "1";
                label.AddToClassList("Title1_Light");
                label.AddToClassList("AlphaDark255");
                m_RootContainer.Add(label);
            }
            incrementFlip = false;
        }
        else if (stageIncrement == 1)
        {
            if (incrementFlip)
            {
                var label = new Label("Let's change that.");
                label.name = "2";
                label.AddToClassList("Title2_Light");
                label.AddToClassList("AlphaDark255");
                m_RootContainer.Add(label);
            }
            incrementFlip = false;
        }
        else if (stageIncrement == 2 && incrementFlip)
        {
            var bg = m_RootVisualElement.Q("Background");
            var instrAr = m_RootVisualElement.Q("Arrows");
            var instrText = m_RootVisualElement.Q("InstructionText");
            m_RootContainer.Remove(m_RootVisualElement.Q("1"));
            m_RootContainer.Remove(m_RootVisualElement.Q("2"));
            bg.AddToClassList("BackgroundBlack");
            instrAr.AddToClassList("AlphaOffWhite255");
            instrText.AddToClassList("AlphaOffWhite255");
            incrementFlip = false;
        }
        else if (stageIncrement == 3)
        {
            if (incrementFlip)
            {
                var label = new Label("That's better.");
                label.name = "1";
                label.AddToClassList("Title1_Dark");
                label.AddToClassList("AlphaOffWhite255");
                m_RootContainer.Add(label);
            }
            incrementFlip = false;
        }
        else if (stageIncrement == 4)
        {
            if (incrementFlip)
            {
                var label = new Label("Dark Mode is about comfort.");
                label.name = "2";
                label.AddToClassList("Title2_Dark");
                label.AddToClassList("AlphaOffWhite255");
                m_RootContainer.Add(label);
            }
            incrementFlip = false;
        }
        else if (stageIncrement == 5)
        {
            if (incrementFlip)
            {
                var label = new Label("But, these colours are far too contrasting.");
                label.name = "3";
                label.AddToClassList("Title2_Dark");
                label.AddToClassList("AlphaOffWhite255");
                m_RootContainer.Add(label);
            }
            incrementFlip = false;
        }

        if (stageIncrement >= incrementMax)
        {
            stageIncrement = -1;
            incrementFlip = true;
            stage++;
            m_RootContainer.Clear();
        }
    }

    private void HandlePerformed(InputAction.CallbackContext ctx)
    {
        stageIncrement++;
        Debug.Log(stageIncrement);
        incrementFlip = true;
    }
}
