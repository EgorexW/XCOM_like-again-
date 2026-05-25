using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuModifierBasePath + "Surrendered Status")]
public class PacifiedStatusFactory : UnitModifierFactory{
    [SerializeField] bool surrendered;
    
    public override UnitModifier Create(){
        return new PacifiedStatus(modifierInfo, surrendered);
    }
}

class PacifiedStatus : UnitModifier{
    readonly bool surrendered;

    public PacifiedStatus(ModifierInfo info, bool surrendered) : base(info){
        this.surrendered = surrendered;
    }

    public override void OnApplied(Unit targetTmp){
        base.OnApplied(targetTmp);
        targetTmp.RemoveFlag(CombatObjectFlags.MovementBlocker | CombatObjectFlags.LoSBlocker);
        targetTmp.AddFlag(CombatObjectFlags.Pacified);
        var suspectComponent = targetTmp.GetCombatComponent<SuspectComponent>();
        if (suspectComponent != null){
            suspectComponent.ChangeState(SuspectState.Pacified);
        }
    }

    public override bool CanExecuteAction(UnitAction action){
        return false;
    }
}