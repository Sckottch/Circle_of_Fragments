public static class CombatCalculator
{
    public static CombatResult GetSkillDamage(Unit attacker, Unit defender, SkillDataSO skillData, float damageMultiplier)
    {
        //Setup Stats Needed for Calculation
        float attackerBaseStat = attacker.GetStats().GetStatByType(skillData.scalingType);
        float defenderDefense = defender.GetStats().GetStatByType(StatType.Defense);

        //Get Type Matchup
        ElementalMatchupResult matchupResult =
            defender.ElementalProfile.GetElementalMatchup(attacker.ElementalProfile.Element);
        SpecialStats matchupBonus = matchupResult.bonus;

        float critChance = attacker.GetSpecialStats().critRate + matchupBonus.critRate;
        float critDamage;
        bool criticalHit = RandomManager.RollChance(critChance);
        if (criticalHit)
        {
            critDamage =  1 + (attacker.GetSpecialStats().critDamage / 100);
        }
        else
        {
            critDamage = 1f;
        }

        float damageBonus = attacker.GetSpecialStats().damageBonus + matchupBonus.damageBonus;
        damageBonus = 1 + (damageBonus / 100);

        //Gets Efective Attack
        float baseStatMultiplier = 1 + (damageMultiplier / 100);
        float effectiveStat = attackerBaseStat * baseStatMultiplier;

        //Get Damage reduction
        float damageReduction = effectiveStat / (defenderDefense + effectiveStat);

        //Calculate Damage
        float damage = effectiveStat * damageBonus * critDamage * damageReduction;

        return new CombatResult() { 
            damageDealt = damage,
            isCriticalHit = criticalHit,
            wasWeakness = matchupResult.isWeakness,
            wasResistance = matchupResult.isResistance
        };
    }
}

public struct CombatResult
{
    public float damageDealt;
    public bool isCriticalHit;
    public bool wasWeakness;
    public bool wasResistance;
}

