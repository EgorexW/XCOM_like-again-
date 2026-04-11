using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[Serializable]
public class CombatGridNode : GridNode{
    public readonly CombatGrid grid;

    [FoldoutGroup("Debug")][ShowInInspector][HideInEditorMode] readonly List<ICombatObject> combatObjects;

    public bool IsOccupied => combatObjects.Find(co => co.OccupiesTile) != null;

    public CombatGridNode(CombatGrid grid, int x, int y) : base(x, y){
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