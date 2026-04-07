using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class CombatObject : MonoBehaviour, ICombatObject{
    [SerializeField] bool occupiesTile = true;

    public CombatGridNode Node{ get; set; }
    public Vector3 WorldPosition => transform.position;
    public bool OccupiesTile => occupiesTile;

    [FoldoutGroup("Events")] public UnityEvent<ICombatObject> onRemove{ get; } = new();
    [FoldoutGroup("Events")] public UnityEvent onInit{ get; } = new();
    public string Name => name;

    protected virtual void Awake(){
        foreach (var combatComponent in GetComponentsInChildren<CombatComponent>()) combatComponent.combatObject = this;
    }

    public T GetCombatComponent<T>() where T : CombatComponent{
        var component = GetComponentInChildren<T>();
        return component;
    }

    public void MoveTo(CombatGridNode targetNode){
        targetNode.grid.PlaceCombatObject(this, targetNode.GetPos());
        transform.position = new Vector3(targetNode.x, targetNode.y, transform.position.z);
    }

    public void Remove(){
        onRemove?.Invoke(this);
        Destroy(gameObject);
    }
}

public interface ICombatObject{
    CombatGridNode Node{ get; set; }
    Vector3 WorldPosition{ get; }
    bool OccupiesTile{ get; }
    T GetCombatComponent<T>() where T : CombatComponent;
    public UnityEvent<ICombatObject> onRemove{ get; }
    public UnityEvent onInit{ get; }
    string Name{ get; }
    public void MoveTo(CombatGridNode targetNode);
    public void Remove();
}

public abstract class CombatComponent : MonoBehaviour{
    public ICombatObject combatObject;
}

public static class CombatObjectExtensions{
    public static CombatGrid Grid(this ICombatObject combatObject){
        return combatObject.Node.grid;
    }
}