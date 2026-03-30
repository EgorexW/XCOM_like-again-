using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActionTargetingUI : UIElement{
    [BoxGroup("References")][Required][SerializeField] TextMeshProUGUI actionNameText;
    [BoxGroup("References")][Required][SerializeField] Button confirmButton;
    [BoxGroup("References")][Required][SerializeField] Button cancelButton;

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

    void OnConfirm(){
        onConfirm.Invoke(action);
    }
    
    public void Show(UnitAction action){
        base.Show();
        this.action = action;
        actionNameText.SetText(action.Name);
    }

    public void OnSelect(Vector2 pos){
        action.SetTarget(pos);
    }
}