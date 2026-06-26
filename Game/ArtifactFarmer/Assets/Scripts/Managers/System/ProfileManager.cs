using System.Collections;
using UnityEngine;

public class ProfileManager : ManagerBase<ProfileManager>
{
    public override IEnumerator Initialize()
    {
        Debug.Log("Inicializando [ProfileManager]");



        Debug.Log("ProfileManager Inicializado");
        yield return null;
    }
}
