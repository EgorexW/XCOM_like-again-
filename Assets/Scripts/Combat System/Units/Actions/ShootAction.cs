using System.Collections.Generic;
using UnityEngine;

public class ShootAction : TargetedUnitAction{
    [SerializeField] float damage = 1;
    [SerializeField] List<UnitStatusEffectCreator> appliedStatusEffects;
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
        foreach (var targetObj in targetObjects.ReadOnly()){
            var healthComp = targetObj.GetCombatComponent<HealthComponent>();
            if (healthComp != null){
                healthComp.TakeDamage(damage);
            }
            if (targetObj is CombatUnit unit){
                foreach (var statusEffect in appliedStatusEffects){
                    unit.ApplyStatus(statusEffect.CreateStatus());
                }
            }
        }
    }

    protected override TargetValidation CheckActionSpecificTargetRules(CombatGridNode node){
        var result = base.CheckActionSpecificTargetRules(node);
        if (node.Contains(unit)){
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
        if (!unit.GetCenterNode().CanShoot(node)){
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

    public override int? GetUsesLeft(){
        var baseUsesLeft = base.GetUsesLeft();
        if (ammoCost <= 0){
            return baseUsesLeft;
        }
        var ammoComp = unit.GetCombatComponent<AmmoComponent>();
        if (ammoComp == null){
            Debug.LogWarning($"Action requires ammo but unit {unit.name} has no AmmoComponent!");
            return 0;
        }
        var ammoUsesLeft = ammoComp.CurrentLoadedAmmo / ammoCost;
        if (baseUsesLeft.HasValue){
            return Mathf.Min(baseUsesLeft.Value, ammoUsesLeft);
        }
        else{
            return ammoUsesLeft;
        }
    }
}