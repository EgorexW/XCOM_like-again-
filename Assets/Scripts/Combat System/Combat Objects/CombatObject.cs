using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class CombatObject : MonoBehaviour, ICombatObject{
    [SerializeField] GridOccupancyType gridOccupancyType;

    [ShowInInspector][HideInEditorMode][FoldoutGroup("Debug")] public List<CombatGridNode> Nodes{ get; set; } = new List<CombatGridNode>();
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

    public void MoveTo(List<CombatGridNode> targetNodes){
        targetNodes.PrimaryGrid().PlaceCombatObject(this, targetNodes);
        transform.position = targetNodes.GetCenter();
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
}

public interface ICombatObject{
    List<CombatGridNode> Nodes{ get; set; }
    CombatSystem CombatSystem { get; set; }
    GameObject GameObject { get; }
    GridOccupancyType OccupancyType { get; }
    T GetCombatComponent<T>() where T : CombatComponent;
    string Name{ get; }
    void MoveTo(List<CombatGridNode> targetNodes);
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
        var nodes = combatObject.Nodes;
        return nodes.PrimaryGrid();
    }

    public static Vector3 WorldPosition(this ICombatObject combatObject){
        return combatObject.GameObject.transform.position;
    }
    public static void MoveTo(this ICombatObject combatObject, CombatGridNode targetNode){
        combatObject.MoveTo(new List<CombatGridNode>{targetNode});
    }

    public static Vector2 GetCenter(this ICombatObject combatObject){
        return combatObject.Nodes.GetCenter();
    }
    public static CombatGridNode GetCenterNode(this ICombatObject combatObject){
        return combatObject.Nodes.GetCenterNode();
    }
}