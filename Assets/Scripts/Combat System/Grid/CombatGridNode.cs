using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[Serializable]
public class CombatGridNode : GridNode{
    public readonly CombatGrid grid;

    [FoldoutGroup("Debug")] [ShowInInspector] [HideInEditorMode] readonly HashSet<ICombatObject> combatObjects;

    public CombatGridNode(CombatGrid grid, int x, int y) : base(x, y){
        this.grid = grid;
        combatObjects = new HashSet<ICombatObject>();
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
        return new List<ICombatObject>(combatObjects);
    }

    public override string ToString(){
        return $"Node({x}, {y}) with {combatObjects.Count} objects";
    }

    public bool Contains(CombatUnit unit){
        return combatObjects.Contains(unit);
    }
}