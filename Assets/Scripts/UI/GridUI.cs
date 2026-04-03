using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class GridUI : UIElement{
    [FormerlySerializedAs("objectsPool")] [BoxGroup("References")] [Required] [SerializeField] ObjectsPool squaresPool;
    [BoxGroup("References")] [Required] [SerializeField] ObjectsPool marksPool;

    public void ShowGrid<T>(Grid<T> grid){ ;
        var count = grid.width * grid.height;
        squaresPool.SetCount(count);
        int i = 0;
        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                GameObject cellObj = squaresPool.GetActiveObject(i);
                cellObj.transform.position = grid.GetWorldPosition(x, y);
                cellObj.transform.localScale = Vector3.one * grid.cellSize;
                i++;
            }
        }
    }

    public void ShowMarks<T>(Grid<T> grid, List<T> positions) where T : GridNode{
        marksPool.SetCount(positions.Count);
        for (int i = 0; i < positions.Count; i++){
            var pos = positions[i];
            GameObject cellObj = marksPool.GetActiveObject(i);
            cellObj.transform.position = grid.GetWorldPosition(pos.x, pos.y);
            cellObj.transform.localScale = Vector3.one * grid.cellSize;
        }
    }

    public void HideMarks(){
        marksPool.Hide();
    }
}