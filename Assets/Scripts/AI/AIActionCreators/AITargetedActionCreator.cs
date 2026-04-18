using System.Collections.Generic;
using UnityEngine;

public abstract class AITargetedActionCreator : BasicAIActionCreator{
    protected override AIAction GetAIAction(AIContext context, UnitAction action){
        if (action is not TargetedUnitAction targetedAction){
            Debug.LogWarning("Action is not TargetedAction", this);
            return AIAction.Invalid;
        }
        
        var validation = action.ValidateAction();

        if (validation != UnitActionValidation.Valid && validation != UnitActionValidation.InvalidTarget){
            return AIAction.Invalid;
        }
        
        CombatGridNode bestNode = null;
        float highestScore = float.MinValue;
        var flags = AIActionFlags.None;
        var validNodes = targetedAction.GetValidTargets();
        
        foreach (var node in validNodes) {
            float score = EvaluateNode(node, context, out var flagsTmp);
            
            score += Random.Range(0f, 1f);

            if (score <= highestScore){
                continue;
            }
            highestScore = score;
            bestNode = node;
            flags = flagsTmp;
        }

        var evaluation = new AIAction(action, bestNode, highestScore, flags);
        return evaluation;
    }

    protected abstract float EvaluateNode(CombatGridNode node, AIContext context, out AIActionFlags flags);
}