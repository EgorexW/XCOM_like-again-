using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnit : CombatObject{
    [SerializeField] int defaultActionPoints = 2;
    [SerializeField] List<UnitAction> unitActions;

    public List<UnitAction> UnitActions => unitActions.Copy();
    public float ActionPoints => actionPoints;

    int actionPoints;
    
    void Awake(){
        foreach (var action in unitActions){
            action.unit = this;
        }
    }

    public void OnStartTurn(){
        actionPoints = defaultActionPoints;
        Debug.Log($"Unit {name} starts turn with {actionPoints} action points.");
    }

    public void OnEndTurn(){ }

    void Reset(){
        unitActions = new List<UnitAction>(GetComponents<UnitAction>());
    }

    public void SpendActionPoints(int cost){
        if (cost > actionPoints){
            Debug.LogWarning($"Unit {name} does not have enough action points to spend {cost}. Current AP: {actionPoints}");
            actionPoints = 0;
            return;
        }
        actionPoints -= cost;
    }
}