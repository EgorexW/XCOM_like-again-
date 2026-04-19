using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class AIAttackActionCreator : AITargetedActionCreator{
    [SerializeField] float baseScore = -5f;
    [SerializeField] float enemyPresent = 50f;
    [SerializeField] float blockPathToEnemy = 20f;
    [SerializeField] float allyPresentPenalty = 1000f;
    [SerializeField] float distanceFalloff = 0.5f;
    
    protected override float EvaluateNode(CombatGridNode node, AIContext context, out AIActionFlags flags) {
        float score = 0;
        flags = AIActionFlags.None;
        var text = "";

        float distance = node.GetDistance(context.Unit.GetCenterNode());
        
        foreach (var enemy in context.Enemies){
            if (node.GetCombatObjects().Contains(enemy)){
                score += enemyPresent;
                flags |= AIActionFlags.EnemyExposed;
            }
            if (context.Unit.GetCenterNode().GetNodesInBetween(enemy.GetCenterNode()).Contains(node)){
                float distanceToEnemy = node.GetDistance(enemy.GetCenterNode());
                var diffBetweenDistanceToSelfAndEnemy = distance - distanceToEnemy;
                var totalDistance = distance + distanceToEnemy;
                var normalizedDiff = diffBetweenDistanceToSelfAndEnemy / totalDistance;
                score += blockPathToEnemy * normalizedDiff;
            }
        }

        foreach (var allie in context.Allies){
            if (node.GetCombatObjects().Contains(allie)){
                score -= allyPresentPenalty;
            }
        }
        
        var invertedDistance = (1f + distance * distanceFalloff);
        score /= invertedDistance;

        score += baseScore;
        
        text += $"Attack Score: {score}\n";
        if (context.Debug){
            General.WorldText(text, node.GetPos(), 0.5f, 1);
        }

        return score;
    }
}