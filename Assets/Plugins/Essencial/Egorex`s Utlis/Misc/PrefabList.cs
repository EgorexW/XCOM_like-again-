using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Egorex/Prefab List", fileName = "Prefab List")]
public class PrefabList : ScriptableObject{
    public List<GameObject> prefabs;

    public int GetPrefabIndex(GameObject prefab){
        for (var i = 0; i < prefabs.Count; i++)
            if (prefab.name == prefabs[i].name){
                return i;
            }
        Debug.LogWarning("Didn't find prefab", prefab);
        return -1;
    }
}