using System.Collections;
using UnityEngine;

public class PlayerManager : ManagerBase<PlayerManager>
{
    public override IEnumerator Initialize()
    {
        Debug.Log("Inicializando [PlayerManager]");



        Debug.Log("PlayerManager Inicializado");
        yield return null;
    }
}
