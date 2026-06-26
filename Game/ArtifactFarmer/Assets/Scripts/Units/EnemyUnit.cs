using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyUnit : Unit
{
    public abstract EnemyCategory Category { get; }
    public override bool IsPlayer => false;

    public List<Element> ElementalWeaknesses { get; protected set; }
    public Element ElementalResistance { get; protected set; }
}

public enum EnemyCategory
{
    Common,
    Elite,
    Boss
}