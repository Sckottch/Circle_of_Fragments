using UnityEngine;
using System.Collections.Generic;

public class GameSystemRoot : SingletonMonoBehaviour<GameSystemRoot>
{
    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this.gameObject);
    }
}
