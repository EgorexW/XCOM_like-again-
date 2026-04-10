using System;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class SupressStatusCreator : UnitStatusEffectCreator{
    [SerializeField] List<ActionType> supressedActionTypes;
    [SerializeField] Optional<int> duration;
    
    public override UnitStatusEffect CreateStatus(){
        int durationValue = duration ? duration.Value : -1;
        var status = new SupressStatus(supressedActionTypes, durationValue);
        return status;
    }
}

public class SupressStatus : UnitStatusEffect{
    List<ActionType> supressedActionTypes;
    int duration;

    public SupressStatus(List<ActionType> actionTypes, int durationTmp){
        supressedActionTypes = actionTypes;
        duration = durationTmp;
    }

    public override void OnApplied(CombatUnit targetTmp){
        base.OnApplied(targetTmp);
        if (duration > 0){
            target.onEndTurn.AddListener(OnEndTurn);
        }
    }

    void OnEndTurn(CombatUnit arg0){
            duration -= 1;
            if (duration <= 0){
                target.RemoveStatus(this);
            }
    }

    public override void OnRemoved(){
        base.OnRemoved();
        target.onEndTurn.RemoveListener(OnEndTurn);
    }

    public override bool CanExecuteAction(UnitAction action){
        if (supressedActionTypes.Contains(action.ActionType)){
            Debug.Log($"Action {action.name} of type {action.ActionType} is supressed for unit {target.name} due to status {ToString()}");
            return false;
        }
        return true;
    }
}