using JetBrains.Annotations;
using UnityEngine;

public class UnitProgression
{
    public int CurrentLevel { get; private set; }
    public int TotalExperience { get; private set; }
    public int CurrentExperience { get; private set; }
    public int CurrentAscensionLevel { get; private set; }

    private Unit unit;
    private Stats baseStats;
    private SpecialStats baseSpecialStats;

    public const int maxLevel = 150;
    public const int ascensionLevel = 30;

    public UnitProgression(Stats baseStats, SpecialStats baseSpecialStats, int experience, Unit unit)
    {
        this.unit = unit;
        this.baseStats = baseStats;
        this.baseSpecialStats = baseSpecialStats;
        CurrentLevel = 1;
        GainExperience(experience);
        //temporário até a implementação do sistema de ascensão
        CurrentAscensionLevel = Mathf.FloorToInt(CurrentLevel / (float)ascensionLevel);
    }
    
    public int GetNeddedExperience()
    {
        return 100 * CurrentLevel + (int)(Mathf.Pow(CurrentLevel, 3));
    }

    public bool GainExperience(int xp)
    {
        TotalExperience += xp;
        CurrentExperience += xp;
        bool leveledUp = false;

        while (CurrentLevel < maxLevel && CurrentExperience >= GetNeddedExperience())
        {
            CurrentExperience -= GetNeddedExperience();
            CurrentLevel++;

            //temporário até a implementação do sistema de ascensão
            CurrentAscensionLevel = Mathf.FloorToInt(CurrentLevel / (float)ascensionLevel);

            leveledUp = true;
        }

        return leveledUp;
    }

    public SpecialStats GetSpecialStats()
    {
        ModifierSystem modifierSystem = unit.ModifierSystem;
        SpecialStats stats = new()
        {
            mana = baseSpecialStats.mana + modifierSystem.GetSpecialModifierValue(SpecialStatType.Mana),
            critRate = baseSpecialStats.critRate + modifierSystem.GetSpecialModifierValue(SpecialStatType.CritRate),
            critDamage = baseSpecialStats.critDamage + modifierSystem.GetSpecialModifierValue(SpecialStatType.CritDamage),
            damageBonus = baseSpecialStats.damageBonus + modifierSystem.GetSpecialModifierValue(SpecialStatType.DamageBonus),
            damageReduction = baseSpecialStats.damageReduction + modifierSystem.GetSpecialModifierValue(SpecialStatType.DamageReduction),
            effectChance = baseSpecialStats.effectChance + modifierSystem.GetSpecialModifierValue(SpecialStatType.EffectChance),
            effectResistance = baseSpecialStats.effectResistance + modifierSystem.GetSpecialModifierValue(SpecialStatType.EffectResistance),
            manaRegeneration = baseSpecialStats.manaRegeneration + modifierSystem.GetSpecialModifierValue(SpecialStatType.ManaRegeneration),
            healingBonus = baseSpecialStats.healingBonus + modifierSystem.GetSpecialModifierValue(SpecialStatType.HealingBonus),
            shieldStrength = baseSpecialStats.shieldStrength + modifierSystem.GetSpecialModifierValue(SpecialStatType.ShieldStrength),
            accuracy = baseSpecialStats.accuracy + modifierSystem.GetSpecialModifierValue(SpecialStatType.Accuracy),
            evasion = baseSpecialStats.evasion + modifierSystem.GetSpecialModifierValue(SpecialStatType.Evasion)
        };
        return stats;
    }

    public Stats GetBaseStats()
    {
        ModifierSystem modifierSystem = unit.ModifierSystem;

        Stats stats = new()
        {
            health = CalculateStat(baseStats.health) + modifierSystem.GetStatModifierValue(StatType.Health, ModifierType.Base),
            attack = CalculateStat(baseStats.attack) + modifierSystem.GetStatModifierValue(StatType.Attack, ModifierType.Base),
            defense = CalculateStat(baseStats.defense) + modifierSystem.GetStatModifierValue(StatType.Defense, ModifierType.Base),
            speed = baseStats.speed + modifierSystem.GetStatModifierValue(StatType.Speed, ModifierType.Base)
        };

        return stats;
    }

    public Stats GetStats()
    {
        Stats stats = GetBaseStats();

        return new() { 
            health = ApplyModifiers(StatType.Health, stats.health),
            attack = ApplyModifiers(StatType.Attack, stats.attack),
            defense = ApplyModifiers(StatType.Defense, stats.defense),
            speed = ApplyModifiers(StatType.Speed, stats.speed)
        };
    }

    private float CalculateStat(float baseStat)
    {
        float multiplier = 1 + (0.10f * (CurrentLevel - 1));
        float ascensionMultiplier = 1 + (0.05f * CurrentAscensionLevel);

        return Mathf.Round(baseStat * multiplier * ascensionMultiplier);
    }

    private float ApplyModifiers(StatType stat, float baseValue)
    {
        float percentModifier = unit.ModifierSystem.GetStatModifierValue(stat, ModifierType.Percent);
        float flatModifier = unit.ModifierSystem.GetStatModifierValue(stat, ModifierType.Flat);
        return baseValue * (1 + percentModifier / 100) + flatModifier;
    }

}
