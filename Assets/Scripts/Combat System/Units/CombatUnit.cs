using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CombatUnit : CombatObject{
    [SerializeField] int defaultActionPoints = 2;

    public List<UnitAction> UnitActions => unitActions.Copy();
    public int ActionPoints => actionPoints;

    [SerializeField] [HideInEditorMode] List<UnitAction> unitActions;
    [SerializeField] [HideInEditorMode] int actionPoints;
    
    List<UnitStatusEffect> activeStatuses = new();

    [FoldoutGroup("Events")] public UnityEvent<CombatUnit> onStartTurn;
    [FoldoutGroup("Events")] public UnityEvent<CombatUnit> onEndTurn;
    [FoldoutGroup("Events")] public UnityEvent<UnitAction> onActionPerformed;

    public override void Init(){
        base.Init();
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

    public void SpendActionPoints(int cost){
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

    public UnitActionValidation CanExecute(UnitAction action) {
        var result = UnitActionValidation.Valid;
        if (ActionPoints < action.GetCost()){
            result |= UnitActionValidation.NotEnoughActionPoints;
        }
        foreach (var status in activeStatuses) {
            if (!status.CanExecuteAction(action)){
                result |= UnitActionValidation.SupressedByStatus;
            }
        }
        return result;
    }
}