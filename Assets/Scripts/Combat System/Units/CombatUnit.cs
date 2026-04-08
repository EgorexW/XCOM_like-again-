using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class CombatUnit : CombatObject{
    [SerializeField] float defaultActionPoints = 2;

    public List<UnitAction> UnitActions => unitActions.Copy();
    public float ActionPoints => actionPoints;

    [SerializeField] [HideInEditorMode] List<UnitAction> unitActions;
    [SerializeField] [HideInEditorMode] float actionPoints;

    protected override void Awake(){
        base.Awake();
        unitActions = GetComponentsInChildren<UnitAction>().ToList();
        foreach (var action in unitActions) action.unit = this;
    }

    public void OnStartTurn(){
        actionPoints = defaultActionPoints;
    }

    public void OnEndTurn(){
        actionPoints = 0;
    }

    public void SpendActionPoints(float cost){
        if (cost > actionPoints){
            Debug.LogWarning(
                $"Unit {name} does not have enough action points to spend {cost}. Current AP: {actionPoints}");
            actionPoints = 0;
            return;
        }
        actionPoints -= cost;
    }
}