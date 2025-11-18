using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

class Interactable_Sun : IInteractable
{
    private Color m_OgFilter;
    private float m_OgTemperature;
    private float m_OgIntensity;
    private float m_OgIndirectMultiplier;

    [Header("Sunset Settings")]
    [SerializeField]
    private Light m_Sun;
    [SerializeField]
    private float m_TransitionSpeed;

    [SerializeField]
    private Color m_Filter;
    [SerializeField]
    private float m_Temperature;
    [SerializeField]
    private float m_Intensity;
    [SerializeField]
    private float m_IndirectMultiplier;

    [Header("Room Lights")]
    [SerializeField]
    private List<Light> m_Lights = new();
    private float[] m_OgLightIntensity = new float[0];
    private Coroutine[] m_LightCoroutines = new Coroutine[0];
    [SerializeField]
    private float m_LightIntensityFactor;

    bool m_SunSetMode;
    Coroutine m_Coroutine;

    protected override void Start()
    {
        base.Start();

        if (m_Sun != null)
        {
            m_OgFilter = m_Sun.color;
            m_OgTemperature = m_Sun.colorTemperature;
            m_OgIntensity = m_Sun.intensity;
            m_OgIndirectMultiplier = m_Sun.bounceIntensity;
        }

        if (m_Lights.Count > 0)
        {
            m_OgLightIntensity = new float[m_Lights.Count];
            m_LightCoroutines = new Coroutine[m_Lights.Count];
            for (int i = 0; i < m_Lights.Count; i++)
                m_OgLightIntensity[i] = m_Lights[i].intensity;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (m_SunSetMode)
        {
            if (m_Coroutine == null && m_Sun != null)
                m_Coroutine = StartCoroutine(SetSunset());
            if (m_Lights.Count > 0)
                for (int i = 0; i < m_LightCoroutines.Length; i++)
                    if (m_LightCoroutines[i] == null && m_Lights[i] != null)
                        m_LightCoroutines[i] = StartCoroutine(SetLightFactor(i, true));
        }
        else
        {
            if (m_Coroutine == null && m_Sun != null)
                m_Coroutine = StartCoroutine(SetDefault());
            if (m_Lights.Count > 0)
                for (int i = 0; i < m_LightCoroutines.Length; i++)
                    if (m_LightCoroutines[i] == null && m_Lights[i] != null)
                        m_LightCoroutines[i] = StartCoroutine(SetLightFactor(i, false));
        }
    }

    public override void OnInteracted()
    {
        m_SunSetMode = !m_SunSetMode;
        OnSwitch();
    }

    private void OnSwitch()
    {
        if (m_Coroutine != null)
        {
            StopCoroutine(m_Coroutine);
            m_Coroutine = null;
        }

        if (m_Lights.Count > 0)
            for (int i = 0; i < m_LightCoroutines.Length; i++)
            {
                if (m_LightCoroutines[i] != null)
                {
                    StopCoroutine(m_LightCoroutines[i]);
                    m_LightCoroutines[i] = null;
                }
            }
    }

    private IEnumerator SetLightFactor(int idx, bool sunset)
    {
        float count = 0f;
        float start = m_Lights[idx].intensity;
        float target = sunset ? m_OgLightIntensity[idx] * m_LightIntensityFactor : m_OgLightIntensity[idx];
        while (true)
        {
            count += Time.deltaTime;
            float t = count / m_TransitionSpeed;
            m_Lights[idx].intensity = Mathf.Lerp(start, target, t);
            yield return null;
        }
    }

    private IEnumerator SetSunset()
    {
        float count = 0f;
        while (true)
        {
            count += Time.deltaTime;
            float t = count / m_TransitionSpeed;
            m_Sun.color = Color.Lerp(m_OgFilter, m_Filter, t);
            m_Sun.colorTemperature = Mathf.Lerp(m_OgTemperature, m_Temperature, t);
            m_Sun.intensity = Mathf.Lerp(m_OgIntensity, m_Intensity, t);
            m_Sun.bounceIntensity = Mathf.Lerp(m_OgIndirectMultiplier, m_IndirectMultiplier, t);
            yield return null;
        }
    }

    private IEnumerator SetDefault()
    {
        float count = 0f;
        while (true)
        {
            count += Time.deltaTime;
            float t = count / m_TransitionSpeed;
            m_Sun.color = Color.Lerp(m_Filter, m_OgFilter, t);
            m_Sun.colorTemperature = Mathf.Lerp(m_Temperature, m_OgTemperature, t);
            m_Sun.intensity = Mathf.Lerp(m_Intensity, m_OgIntensity, t);
            m_Sun.bounceIntensity = Mathf.Lerp(m_IndirectMultiplier, m_OgIndirectMultiplier, t);
            yield return null;
        }
    }
}