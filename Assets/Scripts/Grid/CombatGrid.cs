using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class CombatGrid : MonoBehaviour
{
    [SerializeField] int width = 100;
    [SerializeField] int height = 100;
        
    private Grid<CombatGridNode> grid;

    [FoldoutGroup("Events")] public UnityEvent<CombatGridNode> onCombatGridNodeChanged = new();

    void Awake()
    {
        grid = new Grid<CombatGridNode>(width, height, 1f, Vector3.zero, 
            (Grid<CombatGridNode> g, int x, int y) => new CombatGridNode(this, x, y)
        );
        
        grid.OnGridObjectChanged += Grid_OnGridObjectChanged;
    }
    
    private void OnDestroy()
    {
        if (grid != null)
        {
            grid.OnGridObjectChanged -= Grid_OnGridObjectChanged;
        }
    }

    void Grid_OnGridObjectChanged(object sender, Grid<CombatGridNode>.OnGridObjectChangedEventArgs e){
        CombatGridNode node = grid.GetGridObject(e.x, e.y);
        onCombatGridNodeChanged.Invoke(node);
    }

    public void PlaceCombatObject(ICombatObject combatObject, Vector2Int pos){
        if (combatObject.Node != null){
            combatObject.Node.RemoveCombatObject(combatObject);
        }
        var newNode = grid.GetGridObject(pos.x, pos.y);
        newNode.AddCombatObject(combatObject);
        combatObject.Node = newNode;
    }

    public void MoveCombatObject(Vector2Int oldPos, Vector2Int newPos, ICombatObject combatObject){
        grid.GetGridObject(oldPos).RemoveCombatObject(combatObject);
        Debug.Log($"Combat object: {combatObject} moved from {oldPos} to {newPos}");
        PlaceCombatObject(combatObject, newPos);
    }

    public void TriggerGridObjectChanged(CombatGridNode node){
        Debug.Log($"A grid object was changed at coordinates X: {node.x}, Y: {node.y}");
    }

    public CombatGridNode GetNode(Vector2 pos){
        return grid.GetGridObject(pos);
    }
}

public class CombatGridNode{
    public readonly CombatGrid grid;
    public readonly int x;
    public readonly int y;

    List<ICombatObject> combatObjects;
    
    public bool IsOccupied => combatObjects.Find(co => co.OccupiesTile) != null;

    public CombatGridNode(CombatGrid grid, int x, int y){
        this.grid = grid;
        this.x = x;
        this.y = y;
        combatObjects = new List<ICombatObject>();
    }

    public void AddCombatObject(ICombatObject combatObject){
        combatObjects.Add(combatObject);
        grid.TriggerGridObjectChanged(this);
    }
    
    public void RemoveCombatObject(ICombatObject combatObject){
        combatObjects.Remove(combatObject);
        grid.TriggerGridObjectChanged(this);
    }
    
    public List<ICombatObject> GetCombatObjects(){
        return combatObjects.Copy();
    }

    public Vector2Int GetPos(){
        return new Vector2Int(x, y);
    }
}