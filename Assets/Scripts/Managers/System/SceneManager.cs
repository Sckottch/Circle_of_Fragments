using System.Collections;
using UnityEngine;

public class SceneManager : ManagerBase<SceneManager>
{
    public override IEnumerator Initialize()
    {
        Debug.Log("Inicializando [SceneManager]");



        Debug.Log("SceneManager Inicializado");
        yield return null;
    }
}
