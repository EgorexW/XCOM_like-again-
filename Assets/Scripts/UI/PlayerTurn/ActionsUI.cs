using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

class ActionsUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] ObjectsPool actionsPool;

    [FoldoutGroup("Events")] public UnityEvent<UnitAction> onActionSelected;

    CombatUnit unit;

    public void Show(CombatUnit newUnit){
        base.Show();
        unit = newUnit;
        var actions = unit.UnitActions;
        actions.RemoveAll(a => a.NoUsesLeft);
        actionsPool.SetCount(actions.Count);
        for (var i = 0; i < actions.Count; i++){
            var actionUI = actionsPool.GetActiveObject(i).GetComponent<ActionTileUI>();
            actionUI.SetAction(actions[i], OnActionSelected);
        }
    }

    void OnActionSelected(UnitAction action){
        // Debug.Log("Selected action: " + action.name, action);
        onActionSelected.Invoke(action);
    }

    public void SelectSlot(int slot){
        if (unit == null){
            return;
        }
        var actions = unit.UnitActions;
        if (slot < 0 || slot >= actions.Count){
            Debug.LogWarning($"Invalid action slot {slot}. Unit {unit.name} has {actions.Count} actions.");
            return;
        }
        var action = actions[slot];
        OnActionSelected(action);
    }
}