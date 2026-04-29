using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class AIBehaviour : MonoBehaviour{
    [FormerlySerializedAs("combatUnit")] [BoxGroup("References")] [Required] [SerializeField] protected Unit unit;

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

    public AIAction(UnitAction action = null, CombatGridNode targetNode = null, float score = 0f,
        AIActionFlags actionFlags = AIActionFlags.None){
        this.action = action;
        this.targetNode = targetNode;
        this.score = score;
        this.ActionFlags = actionFlags;
    }

    public void SetScore(float newScore){
        score = newScore;
    }

    bool IsEmpty => action == null;
    public static AIAction Invalid => new(){
        valid = false
    };
    public bool Valid => valid && !IsEmpty;
    public float Score => Valid ? score : float.MinValue;
    public AIActionFlags ActionFlags{ get; private set; }

    public void AddFlag(AIActionFlags flags){
        ActionFlags |= flags;
    }
    //
    // public void RemoveFlag(AIActionFlags flags){
    //     actionFlags &= ~flags;
    // }
}

[Flags]
public enum AIActionFlags{
    None = 0,
    TileExposed = 1 << 1,
    EnemyExposed = 1 << 2,
    MagazineEmpty = 1 << 3,
    SelfExposed = 1 << 4
}

public class AIContext{
    public readonly Unit unit;
    public readonly List<ICombatObject> enemies;

    public readonly List<ICombatObject> allies;

    public readonly bool debug;

    public AIContext(Unit unit, List<ICombatObject> enemies, List<ICombatObject> allies, bool debug){
        this.unit = unit;
        this.enemies = enemies;
        this.allies = allies;
        this.debug = debug;
    }
}