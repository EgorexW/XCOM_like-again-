using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuModifierBasePath + "Add Action Modifier")]
public class AddActionUnitModifierFactory : UnitModifierFactory {
    [SerializeField] GameObject relatedAction; 

    public override UnitModifier Create() {
        return new AddActionUnitModifier(statusName, this, relatedAction);
    }
}

class AddActionUnitModifier : UnitModifier {
    GameObject actionPrefab;
    
    UnitAction instantiatedAction; 

    public AddActionUnitModifier(string name, UnitModifierFactory sourceDefinition, GameObject actionPrefab) : base(name) {
        this.actionPrefab = actionPrefab;
    }

    public override void OnApplied(Unit targetTmp) {
        base.OnApplied(targetTmp);
        instantiatedAction = target.InstantiateAction(actionPrefab);
        target.onActionPerformed.AddListener(OnActionPerformed);
    }

    void OnActionPerformed(UnitAction performedAction) {
        var usesLeft = instantiatedAction.GetUsesLeft();
        if (usesLeft <= 0) {
            target.RemoveModifier(this);
        }
    }
    
    public override void OnRemoved() {
        base.OnRemoved();
        target.RemoveAction(instantiatedAction);
        target.onActionPerformed.RemoveListener(OnActionPerformed);
    }
}