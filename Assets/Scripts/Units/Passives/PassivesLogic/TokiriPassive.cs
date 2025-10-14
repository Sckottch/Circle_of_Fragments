using UnityEngine;

[CreateAssetMenu(fileName = "TokiriPassive", menuName = "ScriptableObjects/Passives/Tokiri Passive")]
public class TokiriPassive : PassiveBase
{
    [Space(10)]
    [Header("Tokiri Passive Values")]
    [SerializeField] private int actionValueAdvance;

    public override void Initialize(Unit unit)
    {
        ownerUnit = unit;
        ownerUnit.OnHit += HandleOnHit;
    }

    private void HandleOnHit(Unit unit)
    {
        ownerUnit.ModifyActionValue(actionValueAdvance);
        Debug.Log($"{ownerUnit.UnitData.unitName} was hit and gained {actionValueAdvance} action value from Tokiri's passive.");
    }

        public override void CleanUp()
    {
        ownerUnit.OnHit -= HandleOnHit;
    }
}
