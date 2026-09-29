using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuModifierBasePath + "Suppression Modifier")]
public class SuppressionModifierFactory : ChangeActionPointsModifierFactory{
    public override UnitModifier Create(){
        return new SuppressionModifier(modifierInfo, DurationValue, actionPointsChange, false);
    }
}

public class SuppressionModifier : ChangeActionPointsModifier{
    public SuppressionModifier(ModifierInfo info, int durationTmp, int actionPointsChange, bool instant)
        : base(info, durationTmp, actionPointsChange, instant){
    }

    protected override void OnStartTurn(Unit arg0){
        var suppressions = target.GetModifiersOfType(GetType())
            .Cast<SuppressionModifier>()
            .ToList();

        if (suppressions.Count == 0) return;
        
        var strongest = suppressions.OrderBy(s => s.ActionPointsChange).First();
        
        if (this == strongest){
            target.ChangeActionPoints(actionPointsChange);
        }
    }
}
