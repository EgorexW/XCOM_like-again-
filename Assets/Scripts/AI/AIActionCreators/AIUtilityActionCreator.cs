using UnityEngine;

public class AIUtilityActionCreator : AITargetedActionCreator{
    [SerializeField] float baseScore = -15f;
    [SerializeField] float enemyPresent = 100f;
    [SerializeField] float allyPresentPenalty = 50f;
    [SerializeField] float hazardAlreadyPresentPenalty = 200f;
    [SerializeField] float spotOnMultiplier = 2f;
    [SerializeField] float range = 1.5f;
    [SerializeField] float distanceFalloff = 0.1f;

    protected override float EvaluateNode(CombatGridNode node, AIContext context, out AIActionFlags flags) {
        float score = 0;
        flags = AIActionFlags.None;
        var text = "";

        float distance = node.GetDistance(context.Unit.GetCenterNode());
        
        foreach (var enemy in context.Enemies){
            Score(enemy, enemyPresent);
        }

        foreach (var allie in context.Allies){
            Score(allie, -allyPresentPenalty);
        }

        var hazards = node.GetHazards();
        foreach (var hazard in hazards){
            score -= hazardAlreadyPresentPenalty * hazard.Intensity;
        }
        
        var invertedDistance = (1f + distance * distanceFalloff);
        score /= invertedDistance;
        
        score += baseScore;
        
        text += $"Utility Score: {score}\n";
        if (context.Debug){
            General.WorldText(text, node.GetPos(), 0.5f, 1);
        }

        return score;

        void Score(ICombatObject enemy, float presentScore){
            float distanceToEnemy = node.GetDistance(enemy.GetCenterNode());
            if (distanceToEnemy > range){
                return;
            }
            float mult = (spotOnMultiplier - 1) * (1 - (distanceToEnemy / range));
            mult += 1;
            score += presentScore * mult;
        }
    }
}