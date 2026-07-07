using UnityEngine;

public class PlayableUnit : Unit
{
    public override bool IsPlayer => true;

    protected PlayableUnitSO PlayerData => (PlayableUnitSO)UnitData;

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

        this.ElementalProfile = new ElementalProfile(PlayerData.element, PlayerData.elementalWeaknesses,
            PlayerData.elementalResistance);

        this.CurrentMana = PlayerData.baseSpecialStats.mana;

        this.spriteRenderer.sprite = PlayerData.unitSprite;

        UnitLevel = new UnitProgression(baseStats, baseSpecialStats, experience, this);
        this.CurrentHealth = UnitLevel.GetStats().health;
        this.lastHealth = UnitLevel.GetStats().health;

        //Skills Setup
        skills.Add(new Skill(this, PlayerData.basicSkill));
        skills.Add(new Skill(this, PlayerData.specialSkill));
        skills.Add(new Skill(this, PlayerData.ultimateSkill));
    }

    public void InitializeFromPlayerData(PlayerUnitData data)
    {
        Initialize(data.unitData, data.experience);
    }

    public override Stats GetStats()
    {
        return UnitLevel.GetStats();
    }

    public override SpecialStats GetSpecialStats()
    {
        return UnitLevel.GetSpecialStats();
    }

    public override Skill GetSkill(ActionType type)
    {
        if (type is ActionType.Enemy or ActionType.Passive)
        {
            Debug.LogError("ActionType inválido para skill de player");
            return null;
        }

        return skills.Find(u => u.SkillData.skillType == type);
    }
}
