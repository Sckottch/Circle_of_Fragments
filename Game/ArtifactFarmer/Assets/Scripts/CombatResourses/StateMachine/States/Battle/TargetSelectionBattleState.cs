using System.Collections;
using System.Linq;
using UnityEngine;

public class TargetSelectionBattleState : ICombatState
{
    private BattleCombatState battleState;

    public TargetSelectionBattleState(BattleCombatState battleState)
    {
        this.battleState = battleState;
    }
    
    public void Enter()
    {
        CombatManager.Instance.StartRoutine(SelectTarget());
    }

    private IEnumerator SelectTarget()
    {
        CombatContext context = CombatManager.Instance.Context;
        Unit caster = context.ActiveUnit;
        Skill skill = context.CurrentSkill;
        
        if (!caster.IsPlayer)
        {
            Unit target = context.ActiveUnits
                .Where(u => u.IsPlayer && u.IsAlive())
                .OrderBy(_ => RandomManager.RangeFloat(0, 1))
                .FirstOrDefault();
            context.SetCurrentTarget(target);
            
            battleState.ChangeState(BattleState.ActionResult);
            yield break;
        }

        if (skill.SkillData.isSelf)
        {
            context.SetCurrentTarget(caster);
            battleState.ChangeState(BattleState.ActionResult);
            yield break;
        }

        StartInputHandler();
        context.SetIsWaitingForTarget(true);

        yield return new WaitUntil(() => !context.IsWaitingForTarget);
        StopInputHandler();
        
        battleState.ChangeState(BattleState.ActionResult);
    }

    private void StartInputHandler()
    {
        CombatInputHandler input = CombatManager.Instance.Inputs;
        input.OnTargetSelected += HandleUnitSelected;
        input.StartListening();
    }
    
    private void StopInputHandler()
    {
        CombatInputHandler input = CombatManager.Instance.Inputs;
        input.OnTargetSelected -= HandleUnitSelected;
        input.StopListening();       
    }

    private void HandleUnitSelected(Unit target)
    {
        CombatContext context = CombatManager.Instance.Context;
        if (!TargetSystem.IsValidTarget(target, context.ActiveUnit, context.ActiveUnits,
                context.CurrentSkill.SkillData))
        {
            Debug.Log($"Invalid Target: {target}");
            return;
        }
       
        context.SetCurrentTarget(target);
        context.SetIsWaitingForTarget(false);
    }
}
