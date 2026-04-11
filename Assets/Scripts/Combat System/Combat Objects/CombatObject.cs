using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class CombatObject : MonoBehaviour, ICombatObject{
    [SerializeField] GridOccupancyType gridOccupancyType;

    [ShowInInspector][HideInEditorMode][FoldoutGroup("Debug")] public CombatGridNode Node{ get; set; }
    [ShowInInspector][HideInEditorMode][FoldoutGroup("Debug")] public CombatSystem CombatSystem{ get; set; }
    public GameObject GameObject => gameObject;
    public GridOccupancyType OccupancyType => gridOccupancyType;
    
    public string Name => name;

    [FoldoutGroup("Events")] public UnityEvent<ICombatObject> onRemove{ get; } = new();

    protected virtual void Awake(){}

    public T GetCombatComponent<T>() where T : CombatComponent{
        var component = GetComponentInChildren<T>();
        return component;
    }

    public void MoveTo(CombatGridNode targetNode){
        targetNode.grid.PlaceCombatObject(this, targetNode);
        transform.position = new Vector3(targetNode.x, targetNode.y, transform.position.z);
    }

    public virtual void Remove(){
        CombatSystem.RemoveCombatObject(this);
        onRemove.Invoke(this);
        gameObject.SetActive(false);
    }

    public void Init(){
        foreach (var combatComponent in GetComponentsInChildren<CombatComponent>()){
            combatComponent.combatObject = this;
            combatComponent.Init();
        }
    }

    protected void OnDisable(){
        Remove();
    }
}

public interface ICombatObject{
    CombatGridNode Node{ get; set; }
    CombatSystem CombatSystem { get; set; }
    GameObject GameObject { get; }
    GridOccupancyType OccupancyType { get; }
    T GetCombatComponent<T>() where T : CombatComponent;
    string Name{ get; }
    public void MoveTo(CombatGridNode targetNode);
    public void Remove();
    void Init();
    UnityEvent<ICombatObject> onRemove{ get; }
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