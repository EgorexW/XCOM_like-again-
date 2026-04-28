using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class AIMoveActionCreator : AITargetedActionCreator{
    [BoxGroup("Score")][SerializeField] float exposedPenalty = 100f;
    [BoxGroup("Score")][SerializeField] float coverFromEnemy = 50f;
    [BoxGroup("Score")][SerializeField] float exposedEnemy = 40f;
    [BoxGroup("Score")][FormerlySerializedAs("distanceToEnemyScore")] [SerializeField] float distanceScore = 20f;
    [BoxGroup("Score")][SerializeField] float hazardPenalty = 100f;
    [BoxGroup("Config")] [FormerlySerializedAs("hazardScoring")] [SerializeField] AIHazardScoring aiHazardScoring;
    [BoxGroup("Config")][FormerlySerializedAs("idealDistance")] [SerializeField] float idealDistanceToEnemy = 4f;
    [BoxGroup("Config")][FormerlySerializedAs("exposedDistance")] [SerializeField] float exposedRange = 10f;
    [BoxGroup("Config")][SerializeField] int diagonalThreshold = 2;
    [BoxGroup("Config")][SerializeField] float distanceFalloff = 1f;

    protected override AIAction GetAIAction(AIContext context, UnitAction action){
        var validation = base.GetAIAction(context, action);
        validation.SetScore(validation.Score - EvaluateNode(context.unit.GetCenterNode(), context, out var flags));
        if (flags.HasFlag(AIActionFlags.TileExposed)){
            validation.AddFlag(AIActionFlags.SelfExposed);
        }
        return validation;
    }

    protected override float EvaluateNode(CombatGridNode node, AIContext context, out AIActionFlags flags){
        float score = 0;
        flags = AIActionFlags.None;
        var text = "";

        float totalCoverFromEnemyScore = 0;
        float totalExposedPenalty = 0;
        float totalExposedEnemiesScore = 0;
        float totalDistanceScore = 0;
        float totalHazardPenalty = 0;

        foreach (var enemy in context.enemies){
            var distance = node.GetDistance(enemy.GetCenterNode());
            var inverseDistance = distance * distanceFalloff + 1f;
            var diffFromIdealDistance = Mathf.Abs(idealDistanceToEnemy - distance);
            totalDistanceScore += distanceScore / (diffFromIdealDistance * distanceFalloff + 1);
            var enemyDirection = node.GetDirections(enemy.GetCenterNode(), diagonalThreshold);
            foreach (var direction in enemyDirection)
                if (node.IsProtectedFrom(direction)){
                    totalCoverFromEnemyScore += coverFromEnemy / (inverseDistance * enemyDirection.Count);
                }
            if (enemy.GetCenterNode().CanShoot(node, exposedRange, objectsToIgnore: context.allies)){
                totalExposedPenalty += exposedPenalty / inverseDistance;
                flags |= AIActionFlags.TileExposed;
            }
            if (node.CanShoot(enemy.GetCenterNode(), exposedRange)){
                totalExposedEnemiesScore += exposedEnemy / inverseDistance;
                flags |= AIActionFlags.EnemyExposed;
            }
        }
        var hazards = node.GetHazards();
        foreach (var hazard in hazards){
            var hazardMult = 1f;
            if (aiHazardScoring != null){
                hazardMult = aiHazardScoring.GetHazardScore(hazard, context);
            }
            totalHazardPenalty += hazardPenalty * hazardMult;
        }

        score -= totalExposedPenalty;
        text += $"Exposed: {-totalExposedPenalty}\n";

        score += totalCoverFromEnemyScore;
        text += $"Cover from Enemies: {totalCoverFromEnemyScore}\n";

        score += totalExposedEnemiesScore;
        text += $"Exposed Enemies: {totalExposedEnemiesScore}\n";

        score += totalDistanceScore;
        text += $"Distance to Enemies: {totalDistanceScore}\n";

        score -= totalHazardPenalty;
        text += $"Hazards: {-totalHazardPenalty}\n";

        text += $"Move Score: {score}\n";
        if (context.debug){
            General.WorldText(text, node.GetPos(), 0.5f, 1);
        }

        return score;
    }
}