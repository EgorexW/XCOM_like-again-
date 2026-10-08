using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class CombatGrid : MonoBehaviour{
    public Grid<CombatGridNode> Grid{ get; private set; }
    [HideInInspector] public CombatSystem combatSystem;

    [FoldoutGroup("Events")] public UnityEvent<CombatGridNode> onCombatGridNodeChanged = new();
    
    public int Width => Grid.width;
    public int Height => Grid.height;

    public void Init(Vector2Int size){
        Grid = new Grid<CombatGridNode>(size.x, size.y, 1f, Vector3.zero,
            (g, x, y) => new CombatGridNode(this, x, y)
        );

        Debug.Log($"CombatGrid initialized with size: {size.x}x{size.y}");

        Grid.OnGridObjectChanged += Grid_OnGridObjectChanged;
    }

    protected void OnDestroy(){
        if (Grid != null){
            Grid.OnGridObjectChanged -= Grid_OnGridObjectChanged;
        }
    }

    void Grid_OnGridObjectChanged(object sender, Grid<CombatGridNode>.OnGridObjectChangedEventArgs e){
        var node = Grid.GetGridObject(e.x, e.y);
        onCombatGridNodeChanged.Invoke(node);
    }
    
    /// <summary>
    /// Use combatObject.MoveTo instead!!!
    /// </summary>
    public void PlaceCombatObject(ICombatObject combatObject, List<CombatGridNode> newNodes){
        foreach (var node in combatObject.Nodes) node.RemoveCombatObject(combatObject);

        foreach (var node in newNodes) node.AddCombatObject(combatObject);

        combatObject.Nodes = newNodes;
    }

    public void TriggerGridObjectChanged(CombatGridNode node){
        // Debug.Log($"A grid object was changed at coordinates X: {node.x}, Y: {node.y}");
    }

    public CombatGridNode GetNode(Vector2 pos){
        return Grid.GetGridObject(pos);
    }
    
    public bool TryGetNode(Vector2 pos, out CombatGridNode node){
        return Grid.TryGetGridObject(pos, out node);
    }

    public void RemoveCombatObject(ICombatObject combatObject){
        foreach (var node in combatObject.Nodes) node.RemoveCombatObject(combatObject);
        combatObject.Nodes.Clear();
    }

    public List<CombatGridNode> GetAllNodes(){
        return Grid.GetAllNodes();
    }
}