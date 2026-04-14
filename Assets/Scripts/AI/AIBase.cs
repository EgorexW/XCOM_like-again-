using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class AIBehaviour : MonoBehaviour{
    [BoxGroup("References")][Required][SerializeField] protected CombatUnit combatUnit;
    
    public abstract AIAction GetAction();
}

public abstract class AIMovementBehaviour : MonoBehaviour{
    public abstract MovementEvaluation EvaluateMovementOptions(CombatUnit unit, List<CombatGridNode> reachableNodes, List<ICombatObject> enemies, List<ICombatObject> allies);
}

public class MovementEvaluation{
    public CombatGridNode bestNode;
    public float nodeScore;
    
    public static MovementEvaluation CurrentBest => new MovementEvaluation{
        bestNode = null,
        nodeScore = float.MinValue
    };
    public bool CurrentNodeBest => bestNode == null;
}

public class AIAction{
    public UnitAction Action;
    public CombatGridNode TargetNode;
    
    public bool IsEmpty => Action == null;
    
    public static AIAction Empty => new AIAction();
}

static class AIHelper{
    public static bool IsExposed(this CombatGridNode node, List<ICombatObject> enemies){
        bool exposed = false;
        foreach (var enemy in enemies){
            if (enemy.Node.CanAttack(node)){
                exposed = true;
                break;
            }
        }
        return exposed;
    }
    public static List<ICombatObject> GetExposedEnemies(this CombatGridNode node, List<ICombatObject> enemies){ // TODO invorporate attack range
        var exposedEnemies = new List<ICombatObject>();
        foreach (var enemy in enemies){
            if (node.CanAttack(enemy.Node)){
                exposedEnemies.Add(enemy);
            }
        }
        return exposedEnemies;
    }
}