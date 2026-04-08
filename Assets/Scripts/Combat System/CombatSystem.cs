using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class CombatSystem : MonoBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] CombatGrid combatGrid;
    [BoxGroup("References")] [Required] [SerializeField] TurnSystem turnSystem;

    readonly List<ICombatObject> combatObjects = new();

    [FoldoutGroup("Events")] public UnityEvent onCombatStarted;

    public List<ICombatObject> CombatObjects => combatObjects.Copy();
    public CombatGrid CombatGrid => combatGrid;
    public TurnSystem TurnSystem => turnSystem;

    public void AddCombatObject(ICombatObject combatObject){
        combatObjects.Add(combatObject);
        combatObject.MoveTo(combatGrid.GetNode(combatObject.WorldPosition()));
        combatObject.CombatSystem = this;
        combatObject.Init();
    }

    public void RemoveCombatObject(ICombatObject arg0){
        combatObjects.Remove(arg0);
        combatGrid.RemoveCombatObject(arg0);
    }

    public void StartCombat(){
        turnSystem.NextTurn();
        onCombatStarted.Invoke();
    }
}