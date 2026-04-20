using UnityEngine;

public class SupressStatusCreator : UnitStatusEffectCreator{
    [SerializeField] ActionFlags supressedFlags;
    [SerializeField] Optional<int> duration;

    public override UnitStatusEffect CreateStatus(){
        var durationValue = duration ? duration.Value : -1;
        var status = new SupressStatus(statusName, supressedFlags, durationValue);
        return status;
    }
}

public class SupressStatus : UnitStatusEffect{
    readonly ActionFlags supressedFlags;
    int duration;

    public SupressStatus(string name, ActionFlags flags, int durationTmp) : base(name){
        supressedFlags = flags;
        duration = durationTmp;
    }

    public override void OnApplied(CombatUnit targetTmp){
        base.OnApplied(targetTmp);
        if (duration > 0){
            target.onEndTurn.AddListener(OnEndTurn);
        }
    }

    void OnEndTurn(CombatUnit arg0){
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