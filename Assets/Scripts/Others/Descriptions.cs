public static class Descriptions{
    public static string GetActionDescription(UnitAction unitAction, string actionDescription){
        actionDescription += $"Cost: {unitAction.GetCost()}";
        if (unitAction is TargetedUnitAction targetedUnitAction){
            actionDescription += $" Range: {targetedUnitAction.Range}";
        } 
        if (unitAction is AttackAction attackAction){
            actionDescription += $" Damage: {attackAction.Damage}";
        }
        return actionDescription;
    }
}