using Sirenix.OdinInspector;
using UnityEngine;

public class GridUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] ObjectsPool objectsPool;

    public void ShowGrid<T>(Grid<T> grid){ ;
        var count = grid.width * grid.height;
        objectsPool.SetCount(count);
        int i = 0;
        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                GameObject cellObj = objectsPool.GetActiveObject(i);
                cellObj.transform.position = grid.GetWorldPosition(x, y);
                cellObj.transform.localScale = Vector3.one * grid.cellSize;
                i++;
            }
        }
    }
}