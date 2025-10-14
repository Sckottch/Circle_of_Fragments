using UnityEngine;

public class UnitBaseData : ScriptableObject
{
    [Space(10)]
    [Header("Unit Info")]

    public string unitName;
    public Sprite unitSprite;

    [Space(10)]
    [Header("Unit Stats")]
    public Stats baseStats;
    public SpecialStats baseSpecialStats;
}

[System.Serializable]
public class Stats
{
    public float health;
    public float attack;
    public float defense;
    public float speed;

    public Stats(float health = 0, float attack = 0, float defense = 0, float speed = 0)
    {
        this.health = health;
        this.attack = attack;
        this.defense = defense;
        this.speed = speed;
    }

    public float GetStatByType(StatType statType)
    {
        return statType switch
        {
            StatType.Health => health,
            StatType.Attack => attack,
            StatType.Defense => defense,
            StatType.Speed => speed,
            _ => 0
        };
    }   
}

[System.Serializable]
public class SpecialStats
{
    public float mana;
    public float critRate;
    public float critDamage;
    public float damageBonus;
    public float damageReduction;
    public float effectChance;
    public float effectResistance;
    public float manaRegeneration;
    public float healingBonus;
    public float shieldStrength;
    public float accuracy;
    public float evasion;

    public SpecialStats(float mana = 0, float critRate = 0, float critDamage = 0, float damageBonus = 0,
        float effectChance = 0, float effectResistance = 0, float manaRegeneration = 0, float healingBonus = 0,
        float shieldStrength = 0, float accuracy = 0, float evasion = 0)
    {
        this.mana = mana;
        this.critRate = critRate;
        this.critDamage = critDamage;
        this.damageBonus = damageBonus;
        this.effectChance = effectChance;
        this.effectResistance = effectResistance;
        this.manaRegeneration = manaRegeneration;
        this.healingBonus = healingBonus;
        this.shieldStrength = shieldStrength;
        this.accuracy = accuracy;
        this.evasion = evasion;
    }

    public float GetStatByType(SpecialStatType statType)
    {
        return statType switch
        {
            SpecialStatType.Mana => mana,
            SpecialStatType.CritRate => critRate,
            SpecialStatType.CritDamage => critDamage,
            SpecialStatType.DamageBonus => damageBonus,
            SpecialStatType.EffectChance => effectChance,
            SpecialStatType.EffectResistance => effectResistance,
            SpecialStatType.ManaRegeneration => manaRegeneration,
            SpecialStatType.HealingBonus => healingBonus,
            SpecialStatType.ShieldStrength => shieldStrength,
            SpecialStatType.Accuracy => accuracy,
            SpecialStatType.Evasion => evasion,
            _ => 0
        };
    }
}