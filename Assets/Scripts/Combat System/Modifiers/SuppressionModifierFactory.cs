using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuModifierBasePath + "Suppression Modifier")]
public class SuppressionModifierFactory : ChangeActionPointsModifierFactory{
    public override UnitModifier Create(){
        // Force instant to false since suppression should apply on start turn only
        return new SuppressionModifier(modifierInfo, DurationValue, actionPointsChange, false);
    }
}

public class SuppressionModifier : ChangeActionPointsModifier{
    public SuppressionModifier(ModifierInfo info, int durationTmp, int actionPointsChange, bool instant)
        : base(info, durationTmp, actionPointsChange, instant){
    }

    protected override void OnStartTurn(Unit arg0){
        // Find all suppression modifiers currently on the unit
        var suppressions = target.GetModifiersOfType(GetType())
            .Cast<SuppressionModifier>()
            .ToList();

        if (suppressions.Count == 0) return;

        // Assuming action points change is negative for suppression (e.g., -1, -2).
        // A "stronger" suppression has a lower value.
        var strongest = suppressions.OrderBy(s => s.ActionPointsChange).First();

        // Only apply the action points change if THIS is the strongest suppression.
        // If there are multiple with the same strength, OrderBy preserves order so only the first one will apply it.
        if (this == strongest){
            target.ChangeActionPoints(actionPointsChange);
        }
    }
}
