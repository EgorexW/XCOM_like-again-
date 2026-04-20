using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SimpleTeamGenerator : TeamGenerator{
    [BoxGroup("References")] [Required] [SerializeField] List<GameObject> teamPrefabs;

    [SerializeField] UnitsTurnTaker turnTaker;

    [SerializeField] int teamCount = 3;

    public override Team GenerateTeam(){
        var combatObjects = new List<ICombatObject>();
        for (var i = 0; i < teamCount; i++){
            var gameObj = Instantiate(teamPrefabs.Random(), transform);
            var combatObject = gameObj.GetComponent<ICombatObject>();
            if (combatObject == null){
                Debug.LogError(
                    $"The prefab {gameObj.name} does not have a component that implements ICombatObject. Skipping this prefab.");
                Destroy(gameObj);
                continue;
            }
            combatObjects.Add(combatObject);
            if (turnTaker != null){
                if (combatObject is CombatUnit combatUnit){
                    turnTaker.AddUnit(combatUnit);
                }
            }
        }
        return new Team(combatObjects);
    }
}