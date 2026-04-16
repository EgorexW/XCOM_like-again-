using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class AIBehaviour : MonoBehaviour{
    [BoxGroup("References")][Required][SerializeField] protected CombatUnit combatUnit;
    
    public abstract AIAction GetAction(AIContext context);
}

public abstract class AIActionCreator : MonoBehaviour{
    public abstract AIAction CreateAIAction(AIContext context);
}

public class AIAction{
    public readonly UnitAction action;
    public readonly CombatGridNode targetNode;
    float score;
    bool valid = true;
    
    public AIAction(UnitAction action = null, CombatGridNode targetNode = null, float score = 0f){
        this.action = action;
        this.targetNode = targetNode;
        this.score = score;
    }
    
    public void SetScore(float newScore){
        score = newScore;
    }
    bool IsEmpty => action == null;
    public static AIAction Invalid => new AIAction{
        valid = false,
    };
    public bool Valid => valid && !IsEmpty;
    public float Score => Valid ? score : float.MinValue;
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

    public readonly bool Debug;

    public AIContext(CombatUnit unit, List<ICombatObject> enemies, List<ICombatObject> allies, bool debug){
        Unit = unit;
        Enemies = enemies;
        Allies = allies;
        Debug = debug;
    }
}