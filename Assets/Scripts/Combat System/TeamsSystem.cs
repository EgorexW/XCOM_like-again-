using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamsSystem : MonoBehaviour{
    List<Team> teams  = new List<Team>();
    
    Dictionary<ICombatObject, Team> combatObjectToTeam = new();

    public void AddTeam(Team team){
        teams.Add(team);
        foreach (var combatObject in team.CombatObjects){
            combatObject.onRemove.AddListener(RemoveCombatObject);
            combatObjectToTeam.Add(combatObject, team);
        }
    }

    void RemoveCombatObject(ICombatObject arg0){
        teams.ForEach(team => team.RemoveCombatObject(arg0));
        combatObjectToTeam.Remove(arg0);
    }

    public Team GetTeam(ICombatObject combatObject){
        return combatObjectToTeam[combatObject];
    }

    public List<Team> GetEnemyTeams(ICombatObject combatObject){
        var enemyTeams = teams.Copy();
        enemyTeams.Remove(GetTeam(combatObject));
        return enemyTeams;
    }
    
    public List<ICombatObject> GetAllies(ICombatObject combatObject){
        var team = GetTeam(combatObject);
        return team.CombatObjects.ToList();
    }
    
     public List<ICombatObject> GetEnemies(ICombatObject combatObject){
         var enemyTeams = GetEnemyTeams(combatObject);
         var enemies = new List<ICombatObject>();
         foreach (var team in enemyTeams) enemies.AddRange(team.CombatObjects);
         return enemies;
     }
}

public class Team{
    List<ICombatObject> combatObjects;

    public Team(List<ICombatObject> combatObjects){
        this.combatObjects = combatObjects;
    }

    public IReadOnlyList<ICombatObject> CombatObjects => combatObjects.ReadOnly();

    public void RemoveCombatObject(ICombatObject arg0){
        combatObjects.Remove(arg0);
    }
}