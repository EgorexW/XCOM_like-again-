using System.Collections.Generic;
using UnityEngine;

public class UnitsTurnTaker : TurnTaker{
    [SerializeField] List<CombatUnit> units;
    
    public List<CombatUnit> Units => units.Copy();
    
    public override void OnEndTurn(){
        base.OnEndTurn();
        foreach (CombatUnit unit in units){
            unit.OnEndTurn();
        }
    }
    public override void OnStartTurn(){
        base.OnStartTurn();
        foreach (CombatUnit unit in units){
            unit.OnStartTurn();
        }
    }
}