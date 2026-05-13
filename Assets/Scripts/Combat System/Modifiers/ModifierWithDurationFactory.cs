using UnityEngine;

public abstract class ModifierWithDurationFactory : UnitModifierFactory{
    [SerializeField] Optional<int> duration;

    protected int DurationValue => duration ? duration.Value : -1;
}

public class ModifierWithDuration : UnitModifier{
    protected int duration;

    protected ModifierWithDuration(string name, int durationTmp) : base(name){
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
            target.RemoveModifier(this);
        }
    }

    public override void OnRemoved(){
        base.OnRemoved();
        target.onEndTurn.RemoveListener(OnEndTurn);
    }
}