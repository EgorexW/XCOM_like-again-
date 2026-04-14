using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

class AIBrain : MonoBehaviour{
    const float WAIT_TIME_BETWEEN_ACTIONS = 1f;
    
    [BoxGroup("References")] [Required] [SerializeField] CombatUnit combatUnit;
    [BoxGroup("References")] [Required] [SerializeField] AIBehaviour aiBehaviour;
    
    public IEnumerator ResolveTurn(){
        Debug.Log($"Resolving turn for {combatUnit.Name}...");
        while (true){
            AIAction action = aiBehaviour.GetAction();

            if (action.IsEmpty){
                break;
            }
            
            if (action.Action is TargetedUnitAction targetedAction) {
                targetedAction.SetTarget(action.TargetNode);
            }
            if (action.Action.ValidateAction() != UnitActionValidation.Valid){
                Debug.LogWarning($"AI for unit {combatUnit.Name} attempted to execute invalid action {action.Action.Name}. Ending turn.");
                break;
            }
            Debug.Log($"AI for unit {combatUnit.Name} executing action {action.Action.Name}.");
            action.Action.Execute(); 
            yield return new WaitForSeconds(WAIT_TIME_BETWEEN_ACTIONS);
        }
    }
}