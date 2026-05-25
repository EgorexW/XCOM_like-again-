using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour{
    [SerializeField] Transform spawnPointsParent;

    public List<CombatObjectSpawn> GetCombatObjectSpawns(){
        var combatObjects = GetComponentsInChildren<CombatObject>();
        var combatObjectSpawns = new List<CombatObjectSpawn>();
        foreach (var combatObject in combatObjects)
            combatObjectSpawns.Add(new CombatObjectSpawn{
                combatObject = combatObject,
                position = Vector2Int.RoundToInt(combatObject.transform.position)
            });
        return combatObjectSpawns;
    }

    public List<MapNode> GetNodes(MapNodeType type, string group){
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
        var allNodes = GetComponentsInChildren<MapNode>();
        var groups = new System.Collections.Generic.HashSet<string>();
        foreach(var node in allNodes){
            if (node.nodeType == type){
                groups.Add(node.nodeGroup);
            }
        }
        var list = new List<string>(groups);
        list.Shuffle();
        return list;
    }
}