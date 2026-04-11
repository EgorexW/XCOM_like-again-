using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitsTurnTaker : TurnTaker{
    [SerializeField] List<CombatUnit> units;

    public List<CombatUnit> Units => units.Copy();

    void Awake(){
        foreach (var unit in units){
            unit.onRemove.AddListener(RemoveUnit);
        }
    }

    void RemoveUnit(ICombatObject arg0){
        if (arg0 is CombatUnit unit){
            units.Remove(unit);
        }
    }

    public override void EndTurn(){
        base.EndTurn();
        foreach (var unit in units) unit.OnEndTurn();
    }

    public override void StartTurn(){
        base.StartTurn();
        if (units.Count == 0){
            CompleteTurn();
            return;
        }
        foreach (var unit in units) unit.OnStartTurn();
    }
}