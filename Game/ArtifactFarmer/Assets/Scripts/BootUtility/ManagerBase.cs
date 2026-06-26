using System.Collections;
using UnityEngine;

public abstract class ManagerBase<T> : SingletonMonoBehaviour<T>, IInitializable where T : MonoBehaviour
{
    public abstract IEnumerator Initialize();
}
