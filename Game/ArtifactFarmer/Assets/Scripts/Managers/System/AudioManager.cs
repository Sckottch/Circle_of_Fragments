using System.Collections;
using UnityEngine;

public class AudioManager : ManagerBase<AudioManager>
{
    public override IEnumerator Initialize()
    {
        Debug.Log("Inicializando [AudioManager]");



        Debug.Log("AudioManager Inicializado");
        yield return null;
    }
}
