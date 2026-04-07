using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class CombatObject : MonoBehaviour, ICombatObject{
    [SerializeField] bool occupiesTile = true;

    public CombatGridNode Node{ get; set; }
    public CombatSystem CombatSystem{ get; set; }
    public GameObject GameObject => gameObject;
    public bool OccupiesTile => occupiesTile;

    [FoldoutGroup("Events")] public UnityEvent<ICombatObject> onRemove{ get; } = new();
    [FoldoutGroup("Events")] public UnityEvent onInit{ get; } = new();
    public string Name => name;

    public T GetCombatComponent<T>() where T : CombatComponent{
        var component = GetComponentInChildren<T>();
        return component;
    }

    public void MoveTo(CombatGridNode targetNode){
        targetNode.grid.PlaceCombatObject(this, targetNode);
        transform.position = new Vector3(targetNode.x, targetNode.y, transform.position.z);
    }

    public void Remove(){
        CombatSystem.RemoveCombatObject(this);
        Destroy(gameObject);
    }

    public void Init(){
        foreach (var combatComponent in GetComponentsInChildren<CombatComponent>()){
            combatComponent.combatObject = this;
            combatComponent.Init();
        }
    }
}

public interface ICombatObject{
    CombatGridNode Node{ get; set; }
    CombatSystem CombatSystem { get; set; }
    GameObject GameObject { get; }
    bool OccupiesTile{ get; }
    T GetCombatComponent<T>() where T : CombatComponent;
    string Name{ get; }
    public void MoveTo(CombatGridNode targetNode);
    public void Remove();
    void Init();
}

public abstract class CombatComponent : MonoBehaviour{
    public ICombatObject combatObject;

    public virtual void Init(){ }
}

public static class CombatObjectExtensions{
    public static CombatGrid Grid(this ICombatObject combatObject){
        return combatObject.Node.grid;
    }
    public static Vector3 WorldPosition(this ICombatObject combatObject){
        return combatObject.GameObject.transform.position;
    }
}