using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatSetup : MonoBehaviour
{
    [SerializeField] private Transform playerTeamTransform;
    [SerializeField] private Transform enemyTeamTransform;

    public List<PlayableUnit> InitializePlayerUnits(List<PlayerUnitData> playerUnits)
    {
        List<PlayableUnit> units = new ();

        //offsets temporários para teste, remover/mudar após migração para 3D
        Vector3[] offsets =
        {
            new(0,0,0),
            new(-3,3,0),
            new(-3,-3,0),
            new(-6,0,0)
        };

        foreach(GameObject child in playerTeamTransform)
        {
            Destroy(child);
        }

        for (int i = 0; i < playerUnits.Count; i++)
        {
            PlayerUnitData unit = playerUnits[i];
            PlayableUnit playerUnit = Instantiate(unit.unitData.unitPrefab, playerTeamTransform);
            playerUnit.transform.position += offsets[i];
            playerUnit.InitializeFromPlayerData(unit);

            units.Add(playerUnit);
        }

        return units;
    }

    public EnemyUnit SpawnEnemyUnit(EnemySpawnInfo enemyInfo)
    {
        EnemyUnit enemyUnit = Instantiate(enemyInfo.enemyData.unitPrefab, enemyTeamTransform);
        enemyUnit.transform.position += enemyInfo.offset;

        enemyUnit.Initialize(enemyInfo.enemyData);

        return enemyUnit;
    }
}
