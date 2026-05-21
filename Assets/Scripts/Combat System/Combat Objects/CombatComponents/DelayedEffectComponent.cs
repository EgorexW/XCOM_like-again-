using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class DelayedEffectComponent : TurnTakerComponentBase{
    [SerializeField] Vector2Int startDurationLeft = Vector2Int.one;
    [SerializeField] bool destroy = true;
    [SerializeField] List<CombatEffect> effects;

    [FoldoutGroup("Events")] public UnityEvent onActivate = new();

    int durationLeft;
    public float DurationLeft => durationLeft;

    protected void Awake(){
        durationLeft = startDurationLeft.Random();
    }

    protected override void OnStartTurn(){
        durationLeft -= 1;
        if (durationLeft <= 0){
            Activate();
        }
    }

    void Activate(){
        foreach (var effect in effects){
            effect.targetNode = CombatObject?.GetCenterNode();
            effect.Execute();
        }
        onActivate.Invoke();
        active = false;
        if (!destroy){
            return;
        }
        CombatObject!.Remove();
        // if (CombatObject == null){
        //     Destroy(gameObject);
        // }
    }

    protected void Reset(){
        effects = new List<CombatEffect>(GetComponentsInChildren<CombatEffect>());
    }
}