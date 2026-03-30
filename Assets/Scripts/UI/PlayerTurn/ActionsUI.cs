using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

class ActionsUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] ObjectsPool actionsPool;

    [FoldoutGroup("Events")]
    public UnityEvent<UnitAction> onActionSelected;
    
    public void Show(CombatUnit unit){
        base.Show();
        var actions = unit.UnitActions;
        actionsPool.SetCount(actions.Count);
        for (int i = 0; i < actions.Count; i++){
            var actionUI = actionsPool.GetActiveObject(i).GetComponent<ActionTileUI>();
            actionUI.SetAction(actions[i], OnActionSelected);
        }
    }

    void OnActionSelected(UnitAction action){
        Debug.Log("Selected action: " + action.Name, action);
        onActionSelected.Invoke(action);
    }
}