using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatUIManager : MonoBehaviour
{
    [Space(10)]
    [Header("UI References")]
    [SerializeField] private Transform playerHUDGroup;
    [SerializeField] private Transform enemyHUDGroup;

    private GameResources resources;

    private void Awake()
    {
        resources = GameManager.Instance.gameResources;
    }

    public void SetupUI(List<PlayableUnit> playerUnits, List<EnemyUnit> enemyUnits)
    {
        foreach (Unit unit in playerUnits)
        {
            GameObject hud = Instantiate(resources.unitHUDPrefab, playerHUDGroup);
            hud.GetComponent<UnitHUD>().SetUnit(unit);
        }

        foreach (EnemyUnit unit in enemyUnits)
        {
            GameObject prefab;

            if (unit.Category == EnemyCategory.Boss)
            {
                prefab = resources.unitHUDPrefab;
                GameObject hud = Instantiate(prefab, enemyHUDGroup);
                hud.GetComponent<UnitHUD>().SetUnit(unit);
            }
            else
            {
                prefab = resources.enemyUnitHUDPrefab;
                GameObject hud = Instantiate(prefab, enemyHUDGroup);
                hud.GetComponent<CommonEnemyUnitHUD>().SetUnit(unit);
            }
        }

    }

    public void AddUnit(Unit unit)
    {
        if (unit.IsPlayer)
        {
            GameObject hud = Instantiate(resources.unitHUDPrefab, playerHUDGroup);
            hud.GetComponent<UnitHUD>().SetUnit(unit);
        }
        else
        {
            EnemyUnit enemyUnit = unit as EnemyUnit;
            GameObject prefab;

            if (enemyUnit.Category == EnemyCategory.Boss)
            {
                prefab = resources.unitHUDPrefab;
                GameObject hud = Instantiate(prefab, enemyHUDGroup);
                hud.GetComponent<UnitHUD>().SetUnit(enemyUnit);
            }
            else
            {
                prefab = resources.enemyUnitHUDPrefab;
                GameObject hud = Instantiate(prefab, enemyHUDGroup);
                hud.GetComponent<CommonEnemyUnitHUD>().SetUnit(enemyUnit);
            }
        }
    }

    public void ClearUI()
    {
        foreach (Transform child in playerHUDGroup)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in enemyHUDGroup)
        {
            Destroy(child.gameObject);
        }
    }

}
