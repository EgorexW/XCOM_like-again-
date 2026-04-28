using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class CombatUnit : CombatObject{
    [SerializeField] int defaultActionPoints = 2;

    public IReadOnlyList<UnitAction> UnitActions => unitActions.AsReadOnly();
    public int ActionPoints => actionPoints;

    [SerializeField] [HideInEditorMode] List<UnitAction> unitActions;
    [SerializeField] [HideInEditorMode] int actionPoints;

    readonly List<UnitModifier> activeStatuses = new();

    [FoldoutGroup("Events")] public UnityEvent<CombatUnit> onStartTurn;
    [FoldoutGroup("Events")] public UnityEvent<CombatUnit> onEndTurn;
    [FoldoutGroup("Events")] public UnityEvent<UnitAction> onActionPerformed;

    public override void Init(){
        base.Init();
        var actionsTmp = GetComponentsInChildren<UnitAction>().ToList();
        foreach (var action in actionsTmp) AddAction(action);
        onActionPerformed.AddListener(_ => CombatSystem.StateChanged());
    }

    public void OnStartTurn(){
        actionPoints = defaultActionPoints;
        onStartTurn.Invoke(this);
    }

    public void OnEndTurn(){
        actionPoints = 0;
        onEndTurn.Invoke(this);
    }

    public void ActionExecuted(UnitAction action){
        var cost = action.GetCost();
        if (cost > actionPoints){
            Debug.LogWarning(
                $"Unit {name} does not have enough action points to spend {cost}. Current AP: {actionPoints}");
            actionPoints = 0;
            return;
        }
        actionPoints -= cost;
        onActionPerformed.Invoke(action);
    }

    public void ApplyStatus(UnitModifier status){
        activeStatuses.Add(status);
        status.OnApplied(this);
        // Debug.Log($"Applied status {status.name} to unit {name}");
    }

    public void RemoveStatus(UnitModifier status){
        if (!activeStatuses.Contains(status)){
            return;
        }
        status.OnRemoved();
        activeStatuses.Remove(status);
        // Debug.Log($"Removed status {status.name} from unit {name}");
    }

    public UnitActionValidation CanExecute(UnitAction action){
        var result = UnitActionValidation.Valid;
        if (ActionPoints < action.GetCost()){
            result |= UnitActionValidation.NotEnoughActionPoints;
        }
        foreach (var status in activeStatuses)
            if (!status.CanExecuteAction(action)){
                result |= UnitActionValidation.SupressedByStatus;
            }
        return result;
    }

    public UnitAction InstantiateAction(GameObject action){
        var actionInstance = Instantiate(action, transform);
        var unitAction = actionInstance.GetComponent<UnitAction>();
        if (unitAction == null){
            Debug.LogError($"The provided action prefab {action.name} does not have a UnitAction component.");
            Destroy(actionInstance);
            return null;
        }
        AddAction(unitAction);
        return unitAction;
    }

    void AddAction(UnitAction unitAction){
        unitAction.unit = this;
        unitActions.Add(unitAction);
    }

    public void RemoveAction(UnitAction action){
        if (!unitActions.Contains(action)){
            Debug.LogWarning($"Unit {name} does not have action {action.ActionInfo.Name} to remove.");
            return;
        }
        unitActions.Remove(action);
        Destroy(action.gameObject);
    }
}