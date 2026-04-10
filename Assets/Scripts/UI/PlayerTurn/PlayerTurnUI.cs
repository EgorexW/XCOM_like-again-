using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnUI : UIElement{
    [BoxGroup("External References")] [Required] [SerializeField] List<PlayerTurnTaker> playerTurnTakers;

    [BoxGroup("Internal References")] [Required] [SerializeField] ActionsUI actionsUI;
    [BoxGroup("Internal References")] [Required] [SerializeField] ActionTargetingUI actionTargetingUI;
    [BoxGroup("Internal References")] [Required] [SerializeField] Button endTurnButton;

    [ShowInInspector] CombatUnit selectedUnit;
    [ShowInInspector] PlayerTurnTaker currentTurnTaker;

    protected void Awake(){
        Hide();
        foreach (var playerTurnTaker in playerTurnTakers){
            playerTurnTaker.onStartTurn.AddListener(ShowTurnTaker);
            playerTurnTaker.onEndTurn.AddListener(Hide);
        }
        endTurnButton.onClick.AddListener(CompleteTurn);
        actionsUI.onActionSelected.AddListener(OnActionSelected);
        actionTargetingUI.onConfirm.AddListener(OnActionTargetConfirmed);
        actionTargetingUI.onCancel.AddListener(OnActionCanceled);
    }

    void Hide(ITurnTaker arg0){
        Hide();
    }

    void ShowTurnTaker(ITurnTaker arg0){
        currentTurnTaker = arg0 as PlayerTurnTaker;
        Show();
    }

    void CompleteTurn(){
        if (currentTurnTaker != null){
            currentTurnTaker.CompleteTurn();
        }
    }

    void OnActionCanceled(){
        actionTargetingUI.Hide();
    }

    public override void Show(){
        base.Show();
        actionsUI.Hide();
        actionTargetingUI.Hide();
    }

    void OnActionTargetConfirmed(UnitAction action){
        action.Execute();
        SelectUnit(selectedUnit);
    }

    void OnActionSelected(UnitAction action){
        actionTargetingUI.Show(action);
    }

    public void OnSelect(){
        if (!IsVisible){
            return;
        }
        if (actionTargetingUI.IsVisible){
            actionTargetingUI.OnSelect(General.GetMouseWorldPos());
        }
        else{
            foreach (var collider in GetCollidersUnderMouse()){
                // Debug.Log("Collider under mouse: " + collider.name, collider);
                var unit = General.GetComponentFromCollider<CombatUnit>(collider);
                if (unit == null){
                    continue;
                }
                // Debug.Log("CombatUnit under mouse: " + unit.name, unit);
                SelectUnit(unit);
            }
        }
    }

    void SelectUnit(CombatUnit unit){
        if (!currentTurnTaker.Units.Contains(unit)){
            Debug.LogWarning($"Cannot select unit {unit.name}, it is not controlled by the player.");
            return;
        }
        DeselectUnit();
        selectedUnit = unit;
        actionsUI.Show(unit);
    }

    public void DeselectUnit(){
        selectedUnit = null;
        actionsUI.Hide();
        actionTargetingUI.Hide();
    }

    List<Collider2D> GetCollidersUnderMouse(){
        var point = General.GetMouseWorldPos();
        var collider2Ds = new List<Collider2D>();
        Physics2D.OverlapPoint(point, ContactFilter2D.noFilter, collider2Ds);
        // Debug.Log("Colliders under mouse: " + collider2Ds.Count);
        return collider2Ds;
    }

    public void OnConfirm(){
        if (!IsVisible){
            return;
        }
        actionTargetingUI.OnConfirm();
    }

    public void OnSlotSelected(int slot){
        if (!IsVisible){
            return;
        }
        actionsUI.SelectSlot(slot);
    }
}