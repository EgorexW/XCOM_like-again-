using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class BasicAIBehaviour : AIBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] AIActionCreator moveActionCreator;
    [BoxGroup("References")] [Required] [SerializeField] AIActionCreator attackActionCreator;
    [BoxGroup("References")] [Required] [SerializeField] AIActionCreator reloadActionCreator;
    [BoxGroup("References")] [Required] [SerializeField] AIActionCreator surrenderActionCreator;

    [BoxGroup("Config")][SerializeField] float attackWhenExposedChance = 0.5f;
    [BoxGroup("Config")][SerializeField] float moveScoreToMove = 5;
    [BoxGroup("Config")][SerializeField] float surrenderChance = 0.5f;
    
    public override AIAction GetAction(AIContext context){
        // Actions
        var moveAction = moveActionCreator.CreateAIAction(context);
        var attackAction = attackActionCreator.CreateAIAction(context);
        var reloadAction = reloadActionCreator.CreateAIAction(context);
        var surrenderAction = surrenderActionCreator.CreateAIAction(context);

        // Circumstances
        HashSet<Circumstance> circumstances = new ();

        if (combatUnit.Node.IsExposed(context.Enemies)){
            circumstances.Add(Circumstance.Exposed);
        }
        
        // var enemiesExposed = GetExposedEnemies(context.Enemies);
        // if (enemiesExposed.Count > 0){
        //     circumstances.Add(Circumstance.EnemyExposed);
        // }
        
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
                    return attackAction;
                }
                if (Random.value < attackWhenExposedChance){
                    return attackAction;
                }
                return moveAction;
            }
            return attackAction;
        }
        if (circumstances.Contains(Circumstance.Exposed)){
            if (moveAction.Score > 0){
                return moveAction;
            }
            if (Random.value < surrenderChance){
                return surrenderAction;
            }
        }
        if (circumstances.Contains(Circumstance.MustReload)){
            return reloadAction;
        }
        if (moveAction.Score / moveScoreToMove >= Random.value){
            return moveAction;
        }
        if (attackAction.Score > 0){
            return attackAction;
        }
        return AIAction.Invalid;
    }

    enum Circumstance{
        Exposed,
        EnemyExposed,
        MustReload,
        LastAction
    }
}