using System.Collections.Generic;
using System;
using UnityEngine;

public class CombatUIManager : MonoBehaviour
{
    [Space(10)]
    [Header("UI References")]
    [SerializeField] private Transform playerHUDGroup;
    [SerializeField] private Transform enemyHUDGroup;
    [SerializeField] private ActionPanel actionPanel;
    [SerializeField] private TurnOrderUI turnOrderUI;

    private GameResources resources;

    private void Awake()
    {
        resources = GameManager.Instance.gameResources;
    }

    public void SubscribeUIEvents()
    {
        turnOrderUI.SubscribeEvents();
    }

    public void UnsubscribeUIEvents()
    {
        turnOrderUI.UnsubscribeEvents();
    }

    public void SetupUI(List<Unit> activeUnits)
    {
        foreach (Unit unit in activeUnits)
        {
            GameObject hud;

            if (unit.IsPlayer)
            {
                hud = Instantiate(resources.unitHUDPrefab, playerHUDGroup);
                hud.GetComponent<UnitHUD>().SetUnit(unit);
                continue;
            }

            if (unit is EnemyUnit { Category: EnemyCategory.Boss })
            {
                hud = Instantiate(resources.unitHUDPrefab, enemyHUDGroup);
                hud.GetComponent<UnitHUD>().SetUnit(unit);
                continue;
            }

            hud = Instantiate(resources.enemyUnitHUDPrefab, enemyHUDGroup);
            hud.GetComponent<CommonEnemyUnitHUD>().SetUnit(unit);
        }
    }

    public void OpenActionPanel(Unit unit, Action<Unit, ActionType> callback)
    {
        actionPanel.Open(unit, callback);
    }

    public void CloseActionPanel()
    {
        actionPanel.Close();
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
