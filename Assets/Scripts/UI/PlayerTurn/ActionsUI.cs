using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

class ActionsUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] ObjectsPool actionsPool;

    [FoldoutGroup("Events")] public UnityEvent<UnitAction> onActionSelected;

    CombatUnit unit;
    List<ActionTileUI> actionsUI;

    public void Show(CombatUnit newUnit){
        base.Show();
        unit = newUnit;
        var actions = unit.UnitActions;
        foreach (var action in actions.Copy()){
            var validation = action.ValidateAction();
            if (validation.HasFlag(UnitActionValidation.NoUsesLeft)){
                actions.Remove(action);
            }
            if (validation.HasFlag(UnitActionValidation.AmmoIssue)){
                actions.Remove(action);
            }
        }
        actionsPool.SetCount(actions.Count);
        actionsUI = new List<ActionTileUI>();
        for (var i = 0; i < actions.Count; i++){
            var actionUI = actionsPool.GetActiveObject(i).GetComponent<ActionTileUI>();
            actionsUI.Add(actionUI);
            actionUI.SetAction(actions[i], OnActionSelected);
        }
    }

    void OnActionSelected(UnitAction action){
        onActionSelected.Invoke(action);
    }

    public void SelectSlot(int slot){
        if (unit == null){
            return;
        }
        if (slot < 0 || slot >= actionsUI.Count){
            Debug.LogWarning($"Invalid action slot {slot}. Unit {unit.name} has {actionsUI.Count} actions.");
            return;
        }
        var actionUI = actionsUI[slot];
        actionUI.OnSelect();
    }
}