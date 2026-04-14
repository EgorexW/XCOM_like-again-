using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class BasicAIMovement : AIMovementBehaviour {
    [SerializeField] float exposedPenalty = 1000f;
    [SerializeField] float enemyImpactDropoffDistance = 10f;
    [SerializeField] float coverFromEnemy = 50f;
    [SerializeField] float exposedEnemy = 40f;
    [SerializeField] float distanceToEnemy = 2f;
    [SerializeField] float currentNodeBonus = 10f;
    [SerializeField] int diagonalThreshold = 2;

    public override MovementEvaluation EvaluateMovementOptions(CombatUnit unit, List<CombatGridNode> reachableNodes, List<ICombatObject> enemies, List<ICombatObject> allies){
        CombatGridNode bestNode = null;
        float highestScore = float.MinValue;
        
        foreach (var node in reachableNodes) {
            float score = EvaluateNode(node, enemies, allies);
            
            score += Random.Range(0f, 1f);

            if (score <= highestScore){
                continue;
            }
            highestScore = score;
            bestNode = node;
        }
        
        var currentNodeScore = EvaluateNode(unit.Node, enemies, allies);

        if (highestScore <= currentNodeScore + currentNodeBonus){
            return MovementEvaluation.CurrentBest;
        }
        var evaluation = new MovementEvaluation{
            bestNode = bestNode,
            nodeScore = highestScore
        };
        return evaluation;
    }

    private float EvaluateNode(CombatGridNode node, List<ICombatObject> enemies, List<ICombatObject> allies) {
        float score = 0;
        
        if (node.IsExposed(enemies)) {
            score -= exposedPenalty;
        }
        
        var shootableEnemies = node.GetExposedEnemies(enemies);
        score += exposedEnemy * shootableEnemies.Count;

        foreach (var enemy in enemies){
            float distance = node.GetDistance(enemy.Node);
            score -= distance * distanceToEnemy;
            var enemyDirection = node.GetDirections(enemy.Node, diagonalThreshold);
            foreach (var direction in enemyDirection){
                if (node.IsProtectedFrom(direction.Opposite())){
                    score += coverFromEnemy;
                }
            }
        }

        General.WorldText(score.ToString(), node.GetPos(), 1, 0.5f);

        return score;
    }
}