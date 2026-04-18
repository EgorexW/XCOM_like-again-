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
    // [BoxGroup("Config")][SerializeField] float surrenderChance = 0.5f;
    
    public override AIAction GetAction(AIContext context){
        // Actions
        var moveAction = moveActionCreator.CreateAIAction(context);
        var attackAction = attackActionCreator.CreateAIAction(context);
        var reloadAction = reloadActionCreator.CreateAIAction(context);
        var surrenderAction = surrenderActionCreator.CreateAIAction(context);

        // Resolution
        var exposed = combatUnit.Node.IsExposed(context.Enemies);
        var enemyExposed = attackAction.ActionFlags.HasFlag(AIActionFlags.EnemyExposed);

        if (exposed){
            if (enemyExposed){
                if (Random.value < attackWhenExposedChance){
                    return attackAction;
                }
            }
            return moveAction.Score > 0 ? moveAction : surrenderAction;
        }
        if (reloadAction.ActionFlags.HasFlag(AIActionFlags.MagazineEmpty)){
            return reloadAction;
        }
        if (enemyExposed){
            return attackAction;
        }
        if (moveAction.Score / moveScoreToMove >= Random.value){
            return moveAction;
        }
        if (attackAction.Score > 0){
            return attackAction;
        }
        return AIAction.Invalid;
    }
}