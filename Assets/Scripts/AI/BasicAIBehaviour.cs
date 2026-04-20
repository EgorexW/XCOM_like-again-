using Sirenix.OdinInspector;
using UnityEngine;

public class BasicAIBehaviour : AIBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] AIActionCreator moveActionCreator;
    [BoxGroup("References")] [Required] [SerializeField] AIActionCreator attackActionCreator;
    [BoxGroup("References")] [Required] [SerializeField] AIActionCreator reloadActionCreator;
    [BoxGroup("References")] [Required] [SerializeField] AIActionCreator surrenderActionCreator;
    [BoxGroup("References")] [Required] [SerializeField] AIActionCreator utilityActionCreator;

    [BoxGroup("Config")] [SerializeField] float minDisToEnemyToAgress = 13f;
    [BoxGroup("Config")] [SerializeField] float attackWhenExposedChance = 0.5f;
    [BoxGroup("Config")] [SerializeField] float moveScoreToMove = 10;
    [BoxGroup("Config")] [SerializeField] float utilityChance = 0.25f;

    public override AIAction GetAction(AIContext context){
        var closestEnemy = context.GetClosestEnemy();
        if (closestEnemy != null){
            var distance = context.unit.GetDistance(closestEnemy);
            if (distance > minDisToEnemyToAgress){
                return AIAction.Invalid;
            }
        }

        // Actions
        var moveAction = moveActionCreator.CreateAIAction(context);
        var attackAction = attackActionCreator.CreateAIAction(context);
        var reloadAction = reloadActionCreator.CreateAIAction(context);
        var surrenderAction = surrenderActionCreator.CreateAIAction(context);
        var utilityAction = utilityActionCreator.CreateAIAction(context);

        // Resolution
        var exposed = moveAction.ActionFlags.HasFlag(AIActionFlags.SelfExposed);
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
        if (Random.value < utilityChance){
            if (utilityAction.Score > 0){
                return utilityAction;
            }
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