using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class UnitAction : MonoBehaviour{
    [SerializeField] public string Name;
    [SerializeField] float cost = 1;

    [HideInEditorMode][ReadOnly] public CombatUnit unit;

    public virtual float GetCost(){
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

    protected virtual bool CanExecute(){
        if (unit.ActionPoints < cost){
            Debug.Log($"Cannot execute action {Name} for unit {unit.name}, not enough action points. Current AP: {unit.ActionPoints}, required AP: {cost}");
            return false;
        }
        return true;
    }

    [Button]
    void ResetName(){
        Name = gameObject.name;
    }

    void Reset(){
        ResetName();
    }
}