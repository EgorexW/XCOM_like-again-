using System;
using UnityEngine;
using UnityEngine.Events;

public class Grid<TGridObject>{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;

    public class OnGridObjectChangedEventArgs : EventArgs{
        public int x;
        public int y;
    }

    public readonly int width;
    public readonly int height;
    public readonly float cellSize;
    public readonly Vector3 originPosition;
    public readonly TGridObject[,] gridArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition,
        Func<Grid<TGridObject>, int, int, TGridObject> createGridObject){
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];

        for (var x = 0; x < gridArray.GetLength(0); x++)
        for (var y = 0; y < gridArray.GetLength(1); y++)
            gridArray[x, y] = createGridObject(this, x, y);
    }

    public Vector3 GetWorldPosition(int x, int y){
        return new Vector3(x, y) * cellSize + originPosition;
    }

    void GetXY(Vector2 worldPosition, out int x, out int y){
        x = Mathf.RoundToInt((worldPosition.x - originPosition.x) / cellSize);
        y = Mathf.RoundToInt((worldPosition.y - originPosition.y) / cellSize);
    }

    public void SetGridObject(int x, int y, TGridObject value){
        if (x >= 0 && y >= 0 && x < width && y < height){
            gridArray[x, y] = value;
            if (OnGridObjectChanged != null){
                OnGridObjectChanged(this, new OnGridObjectChangedEventArgs{ x = x, y = y });
            }
        }
    }

    public void SetGridObject(Vector2 worldPosition, TGridObject value){
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    public TGridObject GetGridObject(int x, int y){
        if (x >= 0 && y >= 0 && x < width && y < height){
            return gridArray[x, y];
        }
        Debug.LogWarning("Trying to get grid object out of bounds");
        return default;
    }

    public TGridObject GetGridObject(Vector2 worldPosition){
        int x, y;
        // Debug.Log($"Getting grid object at world position: {worldPosition}");
        GetXY(worldPosition, out x, out y);
        // Debug.Log($"Calculated grid coordinates: X: {x}, Y: {y}");
        return GetGridObject(x, y);
    }
}

public abstract class GridNode{
    public readonly int x;
    public readonly int y;

    protected GridNode(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    
    public Vector2Int GetPos(){
        return new Vector2Int(x, y);
    }
}