using System.Collections.Generic;
using UnityEngine;

public class BasicAIActionCreator : AIActionCreator{
    [SerializeField] List<UnitAction> unitActions;

    public override AIAction CreateAIAction(AIContext context){
        var highestAction = AIAction.Invalid;
        foreach (var action in unitActions){
            var newAction = GetAIAction(context, action);
            if (newAction.Score > highestAction.Score){
                highestAction = newAction;
            }
        }
        return highestAction;
    }

    protected virtual AIAction GetAIAction(AIContext context, UnitAction action){
        if (action.ValidateAction() != UnitActionValidation.Valid){
            return AIAction.Invalid;
        }
        return new AIAction(action);
    }
}