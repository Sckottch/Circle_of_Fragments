using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "ScriptableObjects/Skills/Effects/DamageEffect")]
public class DamageEffect : SkillEffect
{
    [Space(10)]
    [Header("Parametros da Skill")]
    [SerializeField] private float damageMultiplier;

    public override SkillEffectResult ApplyEffect(Unit targetUnit, Unit casterUnit, List<Unit> allTargets, SkillDataSO skillData)
    {
        List<Unit> targets = TargetSystem.GetUnitsByTargetType(targetType, casterUnit, targetUnit, allTargets);
        float totalDamage = 0f;

        if (targets == null)
        {
            Debug.LogError("No targets found.");
            return null;
        }

        foreach (Unit target in targets)
        {
            CombatResult combatResult = CombatCalculator.GetSkillDamage(casterUnit, target, skillData, damageMultiplier);
            float damage = combatResult.damageDealt;

            Debug.Log($"Caster: {casterUnit.UnitName} | Damage: {damage} | Target: {target.UnitName}");

            target.TakeDamage(damage);

            totalDamage += damage;
        }

        return new SkillEffectResult(totalDamage, SkillResultType.DamageDealt, targets);
    }
}
