using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class CombatObject : MonoBehaviour, ICombatObject{
    [SerializeField] bool occupiesTile = true;
    
    public CombatGridNode Node{ get; set; }
    public GameObject GameObject => gameObject;
    public bool OccupiesTile => occupiesTile;
    public CombatGrid Grid => Node.grid;

    [FoldoutGroup("Events")] public UnityEvent<ICombatObject> onRemove{ get; } = new();

    void Awake(){
        foreach (var combatComponent in GetComponentsInChildren<CombatComponent>()){
            combatComponent.combatObject = this;
        }
    }

    public T GetCombatComponent<T>() where T : CombatComponent{
        var component = GetComponentInChildren<T>();
        if (component != null){
            component.combatObject = this;
        }
        return component;
    }
    public void MoveTo(CombatGridNode targetPos){
        Node.grid.PlaceCombatObject(this, targetPos.GetPos());
        transform.position = new Vector3(targetPos.x, targetPos.y, transform.position.z);
    }

    public void Remove(){
        onRemove?.Invoke(this);
        Destroy(gameObject);
    }
}

public interface ICombatObject{
    CombatGridNode Node{ get; set; }
    GameObject GameObject{ get; }
    bool OccupiesTile{ get; }
    T GetCombatComponent<T>() where T : CombatComponent;
    public UnityEvent<ICombatObject> onRemove{ get; }

}

public abstract class CombatComponent : MonoBehaviour{
    [HideInEditorMode][ReadOnly] public CombatObject combatObject;
}