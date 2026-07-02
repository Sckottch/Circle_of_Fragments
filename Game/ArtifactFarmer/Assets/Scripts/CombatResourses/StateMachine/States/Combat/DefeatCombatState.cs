public class DefeatCombatState : ICombatState
{
    public void Enter()
    {
        FinishCombat();
    }
    
    private void FinishCombat()
    {
        CombatEndResult result = new() { playerWon = false };
        GameManager.Instance.CombatEnd(result);
    }
}
