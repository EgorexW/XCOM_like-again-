using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamsSystem : MonoBehaviour{
    readonly List<Team> teams = new();

    readonly Dictionary<ICombatObject, Team> combatObjectToTeam = new();
    public IReadOnlyList<Team> Teams  => teams.AsReadOnly();

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
        var team = GetTeam(combatObject);
        return GetEnemyTeams(team);
    }

    public List<ICombatObject> GetAllies(ICombatObject combatObject){
        var team = GetTeam(combatObject);
        return team.CombatObjects.ToList();
    }

    public List<ICombatObject> GetEnemies(ICombatObject combatObject){
        var team = GetTeam(combatObject);
        return GetEnemies(team);
    }
    
    public List<ICombatObject> GetEnemies(Team team){
        var enemyTeams = GetEnemyTeams(team);
        var enemies = new List<ICombatObject>();
        foreach (var enemyTeam in enemyTeams) enemies.AddRange(enemyTeam.CombatObjects);
        return enemies;
    }

    public List<Team> GetEnemyTeams(Team team){
        var enemyTeams = teams.Copy();
        enemyTeams.Remove(team);
        return enemyTeams;
    }
}

public class Team{
    readonly List<ICombatObject> combatObjects;

    public Team(List<ICombatObject> combatObjects){
        this.combatObjects = combatObjects;
    }

    public IReadOnlyList<ICombatObject> CombatObjects => combatObjects.AsReadOnly();
    public bool Empty  => combatObjects.Count == 0;

    public void RemoveCombatObject(ICombatObject arg0){
        combatObjects.Remove(arg0);
    }
}