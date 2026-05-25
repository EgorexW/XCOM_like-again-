using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BasicMissionInit : MissionInit
{
    [Required] [SerializeField] SpawnTable mapSpawnTable;
    [SerializeField] Vector2 levelSpawnPos = new(50, 50);
    [SerializeField] List<TeamGenerator> teamGenerators;
    [SerializeField] List<PayoutObjective> payoutObjectives;
    [SerializeField] List<TurnTaker> turnTakers;

    public override void InitMission(CombatContent content)
    {
        // 1. Generate Map
        var mapPrefab = mapSpawnTable.GetGameObject();

        var spawnedLevelObj = Instantiate(mapPrefab, levelSpawnPos, Quaternion.identity);
        var currentLevel = spawnedLevelObj.GetComponent<Level>();
        if (currentLevel == null){
            Debug.LogError("Spawned map is missing Level component.");
            return;
        }

        content.levelPrefab = mapPrefab; 
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
