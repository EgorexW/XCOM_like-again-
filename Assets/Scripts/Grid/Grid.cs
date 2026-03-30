using System;
using UnityEngine;
using UnityEngine.Events;

public class Grid<TGridObject>{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;

    public class OnGridObjectChangedEventArgs : EventArgs{
        public int x;
        public int y;
    }

    readonly int width;
    readonly int height;
    readonly float cellSize;
    readonly Vector3 originPosition;
    readonly TGridObject[,] gridArray;

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
        
        for (var x = 0; x < gridArray.GetLength(0); x++)
        for (var y = 0; y < gridArray.GetLength(1); y++){
            General.WorldText(
                gridArray[x, y]?.ToString(), 
                (Vector2)(GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f), 
                30f, 
                100f, 
                Color.white
            );
            Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    public int GetWidth(){
        return width;
    }

    public int GetHeight(){
        return height;
    }

    public float GetCellSize(){
        return cellSize;
    }

    public Vector3 GetWorldPosition(int x, int y){
        return new Vector3(x, y) * cellSize + originPosition;
    }

    void GetXY(Vector2 worldPosition, out int x, out int y){
        x = Mathf.FloorToInt((worldPosition.x - originPosition.x) / cellSize);
        y = Mathf.FloorToInt((worldPosition.y - originPosition.y) / cellSize);
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
        Debug.Log($"Getting grid object at world position: {worldPosition}");
        GetXY(worldPosition, out x, out y);
        Debug.Log($"Calculated grid coordinates: X: {x}, Y: {y}");
        return GetGridObject(x, y);
    }
}