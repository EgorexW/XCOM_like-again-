using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class CombatSystem : MonoBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] CombatGrid combatGrid;
    [BoxGroup("References")] [Required] [SerializeField] TurnSystem turnSystem;
    [BoxGroup("References")] [Required] [SerializeField] TeamsSystem teamsSystem;

    readonly List<ICombatObject> combatObjects = new();

    [FoldoutGroup("Events")] public UnityEvent onCombatStarted;
    [FoldoutGroup("Events")] public UnityEvent onCombatEnded;
    
    [FoldoutGroup("Events")] public UnityEvent<ICombatObject> onCombatObjectAdded;
    [FoldoutGroup("Events")] public UnityEvent<ICombatObject> onCombatObjectRemoved;

    [FoldoutGroup("Events")] public UnityEvent onStateChanged; 

    public IReadOnlyList<ICombatObject> CombatObjects => combatObjects.AsReadOnly();
    public CombatGrid CombatGrid => combatGrid;
    public TurnSystem TurnSystem => turnSystem;
    public TeamsSystem TeamsSystem => teamsSystem;

    bool active;

    void Awake(){
        onCombatObjectAdded.AddListener(_ => StateChanged());
        onCombatObjectRemoved.AddListener(_ => StateChanged());
        turnSystem.onEndTurn.AddListener(_ => StateChanged());
    }

    public void AddCombatObject(ICombatObject combatObject, List<CombatGridNode> targetNode){
        Assert.IsNotNull(targetNode, $"Node cannot be null when adding a combat object. {combatObject}");
        Assert.IsNotNull(combatObject, $"Combat object cannot be null when adding to combat system. {targetNode}");
        combatObjects.Add(combatObject);
        combatObject.MoveTo(targetNode);
        combatObject.CombatSystem = this;
        combatObject.Init();
        onCombatObjectAdded.Invoke(combatObject);
    }

    public void RemoveCombatObject(ICombatObject arg0){
        combatObjects.Remove(arg0);
        combatGrid.RemoveCombatObject(arg0);
        onCombatObjectRemoved.Invoke(arg0);
    }

    public void StartCombat(){
        active = true;
        turnSystem.NextTurn();
        onCombatStarted.Invoke();
    }

    public void EndCombat(){
        active = false;
        onCombatEnded.Invoke();
        turnSystem.Stop();
    }

    public void StateChanged(){
        if (!active){
            return;
        }
        Debug.Log("Combat state changed!", this);
        onStateChanged.Invoke();
    }
}