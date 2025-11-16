using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
public class VisualElementStep
{
//    public enum ANIMATION_TYPE
//    {
//        INSTANT = 0x00,
//        FADE = 0x01,
//        PAN = 0x02
//    }

//    public VisualElementStep(int stepNumber, VisualElement stepElement, ANIMATION_TYPE stepTransition)
//    {
//        this.stepNumber = stepNumber;
//        this.stepElement = stepElement;
//        this.stepTransition = stepTransition;
//    }

//    public int stepNumber;
//    public VisualElement stepElement;
//    public ANIMATION_TYPE stepTransition;
//}

//public class VisualScene
//{

//    public VisualScene(VisualElement rootVisualElement, int sceneNumber)
//    {
//        this.rootVisualElement = rootVisualElement;
//        this.sceneNumber = sceneNumber;
//    }

//    public VisualElement rootVisualElement;
//    public List<VisualElementStep> steps = new();
//    public int currentStep;
//    public int previousStep;

//    public int sceneNumber;
//}

//public class UIToolkitHelper : MonoBehaviour
//{
//    private UIDocument m_UIDocument;
//    protected VisualElement m_RootVisualElement;
//    protected VisualElement m_RootContainer;

//    protected List<VisualScene> m_SceneList = new();
//    protected int m_CurrentSceneStep;
//    protected int m_PreviousSceneStep;

//    public List<VisualTreeAsset> m_ScenesToBeRegistered = new();

//    public string m_PreviousActionName;
//    public string m_NextActionName;

//    protected InputAction m_PreviousAction;
//    protected InputAction m_NextAction;

//    public string m_ContainerName = "Container";

//    protected virtual void Start()
//    {
//        m_UIDocument = GetComponent<UIDocument>();
//        m_RootVisualElement = m_UIDocument.rootVisualElement;
//        m_RootContainer = m_RootVisualElement.Q(m_ContainerName);

//        m_PreviousAction = InputSystem.actions.FindAction(m_PreviousActionName);
//        m_NextAction = InputSystem.actions.FindAction(m_NextActionName);

//        if (m_PreviousAction != null)
//            m_PreviousAction.performed += HandlePreviousActionPerformed;
//        if (m_NextAction != null)
//            m_NextAction.performed += HandleNextActionPerformed;

//        RegisterSceneSteps();
//    }

//    protected virtual void OnDisable()
//    {
//        if (m_PreviousAction != null)
//            m_PreviousAction.performed -= HandlePreviousActionPerformed;
//        if (m_NextAction != null)
//            m_NextAction.performed -= HandleNextActionPerformed;
//    }

//    protected virtual void Update()
//    {
//        if (m_SceneList.Count == 0) return;

//        if (m_CurrentSceneStep != m_PreviousSceneStep)
//        {
//            m_PreviousSceneStep = m_CurrentSceneStep;
//            m_RootContainer.Clear();
//            m_RootContainer.Add(m_SceneList.Find(s => s.sceneNumber == m_CurrentSceneStep).rootVisualElement);
//            //IEnumerable<VisualElement> children = m_RootContainer.Children();
//            //foreach (VisualElement element in children)
//            //    element.style.display = DisplayStyle.None;
//        }

//        if (m_SceneList[m_CurrentSceneStep].currentStep != m_SceneList[m_CurrentSceneStep].previousStep)
//        {
//            bool forward = m_SceneList[m_CurrentSceneStep].previousStep < m_SceneList[m_CurrentSceneStep].currentStep;
//            int previous = m_SceneList[m_CurrentSceneStep].previousStep;
//            m_SceneList[m_CurrentSceneStep].previousStep = m_SceneList[m_CurrentSceneStep].currentStep;
//            if (forward)
//                m_SceneList[m_CurrentSceneStep].steps[m_SceneList[m_CurrentSceneStep].currentStep].stepElement.style.display = DisplayStyle.Flex;
//            else
//                m_SceneList[m_CurrentSceneStep].steps[previous].stepElement.style.display = DisplayStyle.None;
//        }
//    }

//    protected virtual void RegisterSceneSteps()
//    {
//        for (int i = 0; i < m_ScenesToBeRegistered.Count; i++)
//            m_SceneList.Add(new VisualScene(m_ScenesToBeRegistered[i].CloneTree(), i));
//    }

//    protected virtual void HandlePreviousActionPerformed(InputAction.CallbackContext callbackContext)
//    {
//        if (m_SceneList[m_CurrentSceneStep].currentStep > 0)
//        {
//            m_SceneList[m_CurrentSceneStep].currentStep--;
//        }
//        else
//            m_CurrentSceneStep += m_CurrentSceneStep > 0 ? -1 : m_CurrentSceneStep;
//    }

//    protected virtual void HandleNextActionPerformed(InputAction.CallbackContext callbackContext)
//    {
//        if (m_SceneList[m_CurrentSceneStep].currentStep + 1 < m_SceneList[m_CurrentSceneStep].steps.Count)
//        {
//            m_SceneList[m_CurrentSceneStep].currentStep++;
//        }
//        else
//            m_CurrentSceneStep += m_CurrentSceneStep + 1 < m_SceneList.Count ? 1 : m_CurrentSceneStep;
//    }
}
