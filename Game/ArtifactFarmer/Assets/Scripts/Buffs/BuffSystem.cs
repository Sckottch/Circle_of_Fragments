using System;
using System.Collections.Generic;
using System.Linq;

public class BuffSystem : IDisposable
{
    private CombatEvents eventBus;

    public BuffSystem(CombatEvents events)
    {
        eventBus = events;

        eventBus.OnTurnStart += HandleTurnStart;
        eventBus.OnTurnEnd += HandleTurnEnd;
        eventBus.OnBuffApplied += HandleBuffApplied;
        eventBus.OnBuffRemoved += HandleBuffRemoved;
        eventBus.OnUnitDeath += HandleUnitDeath;
    }

    public void Dispose()
    {
        eventBus.OnTurnStart -= HandleTurnStart;
        eventBus.OnTurnEnd -= HandleTurnEnd;
        eventBus.OnBuffApplied -= HandleBuffApplied;
        eventBus.OnBuffRemoved -= HandleBuffRemoved;
        eventBus.OnUnitDeath -= HandleUnitDeath;
    }

    private void HandleTurnStart(Unit unit)
    {
        foreach (Buff buff in unit.ActiveBuffs)
        {
            buff.Tick();
        }

        List<Buff> buffs = unit.ActiveBuffs.FindAll(b => b.actionTime == BuffActionTime.OnTurnStart);
        foreach (Buff buff in buffs)
        {
            buff.ApplyEffect();
        }
    }
    
    private void HandleTurnEnd(Unit unit)
    {
        List<Buff> buffs = unit.ActiveBuffs.FindAll(b => b.actionTime == BuffActionTime.OnTurnEnd);
        foreach (Buff buff in buffs)
        {
            buff.ApplyEffect();
        }

        foreach (Buff buff in unit.ActiveBuffs) buff.RemoveExpiredStacks();

        List<Buff> buffsToRemove = unit.ActiveBuffs.FindAll(b => b.IsExpired());
        foreach (Buff buff in buffsToRemove)
        {
            unit.RemoveBuff(buff);
        }
    }

    private void HandleBuffApplied(Unit target, Buff buff, Unit caster)
    {
        if (!target.ActiveBuffs.Any(b => b.buffName == buff.buffName))
        {
            Buff newBuff = UnityEngine.Object.Instantiate(buff);
            target.ActiveBuffs.Add(newBuff);
            buff.OnApply(target, caster);
            return;
        }

        Buff activeBuff = target.ActiveBuffs.Find(b => b.buffName == buff.buffName);
        activeBuff.OnApply(target, caster);
    }

    private void HandleBuffRemoved(Unit target, Buff buff)
    {
        buff.OnRemove();
        target.ActiveBuffs.Remove(buff);
    }

    private void HandleUnitDeath(Unit unit)
    {
        foreach (Buff buff in unit.ActiveBuffs) buff.OnRemove();
        unit.ActiveBuffs.Clear();
    }
}
