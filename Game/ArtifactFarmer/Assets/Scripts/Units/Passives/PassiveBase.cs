using UnityEngine;

public abstract class PassiveBase : ScriptableObject
{
    protected Unit ownerUnit;

    [Space(10)]
    [Header("Passive Info")]
    public string passiveName;
    [TextArea]
    public string description;
    public Sprite icon;

    public abstract void Initialize(Unit unit);

    public abstract void CleanUp();
}
