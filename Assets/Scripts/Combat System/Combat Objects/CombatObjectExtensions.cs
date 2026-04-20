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
    
    public static float GetDistance(this ICombatObject combatObject, ICombatObject otherCombatObject){
        var myCenter = combatObject.GetCenter();
        var otherCenter = otherCombatObject.GetCenter();
        return Vector2.Distance(myCenter, otherCenter);
    }
    

    #region Flags
        public static CombatObjectFlags GetBlockingFlags(this CombatObject combatObject){
            var flags = combatObject.Flags;
            CombatObjectFlags blockedBy = CombatObjectFlags.Wall;
            
            if (flags.HasFlag(CombatObjectFlags.Wall)){
                return (CombatObjectFlags)(~0); 
            }
            
            if (flags.HasFlag(CombatObjectFlags.Object)){
                blockedBy |= CombatObjectFlags.Object;
            }

            return blockedBy;
    }
    #endregion

    #region Validation
    public static void Validate(this ICombatObject combatObject){
        combatObject.ValidateFlags();
    }
    public static void ValidateFlags(this ICombatObject combatObject){
        var flags = combatObject.Flags;
        AssertPairFlag(CombatObjectFlags.Wall, CombatObjectFlags.Object);
        // AssertPairFlag(CombatObjectFlags.Wall, CombatObjectFlags.LoSBlocker);
        AssertPairFlag(CombatObjectFlags.Wall, CombatObjectFlags.MovementBlocker);

        void AssertPairFlag(CombatObjectFlags flag1 ,CombatObjectFlags flag2){
            if (!flags.HasFlag(flag1)){
                return;
            }
            if (flags.HasFlag(flag2)){
                return;
            }
            Debug.LogWarning($"CombatObject {combatObject.Name} has {flag1} flag but not {flag2} flag.");
        }
    }
    #endregion
}


public static class GridBlockingFlags{
    public const CombatObjectFlags MovementBlocker = CombatObjectFlags.MovementBlocker;
    public const CombatObjectFlags ShootingBlocker = CombatObjectFlags.LoSBlocker;
    public const CombatObjectFlags ThrowBlocker = CombatObjectFlags.Wall;
}