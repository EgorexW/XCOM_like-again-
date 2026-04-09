using UnityEngine;

public class AttackAction : TargetedUnitAction{
    [SerializeField] float damage = 1;
    public float Damage => damage;

    protected override void OnExecute(){
        var targetObjects = targetNode.GetCombatObjects();
        foreach (var targetObj in targetObjects){
            var healthComp = targetObj.GetCombatComponent<HealthComponent>();
            if (healthComp != null){
                healthComp.TakeDamage(damage);
            }
        }
    }

    protected override bool CheckActionSpecificRules(CombatGridNode node){
        if (node == unit.Node){
            return false;
        }
        var targetObjects = node.GetCombatObjects();
        var foundTarget = false;
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
        if (!unit.Node.CanAttack(node)){
            return false;
        }
        return true;
    }
}