using System;
using System.Collections.Generic;
using Nrjwolf.Tools.AttachAttributes;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class DelayedEffectComponent : TurnTakerComponent{
    [SerializeField] Vector2Int startDurationLeft = Vector2Int.one;
    [SerializeField] bool destroy = true;
    [SerializeField] List<CombatEffect> effects;
    
    [FoldoutGroup("Events")] public UnityEvent onActivate = new();
    
    int durationLeft;
    public float DurationLeft => durationLeft;

    protected void Awake(){
        durationLeft = startDurationLeft.Random();
    }

    public override void StartTurn(){
        base.StartTurn();
        durationLeft -= 1;
        if (durationLeft <= 0){
            Activate();
        }
        else{
            CompleteTurn();
        }
    }

    void Activate(){
        foreach (var effect in effects){
            effect.targetNode = CombatObject?.GetCenterNode();
            effect.Execute();
        }
        onActivate.Invoke();
        TurnSystem.RemoveTurnTaker(this);
        if (destroy){
            CombatObject!.Remove();
            if (CombatObject == null){
                Destroy(gameObject);
            }
        }
    }

    protected void Reset(){
        effects = new List<CombatEffect>(GetComponentsInChildren<CombatEffect>());
    }
}