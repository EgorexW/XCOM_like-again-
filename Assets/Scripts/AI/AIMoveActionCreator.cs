using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

public class AIMoveActionCreator : AITargetedActionCreator{
    [SerializeField] float exposedPenalty = 100f;
    [SerializeField] float coverFromEnemy = 50f;
    [SerializeField] float exposedEnemy = 40f;
    [FormerlySerializedAs("distanceToEnemyScore")] [SerializeField] float distanceScore = 20f;
    [SerializeField] float idealDistance = 4f;
    [SerializeField] int diagonalThreshold = 2;
    [SerializeField] float distanceFalloff = 1f;

    protected override AIAction GetAIAction(AIContext context, UnitAction action){
        var validation = base.GetAIAction(context, action);
        validation.SetScore(validation.Score - EvaluateNode(context.Unit.Node, context));
        return validation;
    }

    protected override float EvaluateNode(CombatGridNode node, AIContext context) {
        float score = 0;
        var text = "";

        float totalCoverFromEnemyScore = 0;
        float totalExposedPenalty = 0;
        float totalExposedEnemiesScore = 0;
        float totalDistanceScore = 0;
        
        foreach (var enemy in context.Enemies){
            float distance = node.GetDistance(enemy.Node);
            float inverseDistance = distance * distanceFalloff;
            float diffFromIdealDistance = Mathf.Abs(idealDistance - distance);
            totalDistanceScore += distanceScore / (diffFromIdealDistance * distanceFalloff + 1);
            var enemyDirection = node.GetDirections(enemy.Node, diagonalThreshold);
            foreach (var direction in enemyDirection){
                if (node.IsProtectedFrom(direction)){
                    totalCoverFromEnemyScore += coverFromEnemy / (inverseDistance * enemyDirection.Count);
                }
            }
            if (enemy.Node.CanAttack(node)){
                totalExposedPenalty += exposedPenalty / inverseDistance;
            }
            if (node.CanAttack(enemy.Node)){
                totalExposedEnemiesScore += exposedEnemy / inverseDistance;
            }
        }

        score -= totalExposedPenalty;
        text += $"Exposed: {-totalExposedPenalty}\n";
        
        score += totalCoverFromEnemyScore;
        text += $"Cover from Enemies: {totalCoverFromEnemyScore}\n";
        
        score += totalExposedEnemiesScore;
        text += $"Exposed Enemies: {totalExposedEnemiesScore}\n";
        
        score += totalDistanceScore;
        text += $"Distance to Enemies: {totalDistanceScore}\n";
        
        text += $"Total Score: {score}\n";
        if (context.Debug){
            General.WorldText(text, node.GetPos(), 0.5f, 1);
        }

        return score;
    }
}