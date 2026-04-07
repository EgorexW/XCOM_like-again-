using System.Collections.Generic;
using UnityEngine;

public abstract class TargetedUnitAction : UnitAction{
    protected CombatGridNode targetNode;
    
    [SerializeField] int range = 5;
    
    public float Range => range;

    protected override bool CanExecute(){
        if (!base.CanExecute()){
            return false;
        }
        if (!IsValidTarget(targetNode)){
            Debug.Log($"Cannot execute action {name} for unit {unit.name}, invalid target.");
            return false;
        }
        return true;
    }

    public virtual bool SetTarget(CombatGridNode node){
        if (!IsValidTarget(node)){
            Debug.LogWarning($"Invalid target node {node} for action {name} of unit {unit.name}");
            return false;
        }
        targetNode = node;
        return true;
    }

    protected virtual bool IsValidTarget(CombatGridNode node){
        if (node == null){
            return false;
        }
        if (unit.Node.GetDistance(node) > range){
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
        foreach (var node in unit.Grid().Grid.gridArray)
            if (IsValidTarget(node)){
                list.Add(node);
            }
        return list;
    }
}