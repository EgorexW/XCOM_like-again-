using UnityEngine;

public abstract class UnitAction : MonoBehaviour{
    public CombatUnit unit;
    
    [SerializeField] int cost = 1;
    
    public string Name => gameObject.name;

    public virtual int GetCost(){
        return cost;
    }

    public void Execute(){
        if (!CanExecute()){
            Debug.LogWarning($"Cannot execute action {Name} for unit {unit.name}");
            return;
        }
        OnExecute();
        unit.SpendActionPoints(cost);
    }

    protected abstract void OnExecute();
    public abstract void SetTarget(Vector2 pos);

    protected virtual bool CanExecute(){
        if (unit.ActionPoints < cost){
            Debug.Log($"Cannot execute action {Name} for unit {unit.name}, not enough action points. Current AP: {unit.ActionPoints}, required AP: {cost}");
            return false;
        }
        if (!HasValidTarget()){
            Debug.Log($"Cannot execute action {Name} for unit {unit.name}, invalid target.");
            return false;
        }
        return true;
    }

    protected virtual bool HasValidTarget(){
        return true;
    }
}