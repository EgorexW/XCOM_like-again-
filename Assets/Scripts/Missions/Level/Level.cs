using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour{
    bool generated;
    
    void Generate(){
        if (generated){
            return;
        }
        foreach (var generator in GetComponentsInChildren<LevelGenerator>()){
            generator.GenerateLevel();
        }
        generated = true;
    }
    
    public List<CombatObjectSpawn> GetCombatObjectSpawns(){
        Generate();
        var combatObjects = GetComponentsInChildren<CombatObject>();
        var combatObjectSpawns = new List<CombatObjectSpawn>();
        foreach (var combatObject in combatObjects)
            combatObjectSpawns.Add(new CombatObjectSpawn{
                combatObject = combatObject,
                position = Vector2Int.RoundToInt(combatObject.transform.position)
            });
        return combatObjectSpawns;
    }

    public List<MapNode> GetNodes(MapNodeType type, string group = null){
        Generate();
        if (string.IsNullOrEmpty(group)) {
            var availableGroups = GetAvailableGroups(type);
            if (availableGroups.Count > 0) {
                group = availableGroups[0];
            }
        }
        var allNodes = GetComponentsInChildren<MapNode>();
        var result = new List<MapNode>();
        foreach(var node in allNodes){
            if (node.nodeType == type && node.nodeGroup == group) {
                result.Add(node);
            }
        }
        return result;
    }

    public List<string> GetAvailableGroups(MapNodeType type){
        Generate();
        var allNodes = GetComponentsInChildren<MapNode>();
        var groups = new System.Collections.Generic.HashSet<string>();
        foreach(var node in allNodes){
            if (node.nodeType == type){
                groups.Add(node.nodeGroup);
            }
        }
        var list = new List<string>(groups);
        return list;
    }
}