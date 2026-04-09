using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class UnitAction : MonoBehaviour{
    [FormerlySerializedAs("Name")] [SerializeField] public new string name;
    [SerializeField] string description;
    [SerializeField] float cost = 1;

    [HideInEditorMode] [ReadOnly] public CombatUnit unit;
    
    public void Execute(){
        if (!CanExecute()){
            Debug.LogWarning($"Cannot execute action {this.name} for unit {unit.name}");
            return;
        }
        OnExecute();
        unit.SpendActionPoints(cost);
    }

    protected abstract void OnExecute();
    public virtual float GetCost(){
        return cost;
    }

    protected virtual bool CanExecute(){
        if (unit.ActionPoints < cost){
            Debug.Log(
                $"Cannot execute action {name} for unit {unit.name}, not enough action points. Current AP: {unit.ActionPoints}, required AP: {cost}");
            return false;
        }
        return true;
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