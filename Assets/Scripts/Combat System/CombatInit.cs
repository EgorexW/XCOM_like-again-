using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CombatInit : MonoBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] CombatSystem combatSystem;
    
    [SerializeField] Level level;

    public void InitCombatSystem(){
        var combatObjs = level.GetCombatObjects();
        foreach (var combatObj in combatObjs) combatSystem.AddCombatObject(combatObj, new List<CombatGridNode>{ combatSystem.CombatGrid.GetNode(combatObj.transform.position) });
        foreach (var turnTaker in level.GetTurnTakers()){
            combatSystem.TurnSystem.AddTurnTaker(turnTaker, InsertTurnTakerType.Last);
        }
        foreach (var team in level.GetTeams()){
            combatSystem.TeamsSystem.AddTeam(team);
        }
        combatSystem.StartCombat();
    }
}