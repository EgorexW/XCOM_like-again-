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

    [FoldoutGroup("Events")] public UnityEvent<ICombatObject> onInit{ get; } = new();

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
            combatComponent.CombatObject = this;
            combatComponent.Init();
        }
        onInit.Invoke(this);
    }
}

public abstract class CombatComponent : MonoBehaviour{
    public ICombatObject CombatObject { get; set; }

    public virtual void Init(){ }
}