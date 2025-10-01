using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;

public class Skill
{
    private Unit caster;
    public SkillDataSO SkillData { get; private set; }

    public Skill(Unit caster, SkillDataSO skillData)
    {
        this.caster = caster;
        this.SkillData = skillData;
    }

    public bool Execute(Unit mainTarget, List<Unit> allTargets)
    {
        if (!caster.ConsumeMana(SkillData.manaCost))
            return false;

        foreach (SkillEffect effect in SkillData.skillEffects)
        {
            effect.ApplyEffect(mainTarget, caster, allTargets, SkillData);
        }

        return true;
    }

    public bool HasEnoughMana()
    {
        return caster.CurrentMana >= SkillData.manaCost;
    }
}
