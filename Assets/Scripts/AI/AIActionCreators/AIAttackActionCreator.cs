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
        float score = baseScore;
        flags = AIActionFlags.None;
        var text = "";

        float distance = node.GetDistance(context.Unit.Node);
        
        foreach (var enemy in context.Enemies){
            float distanceToEnemy = node.GetDistance(enemy.Node);
            if (node.GetCombatObjects().Contains(enemy)){
                score += enemyPresent / (distance * distanceFalloff);
                flags |= AIActionFlags.EnemyExposed;
            }
            if (context.Unit.Node.GetNodesInBetween(enemy.Node).Contains(node)){
                score += blockPathToEnemy / (distance * distanceFalloff * distanceToEnemy);
            }
        }

        foreach (var allie in context.Allies){
            if (node.GetCombatObjects().Contains(allie)){
                score -= allyPresentPenalty;
            }
        }
        
        text += $"Total Score: {score}\n";
        if (context.Debug){
            General.WorldText(text, node.GetPos(), 0.5f, 1);
        }

        return score;
    }
}