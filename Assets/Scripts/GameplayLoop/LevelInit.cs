using System.Collections.Generic;
using UnityEngine;

class LevelInit : MonoBehaviour{
    [SerializeField] List<UnitsTurnTaker> turnTakers;
    
    public CombatContent InitLevel(Level currentLevel){
        var combatObjects = currentLevel.GetCombatObjects();
        var teams = new List<Team>();
        for (int i = 0; i < turnTakers.Count; i++){
            var turnTaker = turnTakers[i];
            List<ICombatObject> units =  new List<ICombatObject>();
            var poses = currentLevel.GetSpawnPoints(i);
            for (int j = 0; j < turnTaker.Units.Count; j++){
                var unit = turnTaker.Units[j];
                unit.transform.position = poses[j];
                units.Add(unit);
                combatObjects.Add(unit);
            }
            Team team = new Team(units); 
            teams.Add(team);
        }
        return new CombatContent{
            combatObjects = combatObjects,
            turnTakers = new List<ITurnTaker>(turnTakers),
            teams = teams
        };
    }
}