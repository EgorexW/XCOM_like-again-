using System.Collections.Generic;
using UnityEngine;

public abstract class TargetedUnitAction : UnitAction{
    protected CombatGridNode targetNode;
    
    [SerializeField] int range = 5;
    
    public float Range => range;

    public override UnitActionValidation CanExecute(){
        var result = base.CanExecute();
        if (!IsValidTarget(targetNode)){
            result |= UnitActionValidation.InvalidTarget;
        }
        return result;
    }

    public virtual bool SetTarget(CombatGridNode node){
        if (!IsValidTarget(node)){
            Debug.LogWarning($"Invalid target node {node} for action {name} of unit {unit.name}");
            return false;
        }
        targetNode = node;
        return true;
    }

    public bool IsValidTarget(CombatGridNode node){
        if (node == null){
            return false;
        }
        if (unit.Node.GetDistance(node) > range){
            return false;
        }
        return CheckActionSpecificTargetRules(node);
    }

    public bool SetTarget(Vector2 pos){
        var node = unit.Grid().GetNode(pos);
        return SetTarget(node);
    }

    public List<CombatGridNode> GetValidTargets(){
        var list = new List<CombatGridNode>();
        foreach (var node in unit.Node.GetNodesInRadius(range)){
            if (CheckActionSpecificTargetRules(node)){
                list.Add(node);
            }
        }
        return list;
    }

    protected virtual bool CheckActionSpecificTargetRules(CombatGridNode node){
        return true; 
    }
}