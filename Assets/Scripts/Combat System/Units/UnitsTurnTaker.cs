using System.Collections.Generic;
using UnityEngine;

public class UnitsTurnTaker : TurnTaker{
    readonly List<Unit> units = new();

    public IReadOnlyList<Unit> Units => units.AsReadOnly();

    void RemoveUnit(ICombatObject arg0){
        if (arg0 is Unit unit){
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

    public void AddUnit(Unit unit){
        if (units.Contains(unit)){
            Debug.LogWarning($"Unit {unit.name} is already in the turn taker. Skipping addition.");
            return;
        }
        units.Add(unit);
        unit.onRemove.AddListener(RemoveUnit);
    }
}