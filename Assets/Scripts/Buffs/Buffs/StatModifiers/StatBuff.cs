using UnityEngine;

[CreateAssetMenu(fileName = "StatBuff", menuName = "ScriptableObjects/Buffs/Stat Modifiers/Stat Buff")]
public class StatBuff : Buff
{
    [SerializeField] private UnifiedStatType statType;
    [SerializeField] private float value;
    [SerializeField] private ModifierType modifierType;

    private Modifier currentModifier;

    public override void ApplyEffect(Unit target)
    {
        
    }

    public override void OnApply(Unit target, Unit caster)
    {
        if (Caster == null && target == caster)
        {
            duration += 1;
        }

        Add();
        Caster = caster;

        if (currentModifier == null)
        {
            ApplyStatModifier(target);
            return;
        }

        target.ModifierSystem.RemoveModifier(currentModifier);
        ApplyStatModifier(target);
    }

    public override void OnRemove(Unit target)
    {
        target.ModifierSystem.RemoveModifier(currentModifier);
    }

    private void ApplyStatModifier(Unit target)
    {
        float modifierValue = value * stacks.Count;

        currentModifier = target.ModifierSystem.CreateModifier(statType, modifierType, modifierValue);

        if (statType == UnifiedStatType.Health) target.MaxHealthChanged();

        if (statType == UnifiedStatType.Speed) target.SpeedChanged();

        if (statType == UnifiedStatType.Mana) target.ManaChanged();
    }
}
