using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;

public class Skill
{
    private Unit caster;
    public SkillDataSO SkillData { get; private set; }

    public Skill(Unit caster, SkillDataSO skillData)
    {
        this.caster = caster;
        SkillData = skillData;
    }

    public SkillResult Execute(Unit mainTarget, List<Unit> allTargets)
    {
        SkillResult finalResult = new SkillResult();

        if (!caster.ConsumeMana(SkillData.manaCost))
            return null;

        foreach (SkillEffect effect in SkillData.skillEffects)
        {
            SkillEffectResult effectResult = effect.ApplyEffect(mainTarget, caster, allTargets, SkillData);

            finalResult.AddSkillEffectResult(effectResult);
        }

        return finalResult;
    }

    public bool HasEnoughMana()
    {
        return caster.CurrentMana >= SkillData.manaCost;
    }
}

public class SkillResult
{
    public float DamageDealt { get; private set; }
    public float HealingDone { get; private set; }
    public float ManaRestored { get; private set; }
    public float ShieldApplied { get; private set; }
    public List<Unit> AffectedUnits { get; set; }

    public SkillResult(float damageDealt = 0, float healingDone = 0, float manaRestored = 0, float shieldApplied = 0, List<Unit> affectedUnits = null)
    {
        DamageDealt = damageDealt;
        HealingDone = healingDone;
        ManaRestored = manaRestored;
        ShieldApplied = shieldApplied;
        AffectedUnits = affectedUnits;
    }

    public void AddSkillEffectResult(SkillEffectResult effectResult)
    {
        switch (effectResult.ResultType)
        {
            case SkillResultType.DamageDealt:
                DamageDealt += effectResult.Value;
                break;

            case SkillResultType.HealingDone:
                HealingDone += effectResult.Value;
                break;

            case SkillResultType.ManaRestored:
                ManaRestored += effectResult.Value;
                break;

            case SkillResultType.ShieldApplied:
                ShieldApplied += effectResult.Value;
                break;

            case SkillResultType.Other:
                break;
        }

        AffectedUnits ??= new List<Unit>();

        foreach (Unit unit in effectResult.AffectedUnits)
        {
            if (!AffectedUnits.Contains(unit))
            {
                AffectedUnits.Add(unit);
            }
        }
    }
}