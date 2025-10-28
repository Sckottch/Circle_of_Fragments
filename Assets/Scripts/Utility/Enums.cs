public enum ActionType
{
    BasicAttack,
    Skill,
    Ultimate,
    Passive
}

public enum StatType 
{
    Health,
    Attack,
    Defense,
    Speed
}

public enum SpecialStatType
{
    Mana,
    CritRate,
    CritDamage,
    DamageBonus,
    DamageReduction,
    EffectChance,
    EffectResistance,
    ManaRegeneration,
    HealingBonus,
    ShieldStrength,
    Accuracy,
    Evasion
}

public enum UnifiedStatType
{
    // Base stats
    Health,
    Attack,
    Defense,
    Speed,
    Mana,
    CritRate,
    CritDamage,

    // Special stats
    DamageBonus,
    EffectChance,
    EffectResistance,
    ManaRegeneration,
    HealingBonus,
    ShieldStrength,
    Accuracy,
    Evasion
}

public enum TargetType
{
    Single,
    AOE,
    Ally,
    AllyAOE,
    Self
}

public enum ModifierType
{
    Base,
    Flat,
    Percent
}

public enum Element
{
    //Basic
    Fire,
    Water,
    Earth,
    Wind,
    Ice,

    //Special
    Light,
    Dark,
    Lightning
}

public enum BuffActionTime
{
    None,
    OnTurnStart,
    OnTurnEnd,
    OnAttack,
    OnHit
}

public enum BuffCategory
{
    StatModifier,
    DamageOverTime,
    HealOverTime,
    Shield,
    Incapacitate,
    Control,
    Immunity,
    Trigger,
    Special
}

public enum SkillResultType
{
    DamageDealt,
    HealingDone,
    ManaRestored,
    ShieldApplied,
    Other
}

public enum CharacterClass
{
    Warrior,
    Mage,
    Rogue,
    Healer,
    Tank,
    Ranger,
    Support
}

public enum GameMode
{
    None,
    MainMenu,
    GameLobby,
    Expedition,
    Combat
}