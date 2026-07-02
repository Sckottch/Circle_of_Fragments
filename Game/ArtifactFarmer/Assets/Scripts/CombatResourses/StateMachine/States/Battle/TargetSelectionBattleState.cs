using System.Collections;
using System.Collections.Generic;
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
        
        context.SetIsWaitingForTarget(true);

        SubscribeActiveUnits(context);

        yield return new WaitUntil(() => !context.IsWaitingForTarget);
        
        UnsubscribeActiveUnits(context);
        battleState.ChangeState(BattleState.ActionResult);
    }

    private void SubscribeActiveUnits(CombatContext context)
    {
        List<Unit> validTargets =
            TargetSystem.GetValidTargets(context.ActiveUnit, context.ActiveUnits, context.CurrentSkill.SkillData);
        
        foreach (Unit unit in validTargets)
        {
            unit.OnUnitSelected += HandleUnitSelected;
        }
    }

    private void UnsubscribeActiveUnits(CombatContext context)
    {
        
        List<Unit> validTargets =
            TargetSystem.GetValidTargets(context.ActiveUnit, context.ActiveUnits, context.CurrentSkill.SkillData);
        
        foreach (Unit unit in validTargets)
        {
            unit.OnUnitSelected -= HandleUnitSelected;
        }
    }

    private void HandleUnitSelected(Unit target)
    {
        CombatContext context = CombatManager.Instance.Context;
       
        context.SetCurrentTarget(target);
        context.SetIsWaitingForTarget(false);
    }
}
