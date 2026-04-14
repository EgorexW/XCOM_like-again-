using System;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class UnitAction : MonoBehaviour{
    [FormerlySerializedAs("Name")] [SerializeField] new string name;
    [SerializeField] string description;
    [SerializeField] int cost = 1;
    [SerializeField] Optional<int> usesLeft;
    [SerializeField] ActionType actionType;

    [HideInEditorMode] [ReadOnly] public CombatUnit unit;
    
    public ActionType ActionType => actionType;
    public string Description => description;
    public string Name => name;

    public void Execute(){
        if (ValidateAction() != UnitActionValidation.Valid){
            Debug.LogWarning($"Cannot execute action {this.Name} for unit {unit.name}, because {ValidateAction().ToString()}");
            return;
        }
        unit.SpendActionPoints(cost);
        if (usesLeft){
            usesLeft -= 1;
        }
        OnExecute();
    }

    protected abstract void OnExecute();
    public virtual float GetCost(){
        return cost;
    }

    public virtual UnitActionValidation ValidateAction(){
        UnitActionValidation result = UnitActionValidation.Valid;
        if (IsLimitedUse()){
            if (GetUsesLeft() <= 0){
                result |= UnitActionValidation.NoUsesLeft;
            }
        }
        result |= unit.CanExecute(this);
        return result;
    }
    
    [Button]
    void ResetName(){
        this.name = gameObject.name;
    }

    protected void Reset(){
        ResetName();
    }

    public virtual bool IsLimitedUse(){
        return GetUsesLeft() > -1;
    }

    public virtual int GetUsesLeft(){
        if (!usesLeft){
            return -1;
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

public enum ActionType{
    Movement,
    Shooting,
    Utility,
    Other
}