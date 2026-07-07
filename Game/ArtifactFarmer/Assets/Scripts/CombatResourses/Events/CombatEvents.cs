using System;
using System.Collections.Generic;

public class CombatEvents
{
    public event Action<Unit> OnUnitDeath;
    public event Action<List<Unit>> OnWaveStart;
    public event Action OnTurnOrderChanged;
    public event Action<Unit, float> OnAVModified;
    public event Action<Unit> OnSpeedChanged; 

    public void UnitDied(Unit unit)
    {
        OnUnitDeath?.Invoke(unit);
    }

    public void WaveStarted(List<Unit> activeUnits)
    {
        OnWaveStart?.Invoke(activeUnits);
    }

    public void TurnOrderChanged()
    {
        OnTurnOrderChanged?.Invoke();
    }

    public void AVModified(Unit unit, float value)
    {
        OnAVModified?.Invoke(unit, value);
    }

    public void SpeedChanged(Unit unit)
    {
        OnSpeedChanged?.Invoke(unit);
    }
}