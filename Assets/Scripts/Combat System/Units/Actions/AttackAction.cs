using UnityEngine;

public class AttackAction : TargetedUnitAction{
    [SerializeField] int range = 10;
    [SerializeField] float damage = 1;

    protected override void OnExecute(){
        var targetObjects = targetNode.GetCombatObjects();
        foreach (var targetObj in targetObjects){
            var healthComp = targetObj.GetCombatComponent<HealthComponent>();
            if (healthComp != null){
                healthComp.TakeDamage(damage);
            }
        }
    }

    protected override bool IsValidTarget(CombatGridNode node){
        if (!base.IsValidTarget(node)){
            return false;
        }
        if (node == unit.Node){
            return false;
        }
        var targetObjects = node.GetCombatObjects();
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
        if (unit.Node.GetDistance(node) > range){
            return false;
        }
        if (!unit.Node.CanAttack(node)){
            return false;
        }
        return true;
    }
}