using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CombatInit : MonoBehaviour{
    [BoxGroup("References")][Required][SerializeField] CombatSystem combatSystem;
    
    [SerializeField] Transform combatObjectsParent;
    [SerializeField] List<TurnTaker> turnTakers;

    void Start(){
        var combatObjs = combatObjectsParent.GetComponentsInChildren<ICombatObject>();
        foreach (var combatObj in combatObjs){
            combatSystem.AddCombatObject(combatObj);
        }
        foreach (var turnTaker in turnTakers){
            combatSystem.turnSystem.AddTurnTaker(turnTaker);
        }
        combatSystem.StartCombat();
    }
}