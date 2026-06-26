using UnityEngine;
using System.Collections;

public class SaveManager : ManagerBase<SaveManager>
{
    public override IEnumerator Initialize()
    {
        Debug.Log("Inicializando [SaveManager]");



        Debug.Log("SaveManager Inicializado");
        yield return null;
    }
}
