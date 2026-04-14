using System.Collections.Generic;
using UnityEngine;

public abstract class BasicAIEvaluator : AITargetEvaluator{
    public override TargetEvaluation EvaluateTargetedAction(AIContext context, TargetedUnitAction action){
        CombatGridNode bestNode = null;
        float highestScore = float.MinValue;
        var validNodes = action.GetValidTargets();
        
        foreach (var node in validNodes) {
            float score = EvaluateNode(node, context);
            
            score += Random.Range(0f, 1f);

            if (score <= highestScore){
                continue;
            }
            highestScore = score;
            bestNode = node;
        }
        
        var evaluation = new TargetEvaluation{
            bestNode = bestNode,
            score = highestScore
        };
        return evaluation;
    }

    protected abstract float EvaluateNode(CombatGridNode node, AIContext context);
}