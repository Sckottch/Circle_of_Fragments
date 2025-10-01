using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "ScriptableObjects/Combat/WaveData")]
public class WaveDataSO : ScriptableObject
{
    public List<EnemySpawnInfo> enemies;
}

[System.Serializable]
public class EnemySpawnInfo
{
    public EnemyBaseData enemyData;
    public Vector3 offset; 
}
