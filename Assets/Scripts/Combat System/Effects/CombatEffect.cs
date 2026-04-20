using UnityEngine;

public abstract class CombatEffect : MonoBehaviour{
    public ICombatObject targetObject;
    public CombatGridNode targetNode;

    public abstract void Execute();

    protected bool HasNode => targetNode != null;
    protected bool HasObject => targetObject != null;
}