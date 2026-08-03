using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

class MissionInitConfig : MonoBehaviour{
    [Required] [SerializeField] SpawnTable mapSpawnTable;
    [SerializeField] List<GameObject> prefabs;
    
    public IReadOnlyList<GameObject> Prefabs => prefabs.AsReadOnly();
    
    public GameObject GetMapPrefab(){
        return mapSpawnTable.GetGameObject();
    }
}