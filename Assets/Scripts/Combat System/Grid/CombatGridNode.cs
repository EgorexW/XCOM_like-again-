using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

[Serializable]
public class CombatGridNode : GridNode{
    public readonly CombatGrid grid;

    [FoldoutGroup("Debug")][ShowInInspector][HideInEditorMode] readonly HashSet<ICombatObject> combatObjects;

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

    public HashSet<ICombatObject> GetCombatObjects(){
        return new HashSet<ICombatObject>(combatObjects);
    }
    
    public bool CanAcceptObject(GridOccupancyType newObjectType, List<ICombatObject> objectsToIgnore = null) {
        objectsToIgnore ??= new List<ICombatObject>();
        var localObjects = GetCombatObjects();
        localObjects.RemoveWhere(o => objectsToIgnore.Contains(o));
        if (localObjects.Any(co => co.OccupancyType == GridOccupancyType.Wall)) {
            return false; 
        }
        
        if (localObjects.Any(co => co.OccupancyType == GridOccupancyType.Character)) {
            if (newObjectType == GridOccupancyType.Character || newObjectType == GridOccupancyType.Wall) {
                return false;
            }
        }

        return true; 
    }

    public override string ToString(){
        return $"Node({x}, {y}) with {combatObjects.Count} objects";
    }

    public bool Contains(CombatUnit unit){
        return combatObjects.Contains(unit);
    }
}

public enum GridOccupancyType{
    Wall,
    Character,
    Other
}