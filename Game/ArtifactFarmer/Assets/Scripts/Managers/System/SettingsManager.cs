using System.Collections;
using UnityEngine;

public class SettingsManager : ManagerBase<SettingsManager>
{
    public override IEnumerator Initialize()
    {
        Debug.Log("Inicializando [SettingsManager]");



        Debug.Log("SettingsManager Inicializado");
        yield return null;
    }
}
