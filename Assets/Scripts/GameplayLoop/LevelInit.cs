using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

class LevelInit : MonoBehaviour{
    [SerializeField] Vector2 levelSpawnPos = new Vector2(50, 50);
    
    public void InitLevel(CombatContent content){
        var spawnedLevel = Instantiate(content.levelPrefab, levelSpawnPos, Quaternion.identity);
        var currentLevel = spawnedLevel.GetComponent<Level>();
        if (currentLevel == null){
            throw new MissingComponentException("Level component is missing from the prefab.");
        }
        var combatObjects = currentLevel.GetCombatObjectSpawns();
        for (int i = 0; i < content.teams.Count; i++){
            var team = content.teams[i];
            var poses = currentLevel.GetSpawnPoints(i);
            for (int j = 0; j < team.CombatObjects.Count; j++){
                if (j >= poses.Count){
                    Debug.LogError($"Team {i} does not have enough spawn points for all combat objects. Skipping remaining objects.");
                    break;
                }
                var unit = team.CombatObjects[j];
                combatObjects.Add(new CombatObjectSpawn{
                    combatObject = unit,
                    position = Vector2Int.RoundToInt(poses[j % poses.Count])
                });
            }
        }
        content.combatObjects.AddRange(combatObjects);
    }
}

