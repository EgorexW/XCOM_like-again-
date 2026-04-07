public static class Descriptions{
    public static string GetActionDescription(UnitAction unitAction, string actionDescription){
        actionDescription += $"Cost: {unitAction.GetCost()}";
        if (unitAction is MoveAction moveAction){
            actionDescription += $" Range: {moveAction.Range}";
        } if (unitAction is AttackAction attackAction){
            actionDescription += $" Range: {attackAction.Range}, Damage: {attackAction.Damage}";
        }
        return actionDescription;
    }
}