using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuModifierBasePath + "Supress Status")]
public class SupressStatusFactory : UnitModifierFactory{
    [SerializeField] ActionFlags supressedFlags;
    [SerializeField] Optional<int> duration;

    public override UnitModifier Create(){
        var durationValue = duration ? duration.Value : -1;
        var status = new SupressStatus(statusName, this, supressedFlags, durationValue);
        return status;
    }
}

public class SupressStatus : UnitModifier{
    readonly ActionFlags supressedFlags;
    int duration;

    public SupressStatus(string name, UnitModifierFactory sourceDefinition, ActionFlags flags, int durationTmp) : base(name, sourceDefinition){
        supressedFlags = flags;
        duration = durationTmp;
    }

    public override void OnApplied(Unit targetTmp){
        base.OnApplied(targetTmp);
        if (duration > 0){
            target.onEndTurn.AddListener(OnEndTurn);
        }
    }

    void OnEndTurn(Unit arg0){
        duration -= 1;
        if (duration <= 0){
            target.RemoveStatus(this);
        }
    }

    public override void OnRemoved(){
        base.OnRemoved();
        target.onEndTurn.RemoveListener(OnEndTurn);
    }

    public override bool CanExecuteAction(UnitAction action){
        if ((supressedFlags & action.ActionInfo.ActionFlags) != 0){
            return false;
        }
        return true;
    }
}