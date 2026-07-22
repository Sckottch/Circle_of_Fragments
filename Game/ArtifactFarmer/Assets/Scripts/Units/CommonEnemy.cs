using UnityEngine;

public class CommonEnemy : EnemyUnit
{
    public override EnemyCategory Category => EnemyCategory.Common;
    protected CommonEnemyDataSO EnemyData => UnitData as CommonEnemyDataSO;

    public override void Initialize(UnitBaseData unitData, int level = 1)
    {
        if (unitData is not CommonEnemyDataSO data)
        {
            Debug.LogError("Unit data is not of the correct Type");
            return;
        }
        UnitData = data;

        UnitName = EnemyData.unitName;

        baseStats = EnemyData.baseStats;
        baseSpecialStats = EnemyData.baseSpecialStats;

        ElementalProfile = new ElementalProfile(EnemyData.element, EnemyData.elementalWeaknesses,
            EnemyData.elementalResistance);

        view.Initialize(this);

        lastHealth = baseStats.health;
        CurrentHealth = baseStats.health;
        CurrentMana = baseSpecialStats.mana;

        skills.Add(new Skill(this, EnemyData.enemySkill));
    }

    public override Skill GetSkill(ActionType type)
    {
        if (type != ActionType.Enemy)
        {
            Debug.LogError("tipo inválido para skill de inimigo");
            return null;
        }
        return skills[0];
    }
}
