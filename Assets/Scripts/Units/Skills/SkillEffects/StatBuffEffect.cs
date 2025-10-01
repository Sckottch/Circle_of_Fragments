using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatBuffEffect", menuName = "ScriptableObjects/Skills/Effects/Stat Buff Effect")]
public class StatBuffEffect : SkillEffect
{
    [Space(10)]
    [Header("Parametros da Skill")]
    [SerializeField] private StatBuff statBuff;

    public override void ApplyEffect(Unit targetUnit, Unit casterUnit, List<Unit> allTargets, SkillDataSO skillData)
    {
        List<Unit> targets = TargetSystem.GetUnisByTargetType(targetType, casterUnit, targetUnit, allTargets);

        foreach (Unit target in targets)
        {
            target.ApplyBuff(statBuff, casterUnit);
        }
    }
}
