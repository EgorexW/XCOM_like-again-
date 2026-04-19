using System;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class UnitAction : MonoBehaviour{
    [SerializeField] ActionInfo actionInfo;
    [SerializeField] int cost = 1;
    [SerializeField] Optional<int> usesLeft;

    [HideInEditorMode] [ReadOnly] public CombatUnit unit;
    
    public ActionInfo ActionInfo => actionInfo;

    public void Execute(){
        if (ValidateAction() != UnitActionValidation.Valid){
            Debug.LogWarning($"Cannot execute action {this.ActionInfo.Name} for unit {unit.name}, because {ValidateAction().ToString()}");
            return;
        }
        unit.SpendActionPoints(cost);
        if (usesLeft){
            usesLeft -= 1;
        }
        OnExecute();
        unit.onActionPerformed.Invoke(this);
    }

    protected abstract void OnExecute();
    public virtual float GetCost(){
        return cost;
    }

    public virtual UnitActionValidation ValidateAction(){
        UnitActionValidation result = UnitActionValidation.Valid;
        if (GetUsesLeft().HasValue){
            if (GetUsesLeft() <= 0){
                result |= UnitActionValidation.NoUsesLeft;
            }
        }
        result |= unit.CanExecute(this);
        return result;
    }

    public virtual int? GetUsesLeft(){
        if (!usesLeft){
            return null;
        }
        return usesLeft;
    }
}

[Flags]
public enum UnitActionValidation{
    Valid = 0,                     
    NotEnoughActionPoints = 1 << 0,  
    NoUsesLeft = 1 << 1,             
    SupressedByStatus = 1 << 2,      
    InvalidTarget = 1 << 3,          
    AmmoIssue = 1 << 4
}

[Serializable][HideLabel]
public class ActionInfo{
    [SerializeField] private string actionName;
    [SerializeField] private string description;
    [FormerlySerializedAs("actionType")] [SerializeField] private ActionFlags actionFlags;
    
    public ActionFlags ActionFlags => actionFlags;
    public string Name => actionName;
    public string Description => description;
}

[Flags]
public enum ActionFlags{
    Movement = 1 << 0,
    Shooting  = 1 << 1,
    Utility  = 1 << 2,
    Aggressive   = 1 << 3,
}