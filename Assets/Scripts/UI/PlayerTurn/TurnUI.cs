using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TurnUI : UIElement{
    [BoxGroup("References")][Required][SerializeField] TurnSystem turnSystem;
    
    [FormerlySerializedAs("actionsUI")] [BoxGroup("Internal References")] [Required] [SerializeField] UnitActionsSelectionUI unitActionsSelectionUI;
    [BoxGroup("Internal References")] [Required] [SerializeField] ActionTargetingUI actionTargetingUI;
    [BoxGroup("Internal References")] [Required] [SerializeField] Button endTurnButton;
    [BoxGroup("Internal References")] [Required] [SerializeField] Transform selectedUnitHighlight;

    [ShowInInspector] CombatUnit selectedUnit;
    [ShowInInspector] PlayerTurnTaker currentTurnTaker;

    protected void Awake(){
        Hide();
        turnSystem.onStartTurn.AddListener(ShowTurnTaker);
        turnSystem.onEndTurn.AddListener(Hide);
        endTurnButton.onClick.AddListener(CompleteTurn);
        unitActionsSelectionUI.onActionSelected.AddListener(OnActionSelected);
        actionTargetingUI.onConfirm.AddListener(OnActionTargetConfirmed);
        actionTargetingUI.onCancel.AddListener(OnActionCanceled);
    }

    void Hide(ITurnTaker arg0){
        Hide();
    }

    void ShowTurnTaker(ITurnTaker arg0){
        if (arg0 is PlayerTurnTaker playerTurnTaker){
            currentTurnTaker = playerTurnTaker;
            Show();
        }
        else{
            Hide();
        }
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
        selectedUnitHighlight.gameObject.SetActive(false);
        unitActionsSelectionUI.Hide();
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
        selectedUnitHighlight.position = selectedUnit.transform.position;
        selectedUnitHighlight.localScale = selectedUnit.transform.lossyScale;
        selectedUnitHighlight.gameObject.SetActive(true);
        unitActionsSelectionUI.Show(unit);
    }

    public void DeselectUnit(){
        selectedUnit = null;
        selectedUnitHighlight.gameObject.SetActive(false);
        unitActionsSelectionUI.Hide();
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
        unitActionsSelectionUI.SelectSlot(slot);
    }

    public void OnCancel(){
        if (!IsVisible){
            return;
        }
        if (actionTargetingUI.IsVisible){
            actionTargetingUI.OnCancel();
        }
        else{
            DeselectUnit();
        }
    }
}