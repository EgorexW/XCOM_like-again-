using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuModifierBasePath + "Surrendered Status")]
public class SurrenderedStatusFactory : UnitModifierFactory{
    public override UnitModifier CreateStatus(){
        return new SurrenderedStatus(statusName);
    }
}

class SurrenderedStatus : UnitModifier{
    readonly List<UnitAction> allowedActions;

    public SurrenderedStatus(string name, List<UnitAction> allowedActions = null) : base(name){
        allowedActions ??= new List<UnitAction>();
        this.allowedActions = allowedActions;
    }

    public override bool CanExecuteAction(UnitAction action){
        return allowedActions.Contains(action);
    }

    public override void OnApplied(CombatUnit targetTmp){
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