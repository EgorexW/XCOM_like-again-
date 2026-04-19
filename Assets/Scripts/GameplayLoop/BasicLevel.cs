using System.Collections.Generic;
using UnityEngine;

public class BasicLevel : Level{
    [SerializeField] List<UnitsTurnTaker> turnTakers;
    
    public override List<ITurnTaker> GetTurnTakers(){
        return new List<ITurnTaker>(turnTakers);
    }
    public override List<Team> GetTeams(){
        var teams = new List<Team>();
        foreach (var turnTaker in turnTakers){
            Team team = new Team(turnTaker.Units.ConvertAll(unit => (ICombatObject) unit)); 
            teams.Add(team);
        }
        return teams;
    }
}