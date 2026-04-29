using System.Collections.Generic;
using Nrjwolf.Tools.AttachAttributes;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class DelayedEffectComponent : TurnTakerComponent{
    [SerializeField] int turnsToActivate = 1;
    [SerializeField] bool destroy = true;
    [SerializeField] List<CombatEffect> effects;
    
    [FoldoutGroup("Events")] public UnityEvent onActivate = new();

    public override void StartTurn(){
        base.StartTurn();
        turnsToActivate -= 1;
        if (turnsToActivate <= 0){
            Activate();
        }
        CompleteTurn();
    }

    void Activate(){
        foreach (var effect in effects){
            effect.targetNode = CombatObject?.GetCenterNode();
            effect.Execute();
        }
        onActivate.Invoke();
        TurnSystem.RemoveTurnTaker(this);
        if (destroy){
            CombatObject?.Remove();
            if (CombatObject == null){
                Destroy(gameObject);
            }
        }
    }

    protected void Reset(){
        effects = new List<CombatEffect>(GetComponentsInChildren<CombatEffect>());
    }
}