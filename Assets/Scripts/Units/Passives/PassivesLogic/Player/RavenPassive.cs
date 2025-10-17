using UnityEngine;

[CreateAssetMenu(fileName = "RavenPassive", menuName = "ScriptableObjects/Passives/Raven Passive")]
public class RavenPassive : PassiveBase
{
    [SerializeField] private int lifeStealPercentage;
    private float maxLifeStealAmount;

    public override void Initialize(Unit unit)
    {
        ownerUnit = unit;
        ownerUnit.OnSkillResult += HandleSkillResult;
        ownerUnit.OnMaxHealthChanged += UpdateMaxLifestealAmount;

        UpdateMaxLifestealAmount(ownerUnit);
    }

    private void UpdateMaxLifestealAmount(Unit unit)
    {
        maxLifeStealAmount = unit.GetStats().health / 4;
    }

    private void HandleSkillResult(Unit unit, SkillResult result)
    {
        if(result.DamageDealt > 0)
        {
            float lifeStealAmount = result.DamageDealt * lifeStealPercentage / 100f;

            if (lifeStealAmount > maxLifeStealAmount)
            {
                lifeStealAmount = maxLifeStealAmount;
            }

            ownerUnit.Heal(lifeStealAmount);
            Debug.Log($"{ownerUnit.UnitData.unitName} healed for {lifeStealAmount} health from Raven's passive.");
        }
    }
    
    public override void CleanUp()
    {
        ownerUnit.OnSkillResult -= HandleSkillResult;
        ownerUnit.OnMaxHealthChanged -= UpdateMaxLifestealAmount;
    }
}