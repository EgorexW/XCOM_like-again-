using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

class AIBrain : MonoBehaviour{
    const float WAIT_TIME_BETWEEN_ACTIONS = 1f;
    [SerializeField] bool debug = false;
    
    [BoxGroup("References")] [Required] [SerializeField] CombatUnit combatUnit;
    [BoxGroup("References")] [Required] [SerializeField] AIBehaviour aiBehaviour;
    
    public IEnumerator ResolveTurn(){
        // Debug.Log($"Resolving turn for {combatUnit.Name}...");
        while (true){
            AIContext context = new AIContext(
                combatUnit,
                combatUnit.CombatSystem.TeamsSystem.GetEnemies(combatUnit),
                combatUnit.CombatSystem.TeamsSystem.GetAllies(combatUnit),
                this.debug
            );
            AIAction action = aiBehaviour.GetAction(context);

            if (action.IsEmpty){
                break;
            }
            
            if (action.Action is TargetedUnitAction targetedAction) {
                targetedAction.SetTarget(action.TargetNode);
            }
            if (action.Action.ValidateAction() != UnitActionValidation.Valid){
                Debug.LogWarning($"AI for unit {combatUnit.Name} attempted to execute invalid action {action.Action.ActionInfo.Name}. Ending turn.");
                break;
            }
            Debug.Log($"AI for unit {combatUnit.Name} executing action {action.Action.ActionInfo.Name}.");
            action.Action.Execute(); 
            yield return new WaitForSeconds(WAIT_TIME_BETWEEN_ACTIONS);
        }
    }
}