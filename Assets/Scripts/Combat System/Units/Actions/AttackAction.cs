using UnityEngine;

public class AttackAction : UnitAction{
    [SerializeField] int range = 10;
    [SerializeField] float damage = 1;
    
    CombatGridNode targetNode;

    protected override void OnExecute(){
        var targetObjects = targetNode.GetCombatObjects();
        foreach (var targetObj in targetObjects){
            var healthComp = targetObj.GetCombatComponent<HealthComponent>();
            if (healthComp != null){
                healthComp.TakeDamage(damage);
            }
        }
    }

    public override void SetTarget(Vector2 pos){
        targetNode = unit.Grid.GetNode(pos);
    }

    protected override bool HasValidTarget(){
        if (targetNode == null){
            return false;
        }
        var targetObjects = targetNode.GetCombatObjects();
        bool foundTarget = false;
        foreach (var targetObj in targetObjects){
            var healthComp = targetObj.GetCombatComponent<HealthComponent>();
            if (healthComp != null){
                foundTarget = true;
                break;
            }
        }
        if (!foundTarget){
            return false;
        }
        if (unit.Node.GetDistance(targetNode) > range){
            return false;
        }
        // if (!unit.Node.LineUnobstructed(targetNode)){
        //     return false;
        // }
        return true;
    }
}