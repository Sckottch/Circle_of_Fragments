using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class UnitView : MonoBehaviour
{
    private Unit unit;

    public void Initialize(Unit unit)
    {
        this.unit = unit;
    }
}