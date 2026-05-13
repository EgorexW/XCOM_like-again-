using Sirenix.OdinInspector;
using UnityEngine;

public class BasicAIBehaviour : AIBehaviour{
    [BoxGroup("References")] [SerializeField] AIActionCreator moveActionCreator;
[BoxGroup("References")] [Required] [SerializeField] AIActionCreator attackActionCreator;
    [BoxGroup("References")] [SerializeField] AIActionCreator reloadActionCreator;
    [BoxGroup("References")] [SerializeField] AIActionCreator surrenderActionCreator;
    [BoxGroup("References")] [SerializeField] AIActionCreator utilityActionCreator;
    [BoxGroup("References")] [SerializeField] AIActionCreator suppressActionCreator;

    [BoxGroup("Config")] [SerializeField] float minDisToEnemyToAgress = 13f;
    [BoxGroup("Config")] [SerializeField] float attackWhenExposedChance = 0.5f;
    [BoxGroup("Config")] [SerializeField] float moveScoreToMove = 10;
    [BoxGroup("Config")] [SerializeField] float suppressChance = 0.25f;
    [BoxGroup("Config")] [SerializeField] float utilityChance = 0.25f;

    public override AIAction GetAction(AIContext context){
        var closestEnemy = context.GetClosestEnemy();
        if (closestEnemy == null){
            return AIAction.Invalid;
        }
        var closestDis = context.unit.GetDistance(closestEnemy);
        if (closestDis > minDisToEnemyToAgress){
            return AIAction.Invalid;
        }

        // Actions
        var moveAction = moveActionCreator != null ? moveActionCreator.CreateAIAction(context) : AIAction.Invalid;
        var attackAction = attackActionCreator != null ? attackActionCreator.CreateAIAction(context) : AIAction.Invalid;
        var reloadAction = reloadActionCreator != null ? reloadActionCreator.CreateAIAction(context) : AIAction.Invalid;
        var surrenderAction = surrenderActionCreator != null ? surrenderActionCreator.CreateAIAction(context) : AIAction.Invalid;
        var utilityAction = utilityActionCreator != null ? utilityActionCreator.CreateAIAction(context) : AIAction.Invalid;
        var suppressAction = suppressActionCreator != null ? suppressActionCreator.CreateAIAction(context) : AIAction.Invalid;

        // Resolution
        var exposed = moveAction.ActionFlags.HasFlag(AIActionFlags.InDanger);
        var enemyExposed = attackAction.ActionFlags.HasFlag(AIActionFlags.EnemyExposed);

        if (exposed){
            if (enemyExposed){
                if (Random.value < attackWhenExposedChance && attackAction.Valid){
                    return attackAction;
                }
            }
            return moveAction.Score > 0 ? moveAction : surrenderAction;
        }
        if (reloadAction.ActionFlags.HasFlag(AIActionFlags.MagazineEmpty) && reloadAction.Valid){
            return reloadAction;
        }
        if (enemyExposed && attackAction.Valid){
            return attackAction;
        }
        if (Random.value < suppressChance){
            if (suppressAction.Score > 0){
                // Debug.Log($"[AI] Decided to Suppress. Target Node: {(suppressAction.targetNode != null ? suppressAction.targetNode.GetPos().ToString() : "NULL")}. Score: {suppressAction.Score}");
                return suppressAction;
            }
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