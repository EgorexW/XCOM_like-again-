using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ShootAction : TargetedUnitAction{
    [FormerlySerializedAs("damage")] [SerializeField] int damageValue = 1;
    [SerializeField] List<UnitModifierFactory> appliedStatusEffects;
    [SerializeField] int ammoCost = 1;
    [SerializeField] bool ignoreCover;

    public float DamageValue => damageValue;

    protected override void OnExecute(){
        if (ammoCost > 0){
            var ammoComp = unit.GetCombatComponent<AmmoComponent>();
            if (ammoComp != null){
                ammoComp.ConsumeAmmo(ammoCost);
            }
        }

        var damage = new Damage(){
            value = this.damageValue,
            source = unit
        };
        var targetObjects = targetNode.GetCombatObjects();
        foreach (var targetObj in (IReadOnlyList<ICombatObject>)targetObjects.AsReadOnly()){
            var healthComp = targetObj.GetCombatComponent<HealthComponent>();
            if (healthComp != null){
                healthComp.TakeDamage(damage);
            }
            if (targetObj is Unit unit){
                foreach (var statusEffect in appliedStatusEffects) unit.ApplyModifier(statusEffect.Create());
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
        if (!unit.GetCenterNode().CanShoot(node, ignoreCover: ignoreCover)){
            result |= TargetValidation.NoPath;
        }
        return result;
    }

    public override UnitActionValidation ValidateAction(){
        var result = base.ValidateAction();
        if (ammoCost > 0){
            var ammoComp = unit.GetCombatComponent<AmmoComponent>();
            if (ammoComp == null){
                Debug.LogWarning($"Action requires ammo but unit {unit.name} has no AmmoComponent!");
                result |= UnitActionValidation.AmmoIssue;
            }
            else if (ammoComp.CurrentLoadedAmmo < ammoCost){
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
        return ammoUsesLeft;
    }
}