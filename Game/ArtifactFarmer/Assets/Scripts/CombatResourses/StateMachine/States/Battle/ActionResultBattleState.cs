using System.Collections;
using System.Linq;
using UnityEngine;

public class ActionResultBattleState : ICombatState
{
    private BattleCombatState battleState;

    public ActionResultBattleState(BattleCombatState battleState)
    {
        this.battleState = battleState;
    }

    public void Enter()
    {
        CombatManager.Instance.StartRoutine(ResolveAction());
    }

    private IEnumerator ResolveAction()
    {
        CombatContext context = CombatManager.Instance.Context;

        Skill skill = context.CurrentSkill;
        Unit target = context.CurrentTarget;

        yield return new WaitForSeconds(0.4f); // breve pausa para simular animação, separado em duas fases para simular momento do hit

        skill.Execute(target, context.ActiveUnits.ToList());

        yield return new WaitForSeconds(0.1f);
        
        battleState.ChangeState(BattleState.TurnEnd);
    }
}
