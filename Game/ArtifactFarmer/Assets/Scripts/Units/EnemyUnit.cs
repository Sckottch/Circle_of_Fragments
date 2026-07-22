public abstract class EnemyUnit : Unit
{
    public abstract EnemyCategory Category { get; }
    public override bool IsPlayer => false;
}

public enum EnemyCategory
{
    Common,
    Elite,
    Boss
}