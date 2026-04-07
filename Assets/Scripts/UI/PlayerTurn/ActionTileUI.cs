using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

class ActionTileUI : UIElement{
    [BoxGroup("References")][Required][SerializeField] TextMeshProUGUI actionName;
    [BoxGroup("References")][Required][SerializeField] Button selectButton;
    
    UnitAction action;
    UnityAction<UnitAction> onSelect;

    protected void Awake(){
        selectButton.onClick.AddListener(OnSelect);
    }

    void OnSelect(){
        onSelect?.Invoke(action);
    }

    public void SetAction(UnitAction action, UnityAction<UnitAction> onSelect){
        actionName.SetText(action.Name);
        this.action = action;
        this.onSelect = onSelect;
    }
}