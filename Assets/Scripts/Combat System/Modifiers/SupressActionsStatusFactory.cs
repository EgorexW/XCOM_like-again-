using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuModifierBasePath + "Supress Status")]
public class SupressActionsStatusFactory : ModifierWithDurationFactory{
    [SerializeField] ActionFlags supressedFlags;

    public override UnitModifier Create(){
        var status = new SupressActionsStatus(modifierInfo, supressedFlags, DurationValue);
        return status;
    }
}

public class SupressActionsStatus : ModifierWithDuration{
    readonly ActionFlags supressedFlags;

    public SupressActionsStatus(ModifierInfo info, ActionFlags flags, int durationTmp) : base(info, durationTmp){
        supressedFlags = flags;
    }

    public override bool CanExecuteAction(UnitAction action){
        if ((supressedFlags & action.ActionInfo.ActionFlags) != 0){
            return false;
        }
        return true;
    }
}