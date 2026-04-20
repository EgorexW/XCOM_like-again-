using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

class AIBrain : MonoBehaviour{
    const float WAIT_TIME_BETWEEN_ACTIONS = 1f;
    [SerializeField] bool debug;

    [BoxGroup("References")] [Required] [SerializeField] CombatUnit combatUnit;
    [BoxGroup("References")] [Required] [SerializeField] AIBehaviour aiBehaviour;

    public IEnumerator ResolveTurn(){
        // Debug.Log($"Resolving turn for {combatUnit.Name}...");
        while (true){
            var context = new AIContext(
                combatUnit,
                combatUnit.CombatSystem.TeamsSystem.GetEnemies(combatUnit),
                combatUnit.CombatSystem.TeamsSystem.GetAllies(combatUnit),
                debug
            );
            var action = aiBehaviour.GetAction(context);

            if (!action.Valid){
                break;
            }

            if (action.action is TargetedUnitAction targetedAction){
                targetedAction.SetTarget(action.targetNode);
            }
            if (action.action.ValidateAction() != UnitActionValidation.Valid){
                Debug.LogWarning(
                    $"AI for unit {combatUnit.Name} attempted to execute invalid action {action.action.ActionInfo.Name}. Ending turn.");
                break;
            }
            if (context.debug){
                Debug.Log($"AI for unit {combatUnit.Name} executing action {action.action.ActionInfo.Name}.");
            }
            action.action.Execute();
            yield return new WaitForSeconds(WAIT_TIME_BETWEEN_ACTIONS);
        }
    }
}