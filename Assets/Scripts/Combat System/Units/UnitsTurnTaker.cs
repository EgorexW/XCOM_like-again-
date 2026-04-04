using System.Collections.Generic;
using UnityEngine;

public class UnitsTurnTaker : TurnTaker{
    [SerializeField] List<CombatUnit> units;
    
    public List<CombatUnit> Units => units.Copy();
    
    public override void EndTurn(){
        base.EndTurn();
        foreach (CombatUnit unit in units){
            unit.OnEndTurn();
        }
    }
    public override void StartTurn(){
        base.StartTurn();
        foreach (CombatUnit unit in units){
            unit.OnStartTurn();
        }
    }
}