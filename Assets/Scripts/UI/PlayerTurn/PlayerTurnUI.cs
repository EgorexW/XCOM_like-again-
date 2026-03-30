using System;
using System.Collections.Generic;
using NUnit.Framework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerTurnUI : UIElement{
    [BoxGroup("External References")][Required][SerializeField] PlayerTurnTaker playerTurnTaker;
    
    [BoxGroup("Internal References")][Required][SerializeField] ActionsUI actionsUI;
    [BoxGroup("Internal References")][Required][SerializeField] ActionTargetingUI  actionTargetingUI;
    [BoxGroup("Internal References")][Required][SerializeField] Button endTurnButton;
    
    [ShowInInspector] CombatUnit selectedUnit;

    void Awake(){
        Hide();
        playerTurnTaker.onStartTurn.AddListener(Show);
        playerTurnTaker.onEndTurn.AddListener(Hide);
        endTurnButton.onClick.AddListener(playerTurnTaker.CompleteTurn);
        actionsUI.onActionSelected.AddListener(OnActionSelected);
        actionTargetingUI.onConfirm.AddListener(OnActionTargetConfirmed);
        actionTargetingUI.onCancel.AddListener(OnActionCanceled);
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
        actionTargetingUI.Hide();
    }

    void OnActionSelected(UnitAction action){
        actionTargetingUI.Show(action);
    }

    public void OnSelect(){
        if (!IsVisible){
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject()){
            return;
        }
        if (actionTargetingUI.IsVisible){
            actionTargetingUI.OnSelect(General.GetMouseWorldPos());
        }
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

    void SelectUnit(CombatUnit unit){
        if (!playerTurnTaker.Units.Contains(unit)){
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
        List<Collider2D> collider2Ds = new List<Collider2D>();
        Physics2D.OverlapPoint(point, ContactFilter2D.noFilter, collider2Ds);
        // Debug.Log("Colliders under mouse: " + collider2Ds.Count);
        return collider2Ds;
    }
}