using System.Collections.Generic;
using UnityEngine;

public static class CombatObjectExtensions{
    public static CombatGrid Grid(this ICombatObject combatObject){
        var nodes = combatObject.Nodes;
        return nodes.PrimaryGrid();
    }
    public static void MoveTo(this ICombatObject combatObject, CombatGridNode targetNode){
        combatObject.MoveTo(new List<CombatGridNode>{targetNode});
    }

    public static Vector2 GetCenter(this ICombatObject combatObject){
        return combatObject.Nodes.GetCenter();
    }
    public static CombatGridNode GetCenterNode(this ICombatObject combatObject){
        return combatObject.Nodes.GetCenterNode();
    }
}