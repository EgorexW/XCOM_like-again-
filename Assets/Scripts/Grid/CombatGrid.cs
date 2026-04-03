using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class CombatGrid : MonoBehaviour
{
    [SerializeField] int width = 100;
    [SerializeField] int height = 100;
        
    private Grid<CombatGridNode> grid;
    
    public Grid<CombatGridNode> Grid => grid;

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

    public void TriggerGridObjectChanged(CombatGridNode node){
        Debug.Log($"A grid object was changed at coordinates X: {node.x}, Y: {node.y}");
    }

    public CombatGridNode GetNode(Vector2 pos){
        return grid.GetGridObject(pos);
    }

    public void RemoveCombatObject(ICombatObject arg0){
        if (arg0.Node != null){
            arg0.Node.RemoveCombatObject(arg0);
            arg0.Node = null;
        }
    }
}

public class CombatGridNode : GridNode{
    public readonly CombatGrid grid;

    List<ICombatObject> combatObjects;
    
    public bool IsOccupied => combatObjects.Find(co => co.OccupiesTile) != null;

    public CombatGridNode(CombatGrid grid, int x, int y) : base(x,y){
        this.grid = grid;
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
}

public static class CombatGridExtensions{
    public static bool InStraightLine(this CombatGridNode node1, CombatGridNode node2){
        if (node1.x != node2.x && node1.y != node2.y){
            return false;
        }
        return true;
    }
    public static bool LineUnobstructed(this CombatGridNode node1, CombatGridNode node2){
        foreach (var node in GetNodesInBetween(node1, node2)){
            if (node.IsOccupied){
                return false;
            }
        }
        return true;
    }

    public static List<CombatGridNode> GetNodesInBetween(this CombatGridNode node1, CombatGridNode node2)
    {
    var nodes = new List<CombatGridNode>();

    int x0 = node1.x;
    int y0 = node1.y;
    int x1 = node2.x;
    int y1 = node2.y;

    int dx = Mathf.Abs(x1 - x0);
    int dy = Mathf.Abs(y1 - y0);
    
    int stepX = x0 < x1 ? 1 : -1;
    int stepY = y0 < y1 ? 1 : -1;
    
    // We multiply by 2 so we can do perfect integer math without decimals
    int error = dx - dy;
    int dx2 = dx * 2;
    int dy2 = dy * 2;

    int x = x0;
    int y = y0;

    while (true)
    {
        nodes.Add(node1.grid.GetNode(new Vector2Int(x, y)));

        // We reached the target!
        if (x == x1 && y == y1) break;

        // Step Horizontally
        if (error > 0)
        {
            x += stepX;
            error -= dy2;
        }
        // Step Vertically
        else if (error < 0)
        {
            y += stepY;
            error += dx2;
        }
        // THE MAGIC BLOCK: error == 0
        // The line passes exactly through a 4-way intersection!
        else 
        {
            // 1. Grab the Horizontal Corner
            int cornerX1 = x + stepX;
            int cornerY1 = y;
            nodes.Add(node1.grid.GetNode(new Vector2Int(cornerX1, cornerY1)));

            // 2. Grab the Vertical Corner
            int cornerX2 = x;
            int cornerY2 = y + stepY;
            nodes.Add(node1.grid.GetNode(new Vector2Int(cornerX2, cornerY2)));

            // 3. Finally, move the main tracker diagonally
            x += stepX;
            y += stepY;
            error -= dy2;
            error += dx2;
        }
    }
    
    nodes.Remove(node1);
    nodes.Remove(node2);

    return nodes;
}
    
    public static float GetDistance(this CombatGridNode node1, CombatGridNode node2){
        return Mathf.Abs(node1.x - node2.x) + Mathf.Abs(node1.y - node2.y);
    }
}
