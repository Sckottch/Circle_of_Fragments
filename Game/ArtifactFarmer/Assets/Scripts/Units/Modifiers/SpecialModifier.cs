public class SpecialModifier : Modifier
{
    public SpecialStatType statType;

    public SpecialModifier(SpecialStatType statType, float value)
    {
        this.statType = statType;
        this.type = ModifierType.Flat;
        this.value = value;
    }
}
