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
    
        public static CombatObjectFlags GetBlockingFlags(this CombatObject combatObject){
            var flags = combatObject.Flags;
            CombatObjectFlags blockedBy = CombatObjectFlags.Wall;
            
            if (flags.HasFlag(CombatObjectFlags.Wall)){
                return (CombatObjectFlags)(~0); 
            }
            
            if (flags.HasFlag(CombatObjectFlags.Unit)){
                blockedBy |= CombatObjectFlags.Unit;
            }

            return blockedBy;
    }
}


public static class GridBlockingFlags{
    public const CombatObjectFlags MovementBlocker = CombatObjectFlags.Wall | CombatObjectFlags.Unit;
    public const CombatObjectFlags AttackBlocker = CombatObjectFlags.Wall | CombatObjectFlags.Unit;
    public const CombatObjectFlags ThrowBlocker = CombatObjectFlags.Wall;
}