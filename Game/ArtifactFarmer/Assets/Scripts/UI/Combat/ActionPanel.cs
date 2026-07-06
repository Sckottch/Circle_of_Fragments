using System;
using UnityEngine;

public class ActionPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform buttonContainer;

    private PlayableUnit currentUnit;

    private Action<Unit, ActionType> onActionSelected;

    public void Open(Unit unit, Action<Unit, ActionType> callBack)
    {
        if(!unit.IsPlayer)
        {
            Debug.LogError("Unit is not a playable unit");
            return;
        }

        gameObject.SetActive(true);
        Clear();

        CreateButton(unit.GetSkill(ActionType.BasicAttack), () =>
        {
            callBack.Invoke(unit, ActionType.BasicAttack);
            Close();
        });
        
        CreateButton(unit.GetSkill(ActionType.Skill), () =>
        {
            callBack.Invoke(unit, ActionType.Skill);
            Close();
        });
        
        CreateButton(unit.GetSkill(ActionType.Ultimate), () =>
        {
            callBack.Invoke(unit, ActionType.Ultimate);
            Close();
        });
    }

    public void Close()
    {
        Clear();
        gameObject.SetActive(false);
    }

    private void Clear()
    {
        foreach (Transform child in buttonContainer)
            Destroy(child.gameObject);

    }

    private void CreateButton(Skill skill, Action onClick)
    {
        GameObject button = Instantiate(GameManager.Instance.gameResources.skillButtonPrefab, buttonContainer);
        ActionButtonUI actionButton = button.GetComponent<ActionButtonUI>();
        actionButton.SetSkill(skill, onClick);
    }
}
