using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameobjectPerTile : MonoBehaviour
{
    [BoxGroup("References")][Required][SerializeField] CombatObject targetObject;
    [BoxGroup("References")][Required][SerializeField] ObjectsPool objectsPool;

    void Awake(){
        targetObject.onInit.AddListener(OnInit);
        targetObject.onMove.AddListener(OnMove);
    }

    void OnMove(ICombatObject arg0){
        UpdateSprites();
    }

    void OnInit(ICombatObject arg0){
        UpdateSprites();
    }

    void UpdateSprites(){
        var nodes = targetObject.Nodes;
        objectsPool.SetCount(nodes.Count);
        for (int i = 0; i < nodes.Count; i++){
            var node = nodes[i];
            var obj = objectsPool.GetActiveObject(i);
            obj.transform.position = (Vector2)node.GetPos();
        }
    }
}
