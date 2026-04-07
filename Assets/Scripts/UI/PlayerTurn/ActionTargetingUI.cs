using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActionTargetingUI : UIElement{
    [BoxGroup("References")][Required][SerializeField] TextMeshProUGUI actionNameText;
    [BoxGroup("References")][Required][SerializeField] TextMeshProUGUI descriptionText;
    [BoxGroup("References")][Required][SerializeField] Button confirmButton;
    [BoxGroup("References")][Required][SerializeField] Button cancelButton;
    [BoxGroup("References")][Required][SerializeField] GridUI gridUI;

    UnitAction action;

    [FoldoutGroup("Events")]
    public UnityEvent<UnitAction> onConfirm;
    [FoldoutGroup("Events")]
    public UnityEvent onCancel;

    void Awake(){
        confirmButton.onClick.AddListener(OnConfirm);
        cancelButton.onClick.AddListener(OnCancel);
    }

    void OnCancel(){
        onCancel.Invoke();
    }

    public void OnConfirm(){
        if (!IsVisible){
            return;
        }
        onConfirm.Invoke(action);
    }
    
    public void Show(UnitAction action){
        base.Show();
        this.action = action;
        if (action is TargetedUnitAction targetedAction){
            confirmButton.interactable = false;
            gridUI.ShowMarks<CombatGridNode>(action.unit.Grid().Grid, targetedAction.GetValidTargets());
        }
        else{
            confirmButton.interactable = true;
        }
        actionNameText.SetText(action.Name);
        descriptionText.SetText(action.Description);
    }

    public void OnSelect(Vector2 pos){
        if (action is TargetedUnitAction targetedAction){
            bool isValid = targetedAction.SetTarget(pos);
            confirmButton.interactable = isValid;
        }
    }

    public override void Hide(){
        base.Hide();
        gridUI.HideMarks();
    }
}