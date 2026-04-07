using System.Collections.Generic;
using UnityEngine;

public abstract class TargetedUnitAction : UnitAction{
    protected CombatGridNode targetNode;
    
    protected override bool CanExecute(){
        if (!base.CanExecute()){
            return false;
        }
        if (!IsValidTarget(targetNode)){
            Debug.Log($"Cannot execute action {Name} for unit {unit.name}, invalid target.");
            return false;
        }
        return true;
    }

    public virtual bool SetTarget(CombatGridNode node){
        if (!IsValidTarget(node)){
            Debug.LogWarning($"Invalid target node {node} for action {Name} of unit {unit.name}");
            return false;
        }
        targetNode = node;
        return true;
    }
    
    protected virtual bool IsValidTarget(CombatGridNode node){
        if (node == null){
            return false;
        }
        return true;
    }

    public bool SetTarget(Vector2 pos){
        var node = unit.Grid().GetNode(pos);
        return SetTarget(node);
    }

    public virtual List<CombatGridNode> GetValidTargets(){
        var list = new List<CombatGridNode>();
        foreach (var node in unit.Grid().Grid.gridArray){
            if (IsValidTarget(node)){
                list.Add(node);
            }
        }
        return list;
    }
}