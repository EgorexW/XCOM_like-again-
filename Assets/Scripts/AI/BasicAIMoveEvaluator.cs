using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

public class BasicAIMoveEvaluator : BasicAIEvaluator {
    [SerializeField] float exposedPenalty = 1000f;
    [SerializeField] float coverFromEnemy = 50f;
    [SerializeField] float exposedEnemy = 40f;
    [SerializeField] float distanceToEnemyScore = 20f;
    [SerializeField] int diagonalThreshold = 2;
    [SerializeField] float distanceFalloff = 1f;

    public override TargetEvaluation EvaluateTargetedAction(AIContext context, TargetedUnitAction targetedAction){
        var validation = base.EvaluateTargetedAction(context, targetedAction);
        validation.score -= EvaluateNode(context.Unit.Node, context);
        return validation;
    }

    protected override float EvaluateNode(CombatGridNode node, AIContext context) {
        float score = 0;
        var text = "";

        float totalCoverFromEnemyScore = 0;
        float closestExposeDistance = float.MaxValue;
        float totalExposedEnemiesScore = 0;
        float totalDistanceScore = 0;
        
        foreach (var enemy in context.Enemies){
            float distance = node.GetDistance(enemy.Node);
            totalDistanceScore += distanceToEnemyScore / (distance * distanceFalloff);
            var enemyDirection = node.GetDirections(enemy.Node, diagonalThreshold);
            foreach (var direction in enemyDirection){
                if (node.IsProtectedFrom(direction)){
                    totalCoverFromEnemyScore += coverFromEnemy / (distance * distanceFalloff * enemyDirection.Count);
                }
            }
            if (enemy.Node.CanAttack(node)){
                if (distance < closestExposeDistance){
                    closestExposeDistance = distance;
                }
            }
            if (node.CanAttack(enemy.Node)){
                totalExposedEnemiesScore += exposedEnemy / (distance * distanceFalloff);
            }
        }

        score -= exposedPenalty / (closestExposeDistance * distanceFalloff);
        text += $"Exposed: {-exposedPenalty / (closestExposeDistance * distanceFalloff)}\n";
        
        score += totalCoverFromEnemyScore;
        text += $"Cover from Enemies: {totalCoverFromEnemyScore}\n";
        
        score += totalExposedEnemiesScore;
        text += $"Exposed Enemies: {totalExposedEnemiesScore}\n";
        
        score += totalDistanceScore;
        text += $"Distance to Enemies: {totalDistanceScore}\n";
        
        text += $"Total Score: {score}\n";
        if (context.debug){
            General.WorldText(text, node.GetPos(), 0.5f, 1);
        }

        return score;
    }
}