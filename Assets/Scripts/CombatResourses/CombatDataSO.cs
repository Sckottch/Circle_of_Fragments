using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatData", menuName = "ScriptableObjects/Combat/CombatData")]
public class CombatDataSO : ScriptableObject
{
    [Space(10)]
    [Header("Wave Info")]
    public List<WaveDataSO> waves;
}
