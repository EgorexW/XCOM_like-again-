using UnityEngine;

public class MoveAction : TargetedUnitAction{
    [SerializeField] int range = 2;

    protected override void OnExecute(){
        unit.MoveTo(targetNode);
    }

    protected override bool IsValidTarget(CombatGridNode node){
        if (!base.IsValidTarget(node)){
            return false;
        }
        if (node.IsOccupied){
            return false;
        }
        if (node == unit.Node){
            return false;
        }
        if (!unit.Node.InStraightLine(node)){
            return false;
        }
        if (unit.Node.GetDistance(node) > range){
            return false;
        }
        if (!unit.Node.LineUnobstructed(node)){
            return false;
        }
        return true;
    }
}