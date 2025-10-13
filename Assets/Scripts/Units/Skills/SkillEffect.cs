using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillEffect : ScriptableObject
{
    [Space(10)]
    [Header("Tipo de Alvo")]
    public TargetType targetType;

    public abstract SkillEffectResult ApplyEffect(Unit targetUnit, Unit casterUnit, List<Unit> allTargets, SkillDataSO skillData);
}

public class SkillEffectResult
{
    public float Value { get; private set; }
    public SkillResultType ResultType { get; private set; }
    public List<Unit> AffectedUnits { get; set; } = new List<Unit>();

    public SkillEffectResult(float value, SkillResultType resultType, List<Unit> affectedUnits)
    {
        Value = value;
        ResultType = resultType;
        AffectedUnits = affectedUnits;
    }

}