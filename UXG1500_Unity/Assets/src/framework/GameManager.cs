using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    [Header("Paths of scenes for loading")]
    private List<string> m_SceneList;
    [SerializeField]
    private int m_FirstSceneToLoad;
    private string m_NextScene;

    public bool m_GameState { get; private set; } = true;
    public bool m_ReadySceneSwitch { get; private set; } = false;

    public void ArmSceneSwitch() => m_ReadySceneSwitch = true;
    public void SetNextScene(string path) => m_NextScene = path;
    public List<string> GetSceneList() => m_SceneList;

    private void Start()
    {
        if (m_SceneList.Count > 0)
        {
            PrepareScene(0);
            m_ReadySceneSwitch = true;
        }
    }

    private void Update()
    {
        if (m_ReadySceneSwitch && SceneManager.GetSceneByPath(m_NextScene).isLoaded)
        {
            // Switch Scenes
            SceneManagement.Instance.SetActiveScene(m_NextScene);

            // Toggle back to false;
            m_ReadySceneSwitch = false;

            // Pause till ready to play
            HandleGamePause(true);
        }
    }

    public void PrepareScene(int sceneIdx)
    {
        m_NextScene = m_SceneList[sceneIdx];
        SceneManagement.Instance.PushLoadScene(m_NextScene);
    }

    public void HandleGamePause(bool pause)
    {
        m_GameState = pause;
        if (pause)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    public void HandleSceneLoaded()
    {
        // Handle scene loaded callback
    }
}
