using UnityEngine;

public class MoveAction : TargetedUnitAction{
    protected override void OnExecute(){
        unit.MoveTo(targetNode);
    }

    protected override TargetValidation CheckActionSpecificTargetRules(CombatGridNode node){
        var result = base.CheckActionSpecificTargetRules(node);
        if (!node.CanAcceptObject(unit.OccupancyType)){
            result |= TargetValidation.InvalidTarget;
        }
        if (node == unit.Node){
            result |= TargetValidation.InvalidTarget;
        }
        if (!unit.Node.LineUnobstructed(node, GridOccupancyType.Character)){
            result |= TargetValidation.NoPath;
        }
        return result;
    }
}