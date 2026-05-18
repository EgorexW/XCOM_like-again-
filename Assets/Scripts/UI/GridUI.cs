using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class GridUI : UIElement{
    [FormerlySerializedAs("squaresPool")] [FormerlySerializedAs("objectsPool")] [BoxGroup("References")] [Required] [SerializeField] ObjectsPool pool;

    public void MarkPositons(List<CombatGridNode> nodes){
        var count = nodes.Count;
        pool.SetCount(count);
        for (int i = 0; i < count; i++){
            var node = nodes[i];
            var obj = pool.GetActiveObject(i);
            obj.transform.position = (Vector2)node.GetPos();
        }
    }

    public void ClearMarks(){
        pool.Clear();
    }
}