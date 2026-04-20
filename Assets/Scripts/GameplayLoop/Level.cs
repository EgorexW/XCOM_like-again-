using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour{
    [SerializeField] Transform spawnPointsParent;

    public List<CombatObjectSpawn> GetCombatObjectSpawns(){
        var combatObjects = GetComponentsInChildren<CombatObject>();
        var combatObjectSpawns = new List<CombatObjectSpawn>();
        foreach (var combatObject in combatObjects){
            combatObjectSpawns.Add(new CombatObjectSpawn{
                combatObject = combatObject,
                position = Vector2Int.RoundToInt(combatObject.transform.position)
            });
        }
        return combatObjectSpawns;
    }

    public List<Vector2> GetSpawnPoints(int team){
        if (team >= spawnPointsParent.childCount){
            Debug.LogError($"Team {team} does not have a spawn point defined.");
            return new List<Vector2>();
        }
        var spawnPoints = spawnPointsParent.GetChild(team);
        var poses = new List<Vector2>();
        for (int i = 0; i < spawnPoints.childCount; i++){
            poses.Add(spawnPoints.GetChild(i).position);
        }
        poses.Shuffle();
        return poses;
    }
}