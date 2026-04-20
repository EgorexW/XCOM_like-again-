using UnityEngine;

public static class AIHelpers{
    public static ICombatObject GetClosestEnemy(this AIContext context){
        ICombatObject closestEnemy = null;
        float closestDistance = float.MaxValue; 

        foreach (var enemy in context.enemies){
            float dist = context.unit.GetDistance(enemy);
            
            if (dist < closestDistance){
                closestDistance = dist;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}