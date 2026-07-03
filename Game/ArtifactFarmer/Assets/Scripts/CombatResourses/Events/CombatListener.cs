using System;
using System.Collections.Generic;

public class CombatListener : IDisposable
{
    private CombatEvents eventBus;

    public CombatListener(CombatEvents eventBus)
    {
        this.eventBus = eventBus;

        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        eventBus.OnUnitDeath += HandleUnitDeath;
        eventBus.OnWaveStart += HandleWaveStart;
    }

    public void Dispose()
    {
        eventBus.OnUnitDeath -= HandleUnitDeath;
        eventBus.OnWaveStart -= HandleWaveStart;
    }

    private void HandleUnitDeath(Unit unit)
    {
        CombatManager.Instance.Context.RemoveActiveUnit(unit);
        eventBus.TurnOrderChanged();
    }

    private void HandleWaveStart(List<Unit> activeUnits)
    {
        CombatManager.Instance.CombatUIManager.SetupUI(activeUnits);   
    }
}