using UnityEngine;

public class ShootAction : TargetedUnitAction{
    [SerializeField] float damage = 1;
    [SerializeField] int ammoCost = 1;
    public float Damage => damage;

    protected override void OnExecute(){
        
        if (ammoCost > 0) {
            var ammoComp = unit.GetCombatComponent<AmmoComponent>();
            if (ammoComp != null) {
                ammoComp.ConsumeAmmo(ammoCost);
            }
        }
        
        var targetObjects = targetNode.GetCombatObjects();
        foreach (var targetObj in targetObjects){
            var healthComp = targetObj.GetCombatComponent<HealthComponent>();
            if (healthComp != null){
                healthComp.TakeDamage(damage);
            }
        }
    }

    protected override TargetValidation CheckActionSpecificTargetRules(CombatGridNode node){
        var result = base.CheckActionSpecificTargetRules(node);
        if (node == unit.Node){
            result |= TargetValidation.InvalidTarget;
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
            result |= TargetValidation.NoValidTarget;
        }
        if (!unit.Node.CanAttack(node)){
            result |= TargetValidation.NoPath;
        }
        return result;
    }

    public override UnitActionValidation ValidateAction(){
        var result = base.ValidateAction();
         if (ammoCost > 0) {
             var ammoComp = unit.GetCombatComponent<AmmoComponent>();
             if (ammoComp == null) {
                 Debug.LogWarning($"Action requires ammo but unit {unit.name} has no AmmoComponent!");
                 result |= UnitActionValidation.AmmoIssue;
             } else if (ammoComp.CurrentLoadedAmmo < ammoCost) {
                 result |= UnitActionValidation.AmmoIssue;
             }
         }
         return result;
    }

    public override int GetUsesLeft(){
        var baseUsesLeft = base.GetUsesLeft();
        var ammoComp = unit.GetCombatComponent<AmmoComponent>();
        if (ammoComp == null){
            Debug.LogWarning($"Action requires ammo but unit {unit.name} has no AmmoComponent!");
            return 0;
        }
        var ammoUsesLeft = ammoComp.CurrentLoadedAmmo / ammoCost;
        if (baseUsesLeft > -1){
            return Mathf.Min(baseUsesLeft, ammoUsesLeft);
        }
        else{
            return ammoUsesLeft;
        }
    }
}