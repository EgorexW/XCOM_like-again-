public class MoveAction : TargetedUnitAction{
    protected override void OnExecute(){
        unit.MoveTo(targetNode);
    }

    protected override TargetValidation CheckActionSpecificTargetRules(CombatGridNode node){
        var result = base.CheckActionSpecificTargetRules(node);
        if (!node.CanAcceptObject(unit)){
            result |= TargetValidation.InvalidTarget;
        }
        if (node.Contains(unit)){
            result |= TargetValidation.InvalidTarget;
        }
        if (!unit.GetCenterNode().LineUnobstructed(node, unit.GetBlockingFlags())){
            result |= TargetValidation.NoPath;
        }
        return result;
    }
}