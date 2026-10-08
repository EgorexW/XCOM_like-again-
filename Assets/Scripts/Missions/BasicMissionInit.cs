using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class BasicMissionInit : MissionInit{
    [FormerlySerializedAs("config")] [BoxGroup("References")] [Required] [SerializeField] MissionInitConfig initConfig;
    [SerializeField] List<TeamGenerator> teamGenerators;
    [SerializeField] List<PayoutObjective> payoutObjectives;
    [SerializeField] List<TurnTaker> turnTakers;

    public override void InitMission(CombatContent content)
    {
        // 1. Generate Map
        var mapPrefab = initConfig.GetMapPrefab();
        content.levelSize = mapPrefab.GetComponent<Level>().GetMapSize();
        var initPos = new Vector3Int(Mathf.CeilToInt(content.levelSize.x / 2f), Mathf.CeilToInt(content.levelSize.y / 2f), 0);
        // TODO ideally all levels expand to the right and up, so we can just spawn at (0,0) and not worry about the size of the map. For now, we spawn at the center of the map to avoid negative coordinates.

        var spawnedLevelObj = Instantiate(mapPrefab, initPos, Quaternion.identity);
        var currentLevel = spawnedLevelObj.GetComponent<Level>();
        if (currentLevel == null){
            Debug.LogError("Spawned map is missing ILevel component.");
            return;
        }

        // content.levelPrefab = mapPrefab; 
        var combatObjects = currentLevel.GetCombatObjectSpawns();

        // 2. Team Spawning
        foreach (var teamGen in teamGenerators)
        {
            var team = teamGen.GenerateTeam();
            content.teams.Add(team);

            var nodeType = teamGen.targetNodeType;
            var nodes = currentLevel.GetNodes(nodeType); // Automatically picks a random group
            nodes.Shuffle();
            
            for (var j = 0; j < team.CombatObjects.Count; j++){
                if (j >= nodes.Count){
                    Debug.LogWarning($"Team {team.Flags} does not have enough spawn points for node type {nodeType}.");
                    break;
                }
                var unit = team.CombatObjects[j];
                combatObjects.Add(new CombatObjectSpawn{
                    combatObject = unit,
                    position = Vector2Int.RoundToInt(nodes[j].transform.position)
                });
            }
        }
        content.combatObjects.AddRange(combatObjects);
        
        // 3. Objectives
        content.payoutObjectives.AddRange(payoutObjectives);
        content.turnTakers.AddRange(turnTakers);
    }
}
