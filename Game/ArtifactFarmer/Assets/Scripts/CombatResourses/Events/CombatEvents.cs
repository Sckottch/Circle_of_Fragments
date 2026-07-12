using System;
using System.Collections.Generic;

public class CombatEvents
{
    public event Action<Unit> OnUnitDeath;
    public event Action<List<Unit>> OnWaveStart;
    public event Action OnTurnOrderChanged;
    public event Action<Unit, float> OnAVModified;
    public event Action<Unit> OnSpeedChanged;
    public event Action<Unit> OnTurnStart;
    public event Action<Unit> OnTurnEnd;
    public event Action<Unit, Buff, Unit> OnBuffApplied;
    public event Action<Unit, Buff> OnBuffRemoved; 

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

    public void TurnStarted(Unit unit)
    {
        OnTurnStart?.Invoke(unit);
    }

    public void TurnEnd(Unit unit)
    {
        OnTurnEnd?.Invoke(unit);
    }

    public void BuffApplied(Unit target, Buff buff, Unit caster)
    {
        OnBuffApplied?.Invoke(target, buff, caster);
    }

    public void BuffRemoved(Unit target, Buff buff)
    {
        OnBuffRemoved?.Invoke(target, buff);
    }
}