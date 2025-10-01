using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillEffect : ScriptableObject
{
    [Space(10)]
    [Header("Tipo de Alvo")]
    public TargetType targetType;

    public abstract void ApplyEffect(Unit targetUnit, Unit casterUnit, List<Unit> allTargets, SkillDataSO skillData);
}
