
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class AITargetedActionEvaluatorSwitch : AITargetedActionEvaluator{
        [FormerlySerializedAs("defaultActionEvaluator")] [FormerlySerializedAs("defaultTargetEvaluator")] [BoxGroup("References")][Required][SerializeField] AITargetedActionEvaluator defaultTargetedActionEvaluator;
        
        [SerializeField] Optional<List<AITargetedActionEvaluator>> evaluatorBasedOnActionPoints;
        
        public override TargetEvaluation EvaluateTargetedAction(AIContext aiContext, TargetedUnitAction targetedAction){
            if (evaluatorBasedOnActionPoints.Enabled){
                var actionPoints = aiContext.Unit.ActionPoints;
                var evaluatorIndex = Mathf.RoundToInt(actionPoints);
                evaluatorIndex = Mathf.Clamp(evaluatorIndex, 0, evaluatorBasedOnActionPoints.Value.Count - 1);
                return evaluatorBasedOnActionPoints.Value[evaluatorIndex].EvaluateTargetedAction(aiContext, targetedAction);
            }
            return defaultTargetedActionEvaluator.EvaluateTargetedAction(aiContext, targetedAction);
        }
    }