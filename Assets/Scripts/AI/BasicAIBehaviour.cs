using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class BasicAIBehaviour : AIBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] AIMovementBehaviour aiMovementBehaviour;
    [Required][BoxGroup("ActionReferences")][SerializeField] TargetedUnitAction movementAction;
    [Required][BoxGroup("ActionReferences")][SerializeField] TargetedUnitAction attackAction;
    [Required][BoxGroup("ActionReferences")][SerializeField] UnitAction reloadAction;

    [BoxGroup("Config")][SerializeField] float attackWhenExposedChance = 0.5f;
    public override AIAction GetAction(){
        var enemies = combatUnit.CombatSystem.TeamsSystem.GetEnemies(combatUnit);
        var allies = combatUnit.CombatSystem.TeamsSystem.GetAllies(combatUnit);
        var movementEvaluation = aiMovementBehaviour.EvaluateMovementOptions(combatUnit, movementAction.GetValidTargets(), enemies, allies);

        // Circumstances
        HashSet<Circumstance> circumstances = new ();

        if (combatUnit.Node.IsExposed(enemies)){
            circumstances.Add(Circumstance.Exposed);
        }

        if (movementEvaluation.CurrentNodeBest){
            circumstances.Add(Circumstance.NotWantToMove);
            circumstances.Remove(Circumstance.Exposed);
        }
        
        var enemiesExposed = GetExposedEnemies(enemies);
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

        // Resolution
        if (circumstances.Contains(Circumstance.EnemyExposed)){
            if (circumstances.Contains(Circumstance.Exposed)){
                if (Random.value < attackWhenExposedChance){
                    return GetAttackAction(enemiesExposed.Random());
                }
                return GetMoveAction(movementEvaluation.bestNode);
            }
            return GetAttackAction(enemiesExposed.Random());
        }
        if (circumstances.Contains(Circumstance.Exposed)){
            return GetMoveAction(movementEvaluation.bestNode);
        }
        if (circumstances.Contains(Circumstance.MustReload)){
            return GetReloadAction();
        }
        if (circumstances.Contains(Circumstance.NotWantToMove)){
            return AIAction.Empty; // TODO come up with something then
        }
        return GetMoveAction(movementEvaluation.bestNode);
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

    AIAction GetAttackAction(ICombatObject target){
        var action = new AIAction{
            Action = attackAction,
            TargetNode = target.Node
        };
        return action;
    }

    enum Circumstance{
        Exposed,
        EnemyExposed,
        MustReload,
        NotWantToMove
    }
    
    public List<ICombatObject> GetExposedEnemies(List<ICombatObject> enemies){ // TODO invorporate attack range
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