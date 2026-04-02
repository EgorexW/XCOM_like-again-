using UnityEngine;

public class MoveAction : UnitAction{
    [SerializeField] int range = 2;
    
    CombatGridNode targetNode;

    protected override void OnExecute(){
        unit.MoveTo(targetNode);
    }

    public override void SetTarget(Vector2 pos){
        targetNode = unit.Grid.GetNode(pos);
    }

    protected override bool HasValidTarget(){
        if (targetNode == null){
            return false;
        }
        if (targetNode.IsOccupied){
            return false;
        }
        if (!unit.Node.InStraightLine(targetNode)){
            return false;
        }
        if (unit.Node.GetDistance(targetNode) > range){
            return false;
        }
        if (!unit.Node.LineUnobstructed(targetNode)){
            return false;
        }
        return true;
    }
}