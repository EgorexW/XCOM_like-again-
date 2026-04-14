
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class AITargetEvaluatorSwitch : AITargetEvaluator{
        [BoxGroup("References")][Required][SerializeField] AITargetEvaluator defaultTargetEvaluator;
        
        [SerializeField] Optional<List<AITargetEvaluator>> evaluatorBasedOnActionPoints;
        
        public override TargetEvaluation EvaluateTargetedAction(AIContext aiContext, TargetedUnitAction targetedAction){
            if (evaluatorBasedOnActionPoints.Enabled){
                var actionPoints = aiContext.Unit.ActionPoints;
                var evaluatorIndex = Mathf.RoundToInt(actionPoints);
                evaluatorIndex = Mathf.Clamp(evaluatorIndex, 0, evaluatorBasedOnActionPoints.Value.Count - 1);
                return evaluatorBasedOnActionPoints.Value[evaluatorIndex].EvaluateTargetedAction(aiContext, targetedAction);
            }
            return defaultTargetEvaluator.EvaluateTargetedAction(aiContext, targetedAction);
        }
    }