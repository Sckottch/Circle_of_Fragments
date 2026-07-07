using System;
using System.Collections.Generic;
using System.Linq;

public class CombatListener : IDisposable
{
    private CombatEvents eventBus;
    private CombatContext context => CombatManager.Instance.Context;

    public CombatListener(CombatEvents eventBus)
    {
        this.eventBus = eventBus;

        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        eventBus.OnUnitDeath += HandleUnitDeath;
        eventBus.OnWaveStart += HandleWaveStart;
        eventBus.OnAVModified += HandleAVModified;
        eventBus.OnSpeedChanged += HandleSpeedChanged;
    }

    public void Dispose()
    {
        eventBus.OnUnitDeath -= HandleUnitDeath;
        eventBus.OnWaveStart -= HandleWaveStart;
        eventBus.OnAVModified -= HandleAVModified;
        eventBus.OnSpeedChanged -= HandleSpeedChanged;
    }

    private void HandleUnitDeath(Unit unit)
    {
        context.RemoveActiveUnit(unit);
        eventBus.TurnOrderChanged();
    }

    private void HandleWaveStart(List<Unit> activeUnits)
    {
        CombatManager.Instance.CombatUIManager.SetupUI(activeUnits);   
    }

    private void HandleAVModified(Unit unit, float percentage)
    {
        UnitTurnData data = context.TurnData.First(u => u.Unit == unit);
        data.ApplyAVModifier(percentage);
        eventBus.TurnOrderChanged();
    }

    private void HandleSpeedChanged(Unit unit)
    {
        UnitTurnData data = context.TurnData.First(u => u.Unit == unit);
        data.SpeedChanged();
        eventBus.TurnOrderChanged();
    }
}