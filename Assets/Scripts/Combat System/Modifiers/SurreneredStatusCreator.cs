using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuModifierBasePath + "Surrendered Status")]
public class SurrenderedStatusFactory : UnitModifierFactory{
    public override UnitModifier Create(){
        return new SurrenderedStatus(statusName, this);
    }
}

class SurrenderedStatus : UnitModifier{
    readonly List<UnitAction> allowedActions;

    public SurrenderedStatus(string name, UnitModifierFactory sourceDefinition, List<UnitAction> allowedActions = null) : base(name, sourceDefinition){
        allowedActions ??= new List<UnitAction>();
        this.allowedActions = allowedActions;
    }

    public override bool CanExecuteAction(UnitAction action){
        return allowedActions.Contains(action);
    }

    public override void OnApplied(Unit targetTmp){
        base.OnApplied(targetTmp);
        targetTmp.RemoveFlag(CombatObjectFlags.MovementBlocker | CombatObjectFlags.LoSBlocker);
        var suspectComponent = targetTmp.GetCombatComponent<SuspectComponent>();
        if (suspectComponent != null){
            suspectComponent.ChangeState(SuspectState.Surrendered);
        }
        else{
            Debug.LogWarning("SuspectComponent is null (Surrendered Status)");
        }
    }
}