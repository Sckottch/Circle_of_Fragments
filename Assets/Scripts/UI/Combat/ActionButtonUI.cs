using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button button;
    [SerializeField] private Image blockerPanel;


    private Action onClickAction;

    public void SetSkill(Skill skill, Action action)
    {
        SkillDataSO skillData = skill.SkillData;


        nameText.text = skillData.skillName;
        onClickAction = action;

        bool isUsable = skill.HasEnoughMana();

        button.interactable = isUsable;

        if(!isUsable)
        {
            blockerPanel.gameObject.SetActive(true);
        }
        else
        {
            blockerPanel.gameObject.SetActive(false);
        }


        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }



    private void OnClick()
    {
        onClickAction?.Invoke();
    }
}
