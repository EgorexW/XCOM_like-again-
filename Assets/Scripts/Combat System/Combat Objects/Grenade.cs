using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Grenade : TurnTaker
{
    [SerializeField] int turnsToActivate = 1;
    [SerializeField] bool destroy = true;
    [SerializeField] CombatObject owner;
    [SerializeField] List<CombatEffect> effects;

    [FoldoutGroup("Events")]
    public UnityEvent onActivate = new();


    public override void StartTurn(){
        base.StartTurn();
        turnsToActivate -= 1;
        if (turnsToActivate <= 0){
            Activate();
        }
    }

    void Activate(){
        foreach (CombatEffect effect in effects){
            effect.targetNode = owner != null ? owner.Node : null;
            effect.Execute();
        }
        onActivate.Invoke();
        TurnSystem.RemoveTurnTaker(this);
        if (destroy){
            Destroy(gameObject);
        }
    }

    protected void Reset(){
        effects = new List<CombatEffect>(GetComponentsInChildren<CombatEffect>());
    }
}
