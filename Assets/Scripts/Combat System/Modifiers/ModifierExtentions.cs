using System;
using System.Collections.Generic;

public static class ModifierExtentions{
    public static List<UnitModifier> GetModifiersOfType(this Unit unit, Type type){
        var modifiers = new List<UnitModifier>();
        foreach (var modifier in unit.ActiveStatuses)
            if (modifier.GetType() == type){
                modifiers.Add(modifier);
            }
        return modifiers;
    }
}