using UnityEngine;

public abstract class CombatEffect : MonoBehaviour{
    public ICombatObject sourceObject;
    public CombatGridNode targetNode;

    public abstract void Execute();

    protected bool HasNode => targetNode != null;
    protected bool HasObject => sourceObject != null;
}