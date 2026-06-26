using UnityEngine;

public class UnitTurnData
{
    public Unit Unit { get; private set; }

    public float CurrentActionValue { get; private set; }
    public float CurrentSpeed { get; private set; }

    private const float actionGauge = 10000f;

    public UnitTurnData(Unit unit)
    {
        Unit = unit;
        CurrentSpeed = unit.GetStats().speed;
        CurrentActionValue = actionGauge / CurrentSpeed;
    }

    public void RecalculateActionValue()
    {
        CurrentActionValue = actionGauge / CurrentSpeed;
    }

    //AV = Action Value
    /// <summary>
    /// Applies a modifier to the current action value, reducing it or increasing it based on the percentage provided.
    /// </summary>
    /// <param name="modifier">The percentage by which the action value will change. If positive advance | If negative delay</param>
    public void ApplyAVModifier(float modifier)
    {
        float avModifier = (actionGauge / CurrentSpeed) * (modifier / 100);

        CurrentActionValue = Mathf.Max(0, CurrentActionValue - avModifier);
    }

    public void ReduceActionValue(float value)
    {
        CurrentActionValue = Mathf.Max(0, CurrentActionValue - value);
    }

    public void SpeedChanged()
    {
        CurrentActionValue *= CurrentSpeed / Unit.GetStats().speed;

        CurrentSpeed = Unit.GetStats().speed;
    }
}
