using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class CombatEffect : MonoBehaviour{
    public ICombatObject targetObject;
    public CombatGridNode targetNode;
    
    public abstract void Execute();

    protected bool HasNode => targetNode != null;
    protected bool HasObject => targetObject != null;
}