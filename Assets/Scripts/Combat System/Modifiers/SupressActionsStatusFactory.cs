using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuModifierBasePath + "Supress Status")]
public class SupressActionsStatusFactory : ModifierWithDurationFactory{
    [SerializeField] ActionFlags supressedFlags;

    public override UnitModifier Create(){
        var status = new SupressActionsStatus(statusName, supressedFlags, DurationValue);
        return status;
    }
}

public class SupressActionsStatus : ModifierWithDuration{
    readonly ActionFlags supressedFlags;

    public SupressActionsStatus(string name, ActionFlags flags, int durationTmp) : base(name, durationTmp){
        supressedFlags = flags;
    }

    public override bool CanExecuteAction(UnitAction action){
        if ((supressedFlags & action.ActionInfo.ActionFlags) != 0){
            return false;
        }
        return true;
    }
}