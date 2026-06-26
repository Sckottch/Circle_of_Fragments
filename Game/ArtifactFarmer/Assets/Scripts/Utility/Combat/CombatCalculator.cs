using UnityEngine;

public static class CombatCalculator
{
    public static CombatResult GetSkillDamage(Unit attacker, Unit defender, SkillDataSO skillData, float damageMultiplier)
    {
        float damage;

        //Setup Stats Needed for Calculation
        float attackerBaseStat = attacker.GetStats().GetStatByType(skillData.scalingType);
        float defenderDefense = defender.GetStats().GetStatByType(StatType.Defense);

        //Get Type Matchup
        SpecialStats matchupBonus;
        ElementalMatchupResult matchupResult = new();
        if (attacker is PlayableUnit player)
        {
            matchupResult = GetPlayerElementalMatchup(player, (EnemyUnit)defender);
            matchupBonus = matchupResult.bonus;
        }
        else if (attacker is EnemyUnit enemy)
        {
            //enemy specific matchup bonus not yet implemented
            matchupBonus = new();
        }
        else
        {
            Debug.LogError("Attacker is not a valid unit type");
            return new CombatResult();
        }

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
        damage = effectiveStat * damageBonus * critDamage * damageReduction;

        return new CombatResult() { 
            damageDealt = damage,
            isCriticalHit = criticalHit,
            wasWeakness = matchupResult.isWeakness,
            wasResistance = matchupResult.isResistance
        };
    }

    private static ElementalMatchupResult GetPlayerElementalMatchup(PlayableUnit attacker, EnemyUnit defender)
    {
        ElementalMatchupResult result = new ElementalMatchupResult();
        result.bonus = new();

        if (IsPlayerAdvantage(attacker, defender))
        {
            result.bonus.critRate = 10f;
            result.bonus.damageBonus = 20f;
            result.bonus.effectChance = 10f;
            result.isWeakness = true;
            return result;
        }

        if (IsPlayerResisted(attacker, defender))
        {
            result.bonus.critRate = -5f;
            result.bonus.damageBonus = -10f;
            result.bonus.effectChance = -5f;
            result.isResistance = true;
            return result;
        }

        if (Helper.IsSpecialElement(attacker.Element))
        {
            result.bonus.critRate = 5f;
            result.bonus.damageBonus = 10f;
            result.bonus.effectChance = 5f;
            return result;
        }

        return result;
    }

    private static bool IsPlayerAdvantage(PlayableUnit attacker, EnemyUnit defender)
    {
        return defender.ElementalWeaknesses.Contains(attacker.Element);
    }

    private static bool IsPlayerResisted(PlayableUnit attacker, EnemyUnit defender)
    {
        return attacker.Element == defender.ElementalResistance;
    }
}

public struct CombatResult
{
    public float damageDealt;
    public bool isCriticalHit;
    public bool wasWeakness;
    public bool wasResistance;
}

public struct ElementalMatchupResult
{
    public SpecialStats bonus;
    public bool isWeakness;
    public bool isResistance;
}