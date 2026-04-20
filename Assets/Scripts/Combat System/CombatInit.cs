using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CombatInit : MonoBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] CombatSystem combatSystem;

    public void InitCombatSystem(CombatContent content){
        var combatObjs = content.combatObjects;
        foreach (var combatObj in combatObjs){
            combatSystem.AddCombatObject(combatObj, new List<CombatGridNode>{ combatSystem.CombatGrid.GetNode(combatObj.transform.position) });
        }
        foreach (var turnTaker in content.turnTakers){
            combatSystem.TurnSystem.AddTurnTaker(turnTaker, InsertTurnTakerType.Last);
        }
        foreach (var team in content.teams){
            combatSystem.TeamsSystem.AddTeam(team);
        }
        combatSystem.StartCombat();
    }
}

public class CombatContent{
    public List<CombatObject> combatObjects;
    public List<ITurnTaker> turnTakers;
    public List<Team> teams;
}