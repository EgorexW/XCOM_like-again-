using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class BasicAIBehaviour : AIBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] AITargetEvaluator aiMovementBehaviour;
    [BoxGroup("References")] [Required] [SerializeField] AITargetEvaluator aiAttackBehaviour;
    [Required][BoxGroup("ActionReferences")][SerializeField] TargetedUnitAction movementAction;
    [Required][BoxGroup("ActionReferences")][SerializeField] TargetedUnitAction attackAction;
    [Required][BoxGroup("ActionReferences")][SerializeField] UnitAction reloadAction;

    [BoxGroup("Config")][SerializeField] float attackWhenExposedChance = 0.5f;
    [BoxGroup("Config")][SerializeField] float moveScoreToMove = 5;
    
    public override AIAction GetAction(AIContext context){
        var movementEvaluation = aiMovementBehaviour.EvaluateTargetedAction(context, movementAction);
        var attackEvaluation = aiAttackBehaviour.EvaluateTargetedAction(context, attackAction);

        // Circumstances
        HashSet<Circumstance> circumstances = new ();

        if (combatUnit.Node.IsExposed(context.Enemies)){
            circumstances.Add(Circumstance.Exposed);
        }
        
        if (movementEvaluation.score <= 0){
            circumstances.Add(Circumstance.NoBetterTile);
            circumstances.Remove(Circumstance.Exposed);
        }
        
        var enemiesExposed = GetExposedEnemies(context.Enemies);
        if (enemiesExposed.Count > 0){
            circumstances.Add(Circumstance.EnemyExposed);
        }
        
        var ammoComponent = combatUnit.GetCombatComponent<AmmoComponent>();
        if (ammoComponent != null){
            if (ammoComponent.IsEmpty){
                circumstances.Add(Circumstance.MustReload);
                circumstances.Remove(Circumstance.EnemyExposed);
            }
        }

        if (context.Unit.ActionPoints < 2){
            circumstances.Add(Circumstance.LastAction);
        }

        // Resolution
        if (circumstances.Contains(Circumstance.EnemyExposed)){
            if (circumstances.Contains(Circumstance.Exposed)){
                if (!circumstances.Contains(Circumstance.LastAction)){
                    return GetAttackAction(attackEvaluation.bestNode);
                }
                if (Random.value < attackWhenExposedChance){
                    return GetAttackAction(attackEvaluation.bestNode);
                }
                return GetMoveAction(movementEvaluation.bestNode);
            }
            return GetAttackAction(attackEvaluation.bestNode);
        }
        if (circumstances.Contains(Circumstance.Exposed)){
            return GetMoveAction(movementEvaluation.bestNode);
        }
        if (circumstances.Contains(Circumstance.MustReload)){
            return GetReloadAction();
        }
        if (movementEvaluation.score / moveScoreToMove >= Random.value){
            return GetMoveAction(movementEvaluation.bestNode);
        }
        if (attackEvaluation.score > 0){
            return GetAttackAction(attackEvaluation.bestNode);
        }
        return AIAction.Empty;
    }

    AIAction GetReloadAction(){
            var action = new AIAction{
                Action = reloadAction
            };
            return action;
    }

    public AIAction GetMoveAction(CombatGridNode targetNode) {
        if (targetNode != null) {
            return new AIAction {
                Action = movementAction,
                TargetNode = targetNode
            };
        }
        return AIAction.Empty;
    }

    AIAction GetAttackAction(CombatGridNode target){
        var action = new AIAction{
            Action = attackAction,
            TargetNode = target
        };
        return action;
    }

    enum Circumstance{
        Exposed,
        EnemyExposed,
        MustReload,
        NoBetterTile,
        LastAction
    }
    
    public List<ICombatObject> GetExposedEnemies(List<ICombatObject> enemies){
        var exposedEnemies = new List<ICombatObject>();
        foreach (var enemy in enemies){
            if (attackAction.ValidateTarget(enemy.Node) != TargetValidation.Valid){
                continue;
            }
            exposedEnemies.Add(enemy);
        }
        return exposedEnemies;
    }
}