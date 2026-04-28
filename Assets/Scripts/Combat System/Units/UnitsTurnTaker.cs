using System.Collections.Generic;
using UnityEngine;

public class UnitsTurnTaker : TurnTaker{
    readonly List<CombatUnit> units = new();

    public IReadOnlyList<CombatUnit> Units => units.AsReadOnly();

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

    public void AddUnit(CombatUnit combatUnit){
        if (units.Contains(combatUnit)){
            Debug.LogWarning($"Unit {combatUnit.name} is already in the turn taker. Skipping addition.");
            return;
        }
        units.Add(combatUnit);
        combatUnit.onRemove.AddListener(RemoveUnit);
    }
}