using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : ManagerBase<SceneLoader>
{
    public string CurrentSceneName { get; private set; } = "Boot";

    public override IEnumerator Initialize()
    {
        Debug.Log("Inicializando [SceneManager]");



        Debug.Log("SceneManager Inicializado");
        yield return null;
    }

    public IEnumerator LoadScene(string sceneName)
    {
        if (sceneName == CurrentSceneName)
        {
            Debug.LogWarning($"Cena '{sceneName}' já está carregada.");
            yield break;
        }

        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName);
        asyncOp.allowSceneActivation = true;

        while (!asyncOp.isDone)
            yield return null;

        CurrentSceneName = sceneName;
        Debug.Log($"Cena '{sceneName}' carregada com sucesso.");
    }
}
