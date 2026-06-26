using System.Collections;
using UnityEngine;

public class LocalizationManager : ManagerBase<LocalizationManager>
{
    public override IEnumerator Initialize()
    {
        Debug.Log("Inicializando [LocalizationManager]");



        Debug.Log("LocalizationManager Inicializado");
        yield return null;
    }
}
