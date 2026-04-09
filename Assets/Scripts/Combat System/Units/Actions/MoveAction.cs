using UnityEngine;

public class MoveAction : TargetedUnitAction{
    protected override void OnExecute(){
        unit.MoveTo(targetNode);
    }

    protected override bool CheckActionSpecificRules(CombatGridNode node){
        if (node.IsOccupied){
            return false;
        }
        if (node == unit.Node){
            return false;
        }
        // if (!unit.Node.InStraightLine(node)){
        //     return false;
        // }
        if (!unit.Node.LineUnobstructed(node)){
            return false;
        }
        return true;
    }
}