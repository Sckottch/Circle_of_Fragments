using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseData : UnitBaseData
{
    [Space(10)]
    [Header("Character Prefab")]
    public EnemyUnit unitPrefab;

    [Space(10)]
    [Header("Elemental MatchUps")]
    public List<Element> elementalWeaknesses;
    public Element elementalResistance;
}
