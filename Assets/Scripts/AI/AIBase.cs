using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class AIBehaviour : MonoBehaviour{
    [BoxGroup("References")][Required][SerializeField] protected CombatUnit combatUnit;
    
    public abstract AIAction GetAction(AIContext context);
}

public abstract class AITargetEvaluator : MonoBehaviour{
    public abstract TargetEvaluation EvaluateTargetedAction(AIContext aiContext, TargetedUnitAction validNodes);
}

public class TargetEvaluation{
    public CombatGridNode bestNode;
    public float score;
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
    public static List<ICombatObject> GetExposedEnemies(this CombatGridNode node, List<ICombatObject> enemies){
        var exposedEnemies = new List<ICombatObject>();
        foreach (var enemy in enemies){
            if (node.CanAttack(enemy.Node)){
                exposedEnemies.Add(enemy);
            }
        }
        return exposedEnemies;
    }
}

public class AIContext{
    public readonly CombatUnit Unit;
    public readonly List<ICombatObject> Enemies;

    public readonly List<ICombatObject> Allies;
    // Możesz tu dorzucić referencję do mapy/gridu, jeśli potrzebna
    // public readonly CombatGrid Grid;

    public AIContext(CombatUnit unit, List<ICombatObject> enemies, List<ICombatObject> allies){
        Unit = unit;
        Enemies = enemies;
        Allies = allies;
    }
}