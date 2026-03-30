using UnityEngine;

public class MoveAction : UnitAction{
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
        return true;
    }
}