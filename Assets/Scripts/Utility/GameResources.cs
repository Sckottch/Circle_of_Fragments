using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameResources", menuName = "Resources/GameResources", order = 1)]
public class GameResources : ScriptableObject
{
    [Space(10)]
    [Header("Combat UI Prefabs")]
    public GameObject unitHUDPrefab;
    public GameObject enemyUnitHUDPrefab;
    public GameObject turnIconPrefab;
    public GameObject skillButtonPrefab;

    [Space(10)]
    public Material defaultMaterial;

    [Space(10)]
    public List<PlayerUnitData> playerUnits;
    public CombatDataSO combatData;

    [Space(10)]
    [Header("Outlines")]
    public Material playerOutlineMaterial;
    public Material targetOutlineMaterial;

}
