using System.Collections.Generic;
using System.Linq;

public static class TargetSystem
{
    public static bool IsValidTarget(Unit target, Unit caster, List<Unit> allTargets, SkillDataSO skillData)
    {
        List<Unit> validTargets = GetValidTargets(caster, allTargets, skillData);

        return validTargets.Contains(target);
    }

    public static List<Unit> GetValidTargets(Unit caster, List<Unit> allTargets, SkillDataSO skillData)
    {
        List<Unit> validTargets = new List<Unit>();

        if (skillData.isSelf)
        {
            validTargets.Add(caster);
            return validTargets;
        }

        if (skillData.isMainTargetAlly)
        {
            return allTargets.Where(target => target.IsPlayer == caster.IsPlayer).ToList();
        }
        else
        {
            return allTargets.Where(target => target.IsPlayer != caster.IsPlayer).ToList();
        }
    }

    public static List<Unit> GetUnisByTargetType(TargetType targetType, Unit caster, Unit mainTarget, List<Unit> allTargets)
    {
        List<Unit> targets = new List<Unit>();
        switch (targetType)
        {
            case TargetType.Single:
                if (mainTarget != null && !IsAlly(caster, mainTarget) && mainTarget != caster)
                {
                    targets.Add(mainTarget);
                }
                break;

            case TargetType.AOE:
                targets.AddRange(allTargets.FindAll(unit => !IsAlly(caster, unit)));
                break;

            case TargetType.Ally:
                if (mainTarget != null && IsAlly(caster, mainTarget) && mainTarget != caster)
                {
                    targets.Add(mainTarget);
                }
                break;

            case TargetType.AllyAOE:
                targets.AddRange(allTargets.FindAll(unit => IsAlly(caster, unit)));
                break;

            case TargetType.Self:
                targets.Add(caster);
                break;
        }
        return targets;
    }

    public static bool IsAlly(Unit a, Unit b)
    {
        return (a.IsPlayer && b.IsPlayer) || (!a.IsPlayer && !b.IsPlayer);
    }
}
