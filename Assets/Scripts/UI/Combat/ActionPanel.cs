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

        currentUnit = unit as PlayableUnit;
        onActionSelected = callBack;

        gameObject.SetActive(true);
        Clear();

        if (currentUnit.BasicSkill != null)
        {
            CreateButton(currentUnit.BasicSkill, () =>
            {
                onActionSelected?.Invoke(currentUnit, ActionType.BasicAttack);
                Close();
            });
        }

        if (currentUnit.SpecialSkill != null)
        {
            CreateButton(currentUnit.SpecialSkill, () =>
            {
                onActionSelected?.Invoke(currentUnit, ActionType.Skill);
                Close();
            });
        }

        if (currentUnit.UltimateSkill != null)
        {
            CreateButton(currentUnit.UltimateSkill, () =>
            {
                onActionSelected?.Invoke(currentUnit, ActionType.Ultimate);
                Close();
            });
        }
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
