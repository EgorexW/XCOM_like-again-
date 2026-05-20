using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuModifierBasePath + "Change Action Points")]
public class ChangeActionPointsModifierFactory : ModifierWithDurationFactory{
    [SerializeField] protected bool instant = true;
    [SerializeField] protected int actionPointsChange;

    public override UnitModifier Create(){
        var status = new ChangeActionPointsModifier(modifierInfo, DurationValue, actionPointsChange, instant);
        return status;
    }
}

public class ChangeActionPointsModifier : ModifierWithDuration{
    public int ActionPointsChange => actionPointsChange;

    protected readonly int actionPointsChange;
    protected readonly bool instant;

    public ChangeActionPointsModifier(ModifierInfo info, int durationTmp, int actionPointsChange, bool instant)
        : base(info, durationTmp){
        this.actionPointsChange = actionPointsChange;
        this.instant = instant;
    }

    public override void OnApplied(Unit targetTmp){
        base.OnApplied(targetTmp);
        targetTmp.onStartTurn.AddListener(OnStartTurn);
        
        if (instant){
            target.ChangeActionPoints(actionPointsChange);
        }
    }

    protected virtual void OnStartTurn(Unit arg0){
        target.ChangeActionPoints(actionPointsChange);
    }

    public override void OnRemoved(){
        base.OnRemoved();
        target.onStartTurn.RemoveListener(OnStartTurn);
    }
}