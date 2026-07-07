using UnityEngine;

public abstract class EnemyBaseData : UnitBaseData
{
    [Space(10)]
    [Header("Character Prefab")]
    public EnemyUnit unitPrefab;
}
