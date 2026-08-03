using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class MissionTeamGenerator : TeamGenerator{
    [FormerlySerializedAs("missionConfig")] [FormerlySerializedAs("teamConfig")] [BoxGroup("References")][Required][SerializeField] MissionInitConfig missionInitConfig;

    [SerializeField] UnitsTurnTaker turnTaker;

    public override Team GenerateTeam(){
        var combatObjects = new List<ICombatObject>();
        foreach (var prefab in missionInitConfig.Prefabs){
            combatObjects.Add(AddTeamMember(prefab));
        }
        return new Team(combatObjects, teamFlag);
    }

    ICombatObject AddTeamMember(GameObject teamPrefab){
        var gameObj = Instantiate(teamPrefab, transform);
        var combatObject = gameObj.GetComponent<ICombatObject>();
        if (combatObject == null){
            Debug.LogError(
                $"The prefab {gameObj.name} does not have a component that implements ICombatObject. Skipping this prefab.");
            Destroy(gameObj);
            return null;
        }
        if (turnTaker != null){
            if (combatObject is Unit combatUnit){
                turnTaker.AddUnit(combatUnit);
            }
        }
        return combatObject;
    }
}