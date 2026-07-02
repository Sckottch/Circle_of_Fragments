using System.Collections.Generic;

public class IdleCombatState : ICombatState
{
    public void Enter()
    {
       CombatInitialSetup(); 
    }

    private void CombatInitialSetup()
    {
        CombatData data = CombatManager.Instance.CurrentCombatData;
        CombatSetup setup = CombatManager.Instance.CombatSetup;

        List<PlayableUnit> playerUnits = setup.InitializePlayerUnits(data.playerUnits);

        CombatContext context = new (playerUnits, data.waves);

        CombatManager.Instance.SetCurrentContext(context);
        CombatManager.Instance.ChangeCombatState(CombatState.WaveStart);
    }
}
