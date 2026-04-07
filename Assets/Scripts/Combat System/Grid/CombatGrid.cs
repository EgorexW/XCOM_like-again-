using System;
using System.Collections;
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

    protected void Awake()
    {
        grid = new Grid<CombatGridNode>(width, height, 1f, Vector3.zero, 
            (Grid<CombatGridNode> g, int x, int y) => new CombatGridNode(this, x, y)
        );
        
        grid.OnGridObjectChanged += Grid_OnGridObjectChanged;
    }

    protected void OnDestroy()
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