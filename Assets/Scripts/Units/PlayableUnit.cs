using System.Collections.Generic;
using UnityEngine;

public class PlayableUnit : Unit
{
    public override bool IsPlayer => true;

    protected PlayableUnitSO PlayerData => (PlayableUnitSO)UnitData;
    public Element Element { get; protected set; }

    public Skill BasicSkill { get; protected set; }
    public Skill SpecialSkill { get; protected set; }
    public Skill UltimateSkill { get; protected set; }

    public UnitProgression UnitLevel { get; protected set; }

    public override void Initialize(UnitBaseData playerData, int experience = 0)
    {
        if (playerData is not PlayableUnitSO)
        { 
            Debug.LogError("Unit data is not of type PlayableUnitSO");
            return;
        }

        //Set Player Data
        this.UnitData = playerData;

        //Unit Basic Info Setup
        this.UnitName = PlayerData.unitName;

        this.baseStats = PlayerData.baseStats;
        this.baseSpecialStats = PlayerData.baseSpecialStats;

        this.Element = PlayerData.element;

        this.CurrentMana = PlayerData.baseSpecialStats.mana;

        this.spriteRenderer.sprite = PlayerData.unitSprite;

        UnitLevel = new UnitProgression(baseStats, baseSpecialStats, experience, this);
        this.CurrentHealth = UnitLevel.GetStats().health;
        this.lastHealth = UnitLevel.GetStats().health;

        //Skills Setup
        BasicSkill = new Skill(this, PlayerData.basicSkill);
        SpecialSkill = new Skill(this, PlayerData.specialSkill);
        UltimateSkill = new Skill(this, PlayerData.ultimateSkill);
    }

    public void InitializeFromPlayerData(PlayerUnitData data)
    {
        Initialize(data.unitData, data.experience);
    }

    public override bool TurnAction(Unit mainTarget, List<Unit> allTargets, ActionType action)
    {
        return UseSkill(action, mainTarget, allTargets);
    }

    public override Stats GetStats()
    {
        return UnitLevel.GetStats();
    }

    public override SpecialStats GetSpecialStats()
    {
        return UnitLevel.GetSpecialStats();
    }

    protected bool UseSkill(ActionType action, Unit mainTarget, List<Unit> allTargets)
    {
        Skill skill = null;
        bool isSkillUsed = false;

        switch (action)
        {
            case ActionType.BasicAttack:
                skill = BasicSkill;
                break;
            case ActionType.Skill:
                skill = SpecialSkill;
                break;
            case ActionType.Ultimate:
                skill = UltimateSkill;
                break;
        }

        if (skill != null)
        {
            isSkillUsed = skill.Execute(mainTarget, allTargets);
        }

        if (isSkillUsed)
        {
            Debug.Log($"{UnitName} used Skill {skill.SkillData.skillName} used on {mainTarget.UnitName}");
        }
        else
        {
            Debug.Log($"Skill {skill.SkillData.skillName} failed to execute.");
        }

        return isSkillUsed;
    }
}
