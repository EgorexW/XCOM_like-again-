using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActionTargetingUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] TextMeshProUGUI actionNameText;
    [BoxGroup("References")] [Required] [SerializeField] TextMeshProUGUI descriptionText;
    [BoxGroup("References")] [Required] [SerializeField] Button confirmButton;
    [BoxGroup("References")] [Required] [SerializeField] Button cancelButton;
    [BoxGroup("References")] [Required] [SerializeField] GridUI gridUI;

    UnitAction action;

    [FoldoutGroup("Events")] public UnityEvent<UnitAction> onConfirm;
    [FoldoutGroup("Events")] public UnityEvent onCancel;

    protected void Awake(){
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
            gridUI.ShowMarks(action.unit.Grid().Grid, targetedAction.GetValidTargets());
        }

        Debug.Log($"Action Validation: {action.CanExecute()}");
        
        confirmButton.interactable = action.CanExecute() == UnitActionValidation.NoIssues;
        actionNameText.SetText(action.name);
        descriptionText.SetText(action.GetDescription());
    }

    public void OnSelect(Vector2 pos){
        if (action is not TargetedUnitAction targetedAction){
            return;
        }
        targetedAction.SetTarget(pos);
        confirmButton.interactable = action.CanExecute() == UnitActionValidation.NoIssues;
    }

    public override void Hide(){
        base.Hide();
        gridUI.HideMarks();
    }
}