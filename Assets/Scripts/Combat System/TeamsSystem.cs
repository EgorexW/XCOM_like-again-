using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[Flags]
public enum TeamFlag{
    None = 0,
    Player = 1 << 0,
    Enemy = 1 << 1
}


public class TeamsSystem : MonoBehaviour{
    [FoldoutGroup("Debug")] [ShowInInspector] readonly List<Team> teams = new();

    readonly Dictionary<ICombatObject, Team> combatObjectToTeam = new();
    public IReadOnlyList<Team> Teams => teams.AsReadOnly();

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
        return combatObjectToTeam.GetValueOrDefault(combatObject);
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

[Serializable]
public class Team{
    [ShowInInspector] readonly List<ICombatObject> combatObjects;

    public TeamFlag Flags{ get; private set; }

    public Team(List<ICombatObject> combatObjects, TeamFlag flags = TeamFlag.None){
        this.combatObjects = combatObjects;
        Flags = flags;
    }

    public IReadOnlyList<ICombatObject> CombatObjects => combatObjects.AsReadOnly();
    public bool Empty => combatObjects.Count == 0;

    public void RemoveCombatObject(ICombatObject arg0){
        combatObjects.Remove(arg0);
    }
}