using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RestoreManaFlat", menuName = "ScriptableObjects/Skills/Effects/RestoreFlatManaEffect")]
public class RestoreFlatManaEffect : SkillEffect
{
    [Space(10)]
    [Header("Parametros da Skill")]
    public float restoreAmount;

    public override void ApplyEffect(Unit targetUnit, Unit casterUnit, List<Unit> allTargets, SkillDataSO skillData)
    {
        List<Unit> targets = TargetSystem.GetUnisByTargetType(targetType, casterUnit, targetUnit, allTargets);

        foreach (Unit target in targets)
        {
            target.GainMana(restoreAmount);
        }
    }
}
