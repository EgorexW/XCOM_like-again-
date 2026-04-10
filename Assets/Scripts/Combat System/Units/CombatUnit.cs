using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class CombatUnit : CombatObject{
    [SerializeField] float defaultActionPoints = 2;

    public List<UnitAction> UnitActions => unitActions.Copy();
    public float ActionPoints => actionPoints;

    [SerializeField] [HideInEditorMode] List<UnitAction> unitActions;
    [SerializeField] [HideInEditorMode] float actionPoints;
    
    List<UnitStatusEffect> activeStatuses = new();

    [FoldoutGroup("Events")] public UnityEvent<CombatUnit> onStartTurn;
    [FoldoutGroup("Events")] public UnityEvent<CombatUnit> onEndTurn;

    protected override void Awake(){
        base.Awake();
        unitActions = GetComponentsInChildren<UnitAction>().ToList();
        foreach (var action in unitActions) action.unit = this;
    }

    public void OnStartTurn(){
        actionPoints = defaultActionPoints;
        onStartTurn.Invoke(this);
    }

    public void OnEndTurn(){
        actionPoints = 0;
        onEndTurn.Invoke(this);
    }

    public void SpendActionPoints(float cost){
        if (cost > actionPoints){
            Debug.LogWarning(
                $"Unit {name} does not have enough action points to spend {cost}. Current AP: {actionPoints}");
            actionPoints = 0;
            return;
        }
        actionPoints -= cost;
    }
    
    public void ApplyStatus(UnitStatusEffect status) {
        activeStatuses.Add(status);
        status.OnApplied(this); 
    }

    public void RemoveStatus(UnitStatusEffect status) {
        if (activeStatuses.Contains(status)) {
            status.OnRemoved(); // Clean up listeners!
            activeStatuses.Remove(status);
        }
    }

    public bool CanExecute(UnitAction action) {
        if (ActionPoints < action.GetCost()){
            Debug.Log(
                $"Cannot execute action {action.name} for unit {name}, not enough action points. Current AP: {ActionPoints}, required AP: {action.GetCost()}");
            return false;
        }
        foreach (var status in activeStatuses) {
            if (!status.CanExecuteAction(action)){
                return false;
            }
        }
        return true;
    }
}