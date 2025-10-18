using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnOrderUI : MonoBehaviour
{
    [SerializeField] private Transform ContentParent;
    [SerializeField] private TextMeshProUGUI turnDataText;

    private List<GameObject> currentIcons = new();


    public void SetupIcons(CurrentTurnData turnData)
    {
        Clear();

        turnDataText.text = $"{turnData.CurrentTurn} /// {Math.Round(turnData.CurrentTurnActionValue)}";

        foreach (UnitTurnData unitData in turnData.ActiveUnits)
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

    public void Clear()
    {
        foreach (GameObject icon in currentIcons)
            Destroy(icon);

        currentIcons.Clear();

        turnDataText.text = string.Empty;
    }


}
