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
        if (node.Contains(unit)){
            result |= TargetValidation.InvalidTarget;
        }
        if (!unit.GetCenterNode().LineUnobstructed(node, GridOccupancyType.Character)){
            result |= TargetValidation.NoPath;
        }
        return result;
    }
}