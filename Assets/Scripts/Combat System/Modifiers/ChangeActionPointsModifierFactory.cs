using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuModifierBasePath + "Change Action Points")]
public class ChangeActionPointsModifierFactory : ModifierWithDurationFactory{
    [SerializeField] bool instant = true;
    [SerializeField] int actionPointsChange;
    [SerializeField] int maxStack = 1;

    public override UnitModifier Create(){
        var status = new ChangeActionPointsModifier(statusName, DurationValue, actionPointsChange, instant, maxStack);
        return status;
    }
}

public class ChangeActionPointsModifier : ModifierWithDuration{
    readonly int actionPointsChange;
    readonly bool instant;
    readonly int maxStack;

    public ChangeActionPointsModifier(string name, int durationTmp, int actionPointsChange, bool instant, int maxStack)
        : base(name, durationTmp){
        this.actionPointsChange = actionPointsChange;
        this.instant = instant;
        this.maxStack = maxStack;
    }

    public override void OnApplied(Unit targetTmp){
        base.OnApplied(targetTmp);
        targetTmp.onStartTurn.AddListener(OnStartTurn);
        var currentStack = targetTmp.GetModifiersOfType(typeof(ChangeActionPointsModifier)).Count;
        if (currentStack > maxStack){
            targetTmp.RemoveModifier(this);
            return;
        }
        if (instant){
            target.ChangeActionPoints(actionPointsChange);
        }
    }

    void OnStartTurn(Unit arg0){
        target.ChangeActionPoints(actionPointsChange);
    }

    public override void OnRemoved(){
        base.OnRemoved();
        target.onStartTurn.RemoveListener(OnStartTurn);
    }
}