using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class UnitAction : MonoBehaviour{
    [FormerlySerializedAs("Name")] [SerializeField] public new string name;
    [SerializeField] string description;
    [SerializeField] float cost = 1;
    [SerializeField] Optional<int> usesLeft;
    [SerializeField] ActionType actionType;

    [HideInEditorMode] [ReadOnly] public CombatUnit unit;

    public int UsesLeft => usesLeft ? usesLeft : 1;
    public bool LimitedUses => usesLeft;
    public ActionType ActionType => actionType;
    
    public void Execute(){
        if (CanExecute() != UnitActionValidation.Success){
            Debug.LogWarning($"Cannot execute action {this.name} for unit {unit.name}, because {CanExecute().ToString()}");
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

    public virtual UnitActionValidation CanExecute(){
        if (usesLeft){
            if (usesLeft <= 0){
                return UnitActionValidation.NoUsesLeft;
            }
        }
        return unit.CanExecute(this);
    }
    

    public virtual string GetDescription(){
        return Descriptions.GetActionDescription(this, description);
    }
    [Button]
    void ResetName(){
        this.name = gameObject.name;
    }

    protected void Reset(){
        ResetName();
    }
}

public enum UnitActionValidation{
    Success,
    NotEnoughActionPoints,
    NoUsesLeft,
    SupressedByStatus,
    InvalidTarget
}

public enum ActionType{
    Movement,
    Shooting,
    Utility
}