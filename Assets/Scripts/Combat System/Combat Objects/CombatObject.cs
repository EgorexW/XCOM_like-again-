using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class CombatObject : MonoBehaviour, ICombatObject{
    [SerializeField] CombatObjectFlags flags = CombatObjectFlags.None;

    [ShowInInspector][HideInEditorMode][FoldoutGroup("Debug")] public List<CombatGridNode> Nodes{ get; set; } = new List<CombatGridNode>();
    [ShowInInspector][HideInEditorMode][FoldoutGroup("Debug")] public CombatSystem CombatSystem{ get; set; }
    public GameObject GameObject => gameObject;
    public CombatObjectFlags Flags => flags;
    
    public string Name => name;

    [FoldoutGroup("Events")] public UnityEvent<ICombatObject> onRemove{ get; } = new();

    [FoldoutGroup("Events")] public UnityEvent<ICombatObject> onInit{ get; } = new();

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

    public virtual void Init(){
        foreach (var combatComponent in GetComponentsInChildren<CombatComponent>()){
            combatComponent.CombatObject = this;
            combatComponent.Init();
        }
        this.Validate();
        onInit.Invoke(this);
    }

    void OnValidate(){
        this.Validate();
    }
}