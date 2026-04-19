
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class AIActionPointsSwitch : AIActionCreator{
        [SerializeField] List<BasicAIActionCreator> actionCreators;
        
        public override AIAction CreateAIAction(AIContext context){
                var actionPoints = context.unit.ActionPoints;
                var evaluatorIndex = Mathf.RoundToInt(actionPoints);
                evaluatorIndex = Mathf.Clamp(evaluatorIndex, 0, actionCreators.Count - 1);
                return actionCreators[evaluatorIndex].CreateAIAction(context);
        }
    }