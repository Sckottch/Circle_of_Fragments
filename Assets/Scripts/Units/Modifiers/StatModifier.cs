public class StatModifier: Modifier
{
    public StatType statType;

    public StatModifier(StatType statType, ModifierType modifierType, float value)
    {
        this.statType = statType;
        this.type = modifierType;
        this.value = value;
    }
}
