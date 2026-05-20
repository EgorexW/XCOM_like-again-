using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu]
[InlineEditor]
public class SpawnTable : ScriptableObject
{
    [SerializeField] List<ObjectWithValue<Object>> gameObjects;
    
#if UNITY_EDITOR
    [ShowInInspector][ReadOnly] List<GameObject> possibleGameObjects = new();
    [ShowInInspector][ReadOnly] List<SpawnTable> referencedByTables = new();
#endif

    public GameObject GetGameObject()
    {
        if (gameObjects.Count == 0){
            return null;
        }
        var rolledObj = gameObjects.GetWeightedRoll();
        if (rolledObj.Object is GameObject){
            return rolledObj.Object as GameObject;
        }
        if (rolledObj.Object is SpawnTable getGameObject){
            return getGameObject.GetGameObject();
        }
        Debug.LogError("Object is not GameObject or another SpawnTable is " + name, this);
        return null;
    }
    
#if UNITY_EDITOR
    void OnValidate()
    {
        possibleGameObjects.Clear();
        Queue<ObjectWithValue<Object>> toProcess = new(gameObjects);
        foreach (var objectWithValue in toProcess){
            if (objectWithValue.Object is not SpawnTable table){
                return;
            }
            if (!table.referencedByTables.Contains(this)){
                table.referencedByTables.Add(this);
            }
        }
        while (toProcess.Count > 0){
            var item = toProcess.Dequeue();
            switch (item.Object){
                case SpawnTable getGameObject:
                    foreach (var obj in getGameObject.gameObjects){
                        toProcess.Enqueue(obj);
                    }
                    break;
                case GameObject gameObj:
                    if (!possibleGameObjects.Contains(gameObj)){
                        possibleGameObjects.Add(gameObj);
                    }
                    break;
            }
        }
        
        foreach (var table in referencedByTables.Copy()){
            if (table.gameObjects.All(x => x.Object != this)){
                referencedByTables.Remove(table);
            }
        }
    }
#endif
}