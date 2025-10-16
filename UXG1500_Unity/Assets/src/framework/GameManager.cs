using System.Collections.Generic;
using System.IO;
using System.Reflection;
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

    protected override void Start()
    {
        base.Start();
        Logger.Log("Initialised " + MethodBase.GetCurrentMethod().ReflectedType.Name, 
            Logger.SEVERITY_LEVEL.INFO, 
            Logger.LOGGER_OPTIONS.SIMPLE);
        
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
            if (!SceneManagement.Instance.SetActiveScene(m_NextScene))
                Logger.Log("Scene " + m_NextScene + " has not yet been loaded! Unable to switch scenes",
                    Logger.SEVERITY_LEVEL.WARNING,
                    Logger.LOGGER_OPTIONS.VERBOSE,
                    MethodBase.GetCurrentMethod());

            // Toggle back to false;
            m_ReadySceneSwitch = false;

            // Pause till ready to play
            HandleGamePause(true);
        }
    }

    public void PrepareScene(int sceneIdx)
    {
        if (SceneManager.GetActiveScene().path == m_SceneList[sceneIdx])
        {
            SceneManagement.Instance.ReloadScene();
            return;
        }
        m_NextScene = m_SceneList[sceneIdx];
        SceneManagement.Instance.PushLoadScene(m_NextScene);
    }

    public void HandleGamePause(bool pause)
    {
        Logger.Log("Current Scene Paused",
            Logger.SEVERITY_LEVEL.INFO,
            Logger.LOGGER_OPTIONS.VERBOSE,
            MethodBase.GetCurrentMethod());

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
