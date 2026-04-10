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
    public bool NoUsesLeft => usesLeft && usesLeft <= 0;
    public ActionType ActionType => actionType;
    
    public void Execute(){
        if (!CanExecute()){
            Debug.LogWarning($"Cannot execute action {this.name} for unit {unit.name}");
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

    protected virtual bool CanExecute(){
        if (usesLeft){
            if (usesLeft <= 0){
                Debug.Log(
                    $"Cannot execute action {name} for unit {unit.name}, no uses left. Current uses: {usesLeft}");
                return false;
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

public enum ActionType{
    Movement,
    Shooting,
    Utility
}