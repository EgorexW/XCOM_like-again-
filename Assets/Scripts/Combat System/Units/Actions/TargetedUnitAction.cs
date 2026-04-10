using System.Collections.Generic;
using UnityEngine;

public abstract class TargetedUnitAction : UnitAction{
    protected CombatGridNode targetNode;
    
    [SerializeField] int range = 5;
    
    public float Range => range;

    public override UnitActionValidation CanExecute(){
        var baseValidation = base.CanExecute();
        if (baseValidation != UnitActionValidation.Success){
            return baseValidation;
        }
        if (!IsValidTarget(targetNode)){
            return UnitActionValidation.InvalidTarget;
        }
        return UnitActionValidation.Success;
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
        return CheckActionSpecificRules(node);
    }

    public bool SetTarget(Vector2 pos){
        var node = unit.Grid().GetNode(pos);
        return SetTarget(node);
    }

    public List<CombatGridNode> GetValidTargets(){
        var list = new List<CombatGridNode>();
        foreach (var node in unit.Node.GetNodesInRadius(range)){
            if (CheckActionSpecificRules(node)){
                list.Add(node);
            }
        }
        return list;
    }

    protected virtual bool CheckActionSpecificRules(CombatGridNode node){
        return true; 
    }
}