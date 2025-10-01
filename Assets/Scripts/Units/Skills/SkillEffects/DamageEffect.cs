using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "ScriptableObjects/Skills/Effects/DamageEffect")]
public class DamageEffect : SkillEffect
{
    [Space(10)]
    [Header("Parametros da Skill")]
    [SerializeField] private float damageMultiplier;

    public override void ApplyEffect(Unit targetUnit, Unit casterUnit, List<Unit> allTargets, SkillDataSO skillData)
    {
        List<Unit> targets = TargetSystem.GetUnisByTargetType(targetType, casterUnit, targetUnit, allTargets);
        float baseStat = casterUnit.GetStats().GetStatByType(skillData.scalingType); 

        if (targets == null)
        {
            Debug.LogError("No targets found.");
            return;
        }

        foreach (Unit target in targets)
        {
            CombatResult combatResult = CombatCalculator.GetSkillDamage(casterUnit, target, skillData, damageMultiplier);
            float damage = combatResult.damageDealt;

            Debug.Log($"Caster: {casterUnit.UnitName} | Damage: {damage} | Target: {target.UnitName}");

            target.TakeDamage(damage);
        }
    }
}
