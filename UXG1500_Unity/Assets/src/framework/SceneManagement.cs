using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneManagement : Singleton<SceneManagement>
{
    public UnityEvent onSceneLoaded;
    public UnityEvent onSceneUnloaded;
    public UnityEvent<float> onSceneLoadProgress;
    public UnityEvent<float> onSceneUnloadProgress;
    private string m_SceneToLoad = "";
    private string m_SceneToUnload = "";

    private void Start()
    {
        
    }

    private void Update()
    {
        if (m_SceneToLoad != "")
        {
            StartCoroutine(LoadSceneAsync(m_SceneToLoad));
            m_SceneToLoad = "";
        }
        if (m_SceneToUnload != "")
        {
            StartCoroutine(UnloadSceneAsync(m_SceneToUnload));
            m_SceneToUnload = "";
        }
    }

    public void PushLoadScene(string path) => m_SceneToLoad = path;

    public bool SetActiveScene(string path)
    {
        string curPath = SceneManager.GetActiveScene().path;
        if (SceneManager.SetActiveScene(SceneManager.GetSceneByPath(path)))
        {
            m_SceneToUnload = curPath;
            return true;
        }
        else
            return false;
    }

    private IEnumerator LoadSceneAsync(string path)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(path, LoadSceneMode.Additive);
        while (!asyncOperation.isDone) 
        {
            onSceneLoadProgress?.Invoke(asyncOperation.progress);
            yield return null;
        }
        onSceneLoaded?.Invoke();
    }

    private IEnumerator UnloadSceneAsync(string path)
    {
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(path);
        while (!asyncOperation.isDone) 
        {
            onSceneUnloadProgress?.Invoke(asyncOperation.progress);
            yield return null;
        }
        onSceneUnloaded?.Invoke();
    }
}
