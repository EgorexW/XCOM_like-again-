using UnityEngine;

public class ReloadAction : UnitAction {
    
    protected override void OnExecute() {
        var ammoComp = unit.GetCombatComponent<AmmoComponent>();
        ammoComp.Reload();
    }
    
    public override UnitActionValidation ValidateAction() {
        var result = base.ValidateAction();
        
        var ammoComp = unit.GetCombatComponent<AmmoComponent>();
        if (ammoComp == null) {
            Debug.LogWarning($"Can't reload, unit {unit.name} has no AmmoComponent!");
            result |= UnitActionValidation.AmmoIssue;
            return result;
        }
        if (ammoComp.IsFull) {
            result |= UnitActionValidation.AmmoIssue;
        }

        return result;
    }
    
    public override int? GetUsesLeft(){
        var baseUsesLeft = base.GetUsesLeft();
        var ammoComp = unit.GetCombatComponent<AmmoComponent>();
        if (ammoComp == null){
            Debug.LogWarning($"Can't reload, unit {unit.name} has no AmmoComponent!");
            return baseUsesLeft;
        }
        var ammoUsesLeft = ammoComp.Magazines;
        if (baseUsesLeft.HasValue){
            return Mathf.Min(baseUsesLeft.Value, ammoUsesLeft);
        }
        return ammoUsesLeft;
    }
}