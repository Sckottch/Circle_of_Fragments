public class VictoryCombatState : ICombatState
{
    public void Enter()
    {
        FinishCombat();
    }

    private void FinishCombat()
    {
        CombatEndResult result = new() { playerWon = true };
        GameManager.Instance.CombatEnd(result);
    }
}
