using System.Linq;

public static class TurnService
{
    public static UnitTurnData GetNextUnitToAct(CombatContext context)
    {
        if (context.TurnData.Count(u => u.CurrentActionValue <= 0) == 0)
        {
            Tick(context);
        }

        return context.TurnData.FirstOrDefault(u => u.CurrentActionValue <= 0);
    }

    private static void Tick(CombatContext context)
    {
        float tickAmount = context.TurnData.Min(u => u.CurrentActionValue);
        
        foreach (UnitTurnData data in context.TurnData)
        {
            data.ReduceActionValue(tickAmount);
        }

        context.AddTurnActionValue(tickAmount);
    }
}