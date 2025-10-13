using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CommonEnemy : EnemyUnit
{
    public override EnemyCategory Category => EnemyCategory.Common;
    protected CommonEnemyDataSO EnemyData => UnitData as CommonEnemyDataSO;

    private Skill skill;

    public override void Initialize(UnitBaseData unitData, int level = 1)
    {
        if (unitData is not CommonEnemyDataSO data)
        {
            Debug.LogError("Unit data is not of the correct Type");
            return;
        }
        UnitData = data;

        this.UnitName = EnemyData.unitName;

        this.baseStats = EnemyData.baseStats;
        this.baseSpecialStats = EnemyData.baseSpecialStats;

        this.ElementalWeaknesses = EnemyData.elementalWeaknesses;
        this.ElementalResistance = EnemyData.elementalResistance;

        this.spriteRenderer.sprite = EnemyData.unitSprite;

        this.lastHealth = this.baseStats.health;
        this.CurrentHealth = this.baseStats.health;
        this.CurrentMana = this.baseSpecialStats.mana;

        skill = new Skill(this, EnemyData.enemySkill);
    }

    public override bool TurnAction(Unit mainTarget, List<Unit> allTargets, ActionType action)
    {
        if (skill == null)
        {
            Debug.LogError($"No Skill set for {UnitName}");
            return false;
        }

        if (skill.HasEnoughMana())
        {
            skill.Execute(mainTarget, allTargets);
            return true;
        }
        else
        {
            Debug.Log($"{UnitName} does not have enough mana to use the skill.");
            return false;
        }
    }

}
