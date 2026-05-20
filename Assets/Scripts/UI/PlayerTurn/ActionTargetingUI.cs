using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ActionTargetingUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] TextMeshProUGUI actionNameText;
    [BoxGroup("References")] [Required] [SerializeField] TextMeshProUGUI descriptionText;
    [BoxGroup("References")] [Required] [SerializeField] Button confirmButton;
    [BoxGroup("References")] [Required] [SerializeField] Button cancelButton;
    
    [BoxGroup("References")] [Required] [SerializeField] GridUI validTargetsUI;
    [BoxGroup("References")] [Required] [SerializeField] GridUI invalidTargetsUI;

    UnitAction action;

    [FoldoutGroup("Events")] public UnityEvent<UnitAction> onConfirm;
    [FoldoutGroup("Events")] public UnityEvent onCancel;

    protected void Awake(){
        confirmButton.onClick.AddListener(OnConfirm);
        cancelButton.onClick.AddListener(OnCancel);
    }

    public void OnCancel(){
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
        validTargetsUI.ClearMarks();
        if (action is TargetedUnitAction targetedAction){
            var allTargets = targetedAction.GetAllTargets();
            var validTargets = new List<CombatGridNode>();
            var nonValidTargetsToShow = new List<CombatGridNode>();
            foreach (var target in allTargets){
                var validation = targetedAction.ValidateTarget(target);
                if (validation == TargetValidation.Valid){
                    validTargets.Add(target);
                    continue;
                }
                if (validation == TargetValidation.NoValidTarget){
                    nonValidTargetsToShow.Add(target);
                }
            }
            validTargetsUI.MarkPositons(validTargets);
            invalidTargetsUI.MarkPositons(nonValidTargetsToShow);
        }

        confirmButton.interactable = action.ValidateAction() == UnitActionValidation.Valid;
        actionNameText.SetText(action.ActionInfo.Name);
        descriptionText.SetText(action.GetDescription());
    }

    public void OnSelect(Vector2 pos){
        if (action is not TargetedUnitAction targetedAction){
            return;
        }
        targetedAction.SetTarget(pos);
        confirmButton.interactable = action.ValidateAction() == UnitActionValidation.Valid;
    }

    public override void Hide(){
        base.Hide();
        validTargetsUI.ClearMarks();
        invalidTargetsUI.ClearMarks();
    }
}