using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CombatInit : MonoBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] CombatSystem combatSystem;

    [SerializeField] Transform combatObjectsParent;
    [SerializeField] List<UnitsTurnTaker> turnTakers;

    protected void Start(){
        var combatObjs = combatObjectsParent.GetComponentsInChildren<ICombatObject>();
        foreach (var combatObj in combatObjs) combatSystem.AddCombatObject(combatObj, combatSystem.CombatGrid.GetNode(combatObj.WorldPosition()));
        foreach (var turnTaker in turnTakers){
            combatSystem.TurnSystem.AddTurnTaker(turnTaker, InsertTurnTakerType.Last);
            Team team = new Team(turnTaker.Units.ConvertAll(unit => (ICombatObject) unit));
            combatSystem.TeamsSystem.AddTeam(team);
        }
        combatSystem.StartCombat();
    }
}