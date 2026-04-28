using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuModifierBasePath + "Add Action Modifier")]
public class AddActionUnitModifierFactory : UnitModifierFactory {
    [SerializeField] GameObject relatedAction; 
    [SerializeField] bool consumable = true;

    public override UnitModifier CreateStatus() {
        return new AddActionUnitModifier(statusName, relatedAction, consumable);
    }
}

class AddActionUnitModifier : UnitModifier {
    GameObject actionPrefab;
    bool consumable;
    
    UnitAction instantiatedAction; 

    public AddActionUnitModifier(string name, GameObject actionPrefab, bool consumable) : base(name) {
        this.actionPrefab = actionPrefab;
        this.consumable = consumable;
    }

    public override void OnApplied(CombatUnit targetTmp) {
        base.OnApplied(targetTmp);
        instantiatedAction = target.InstantiateAction(actionPrefab);
        target.onActionPerformed.AddListener(OnActionPerformed);
    }

    void OnActionPerformed(UnitAction performedAction) {
        if (!consumable) {
            return;
        }
        var usesLeft = instantiatedAction.GetUsesLeft();
        if (!usesLeft.HasValue){
            Debug.LogWarning("Action does not have limited uses, but is consumable");
            return;
        }
        if (usesLeft <= 0) {
            target.RemoveStatus(this);
        }
    }
    
    public override void OnRemoved() {
        base.OnRemoved();
        target.RemoveAction(instantiatedAction);
        target.onActionPerformed.RemoveListener(OnActionPerformed);
    }
}