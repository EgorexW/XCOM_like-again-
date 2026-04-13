using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetedUnitAction : UnitAction{
    protected CombatGridNode targetNode;
    
    [SerializeField] int range = 5;
    
    public float Range => range;

    public override UnitActionValidation ValidateAction(){
        var result = base.ValidateAction();
        if (ValidateTarget(targetNode) != TargetValidation.Valid){
            result |= UnitActionValidation.InvalidTarget;
        }
        return result;
    }

    public virtual bool SetTarget(CombatGridNode node){
        if (ValidateTarget(node) != TargetValidation.Valid){
            Debug.LogWarning($"Invalid target node {node} for action {Name} of unit {unit.name}");
            return false;
        }
        Debug.Log("Setting target node to " + node);
        targetNode = node;
        return true;
    }

    public TargetValidation ValidateTarget(CombatGridNode node){
        var result = TargetValidation.Valid;
        if (node == null){
            result |= TargetValidation.NotANode;
            return result; // Can't Validate further
        }
        if (unit.Node.GetDistance(node) > range){
            result |= TargetValidation.OutOfRange;
        }
        result |= CheckActionSpecificTargetRules(node);
        return result;
    }

    public bool SetTarget(Vector2 pos){
        var node = unit.Grid().GetNode(pos);
        return SetTarget(node);
    }

    public List<CombatGridNode> GetValidTargets(){
        var list = new List<CombatGridNode>();
        foreach (var node in GetAllTargets()){
            if (CheckActionSpecificTargetRules(node) == TargetValidation.Valid){
                list.Add(node);
            }
        }
        return list;
    }

    protected virtual TargetValidation CheckActionSpecificTargetRules(CombatGridNode node){
        return TargetValidation.Valid;
    }

    public List<CombatGridNode> GetAllTargets(){
        return unit.Node.GetNodesInRadius(range);
    }
}

[Flags]
public enum TargetValidation{
    Valid          = 0,      
    OutOfRange     = 1 << 0, 
    InvalidTarget  = 1 << 1, 
    NoValidTarget  = 1 << 2, 
    NoPath         = 1 << 3,
    NotANode       = 1 << 4,
}