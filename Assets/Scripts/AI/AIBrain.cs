using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

class AIBrain : MonoBehaviour{
    const float WAIT_TIME_BETWEEN_ACTIONS = 1f;
    [SerializeField] bool debug;

    [FormerlySerializedAs("combatUnit")] [BoxGroup("References")] [Required] [SerializeField] Unit unit;
    [BoxGroup("References")] [Required] [SerializeField] AIBehaviour aiBehaviour;

    public IEnumerator ResolveTurn(){
        // Debug.Log($"Resolving turn for {combatUnit.Name}...");
        while (true){
            var context = new AIContext(
                unit,
                unit.CombatSystem.TeamsSystem.GetEnemies(unit),
                unit.CombatSystem.TeamsSystem.GetAllies(unit),
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
                    $"AI for unit {unit.Name} attempted to execute invalid action {action.action.ActionInfo.Name}. Ending turn.");
                break;
            }
            if (context.debug){
                Debug.Log($"AI for unit {unit.Name} executing action {action.action.ActionInfo.Name}.");
            }
            action.action.Execute();
            yield return new WaitForSeconds(WAIT_TIME_BETWEEN_ACTIONS);
        }
    }
}