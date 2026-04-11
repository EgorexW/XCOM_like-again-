using UnityEngine;

public class MoveAction : TargetedUnitAction{
    protected override void OnExecute(){
        unit.MoveTo(targetNode);
    }

    protected override bool CheckActionSpecificRules(CombatGridNode node){
        if (!node.CanAcceptObject(unit.OccupancyType)){
            return false;
        }
        if (node == unit.Node){
            return false;
        }
        if (!unit.Node.LineUnobstructed(node, GridOccupancyType.Character)){
            return false;
        }
        return true;
    }
}