public static class AIHelpers{
    public static ICombatObject GetClosestEnemy(this AIContext context){
        ICombatObject closestEnemy = null;
        var closestDistance = float.MaxValue;

        foreach (var enemy in context.enemies){
            var dist = context.unit.GetDistance(enemy);

            if (dist < closestDistance){
                closestDistance = dist;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}