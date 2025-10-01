using System.Collections.Generic;
using UnityEngine;

public class TurnOrderUI : MonoBehaviour
{
    [SerializeField] private Transform ContentParent;

    private List<GameObject> currentIcons = new();


    public void SetupIcons(List<UnitTurnData> units)
    {
        Clear();

        foreach (UnitTurnData unitData in units)
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
    }


}
