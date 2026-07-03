using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TurnOrderUI : MonoBehaviour
{
    [SerializeField] private Transform ContentParent;
    [SerializeField] private TextMeshProUGUI turnDataText;

    private List<GameObject> currentIcons = new();

    public void SubscribeEvents()
    {
        CombatManager.Instance.Events.OnTurnOrderChanged += SetupIcons;
    }

    public void UnsubscribeEvents()
    {
        
        CombatManager.Instance.Events.OnTurnOrderChanged -= SetupIcons;
    }

    private void SetupIcons()
    {
        CombatContext context = CombatManager.Instance.Context;
        Clear();

        turnDataText.text = $"{context.CurrentTurn} /// {Math.Round(context.CurrentTurnActionValue)}";

        foreach (UnitTurnData unitData in context.TurnData.OrderBy(u => u.CurrentActionValue))
        {
            CreateIcon(unitData.Unit, (int)unitData.CurrentActionValue);
        }
    }

    private void CreateIcon(Unit unit, int index)
    {
        GameObject go = Instantiate(GameManager.Instance.gameResources.turnIconPrefab, ContentParent);
        TurnOrderIcon icon = go.GetComponent<TurnOrderIcon>();
        icon.SetUnit(unit, index);

        currentIcons.Add(go);
    }

    private void Clear()
    {
        foreach (GameObject icon in currentIcons)
            Destroy(icon);

        currentIcons.Clear();

        turnDataText.text = string.Empty;
    }


}