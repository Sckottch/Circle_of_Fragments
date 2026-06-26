using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModifierSystem
{
    public List<Modifier> AllModifiers { get; private set; } = new();
    public List<StatModifier> StatModifiers { get; private set; } = new();
    public List<SpecialModifier> SpecialModifiers { get; private set; } = new();

    /// <summary>
    /// Crate a new modifier based on the stat type, modifier type and value. Also adds it to the modifier system.
    /// </summary>
    /// <returns>
    /// Returns the created modifier. If the stat type is invalid, returns null.
    /// </returns>
    public Modifier CreateModifier(UnifiedStatType statType, ModifierType type, float value)
    {
        Modifier modifier;

        if (Enum.TryParse(statType.ToString(), out StatType stat))
        {
            modifier = new StatModifier(stat, type, value);
            AddModifier(modifier);
            return modifier;
        }

        if (Enum.TryParse(statType.ToString(), out SpecialStatType specialStat))
        {
            modifier = new SpecialModifier(specialStat, value);
            AddModifier(modifier);
            return modifier;
        }

        Debug.LogError($"Invalid stat type: {statType}");
        return null; 
    }

    public void AddModifier(Modifier modifier)
    {
        AllModifiers.Add(modifier);

        if(modifier is StatModifier sm) StatModifiers.Add(sm);

        if (modifier is SpecialModifier sp) SpecialModifiers.Add(sp);
    }

    public void RemoveModifier(Modifier modifier)
    {
        AllModifiers.Remove(modifier);

        if (modifier is StatModifier sm) StatModifiers.Remove(sm);

        if (modifier is SpecialModifier sp) SpecialModifiers.Remove(sp);
    }

    public void ClearModifiers()
    {
        AllModifiers.Clear();
        StatModifiers.Clear();
        SpecialModifiers.Clear();
    }

    public float GetStatModifierValue(StatType statType, ModifierType modifierType)
    {
        return StatModifiers
            .Where(m => m.statType == statType && m.type == modifierType)
            .Sum(m => m.value);
    }

    public float GetSpecialModifierValue(SpecialStatType statType)
    {
        return SpecialModifiers
            .Where(m => m.statType == statType)
            .Sum(m => m.value);
    }

}
