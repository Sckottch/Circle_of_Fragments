using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RestoreManaFlat", menuName = "ScriptableObjects/Skills/Effects/RestoreFlatManaEffect")]
public class RestoreFlatManaEffect : SkillEffect
{
    [Space(10)]
    [Header("Parametros da Skill")]
    public float restoreAmount;

    public override SkillEffectResult ApplyEffect(Unit targetUnit, Unit casterUnit, List<Unit> allTargets, SkillDataSO skillData)
    {
        List<Unit> targets = TargetSystem.GetUnitsByTargetType(targetType, casterUnit, targetUnit, allTargets);
        float totalRestored = 0f;

        foreach (Unit target in targets)
        {
            target.GainMana(restoreAmount);
            totalRestored += restoreAmount;
        }

        return new SkillEffectResult(totalRestored, SkillResultType.ManaRestored, targets);
    }
}
