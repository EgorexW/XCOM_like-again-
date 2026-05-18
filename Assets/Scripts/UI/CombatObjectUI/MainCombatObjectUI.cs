using System.Collections.Generic;
using Nrjwolf.Tools.AttachAttributes;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class CombatObjectUI : UIElement{
    public abstract void SetCombatObject(ICombatObject combatObject);
}

class MainCombatObjectUI : CombatObjectUI{
    [BoxGroup("References")] [GetComponent] [SerializeField] RectTransform rectTransform;
    [SerializeField] TooltipTrigger tooltipTrigger;

    List<CombatObjectUI> uiChildren;

    protected void Awake(){
        uiChildren = new List<CombatObjectUI>(GetComponentsInChildren<CombatObjectUI>(true));
        uiChildren.Remove(this);
    }

    public override void SetCombatObject(ICombatObject combatObject){
        rectTransform.position = combatObject.GetCenter();
        foreach (var ui in uiChildren) ui.SetCombatObject(combatObject);
        tooltipTrigger?.SetMessage(combatObject.GetMessage());
    }
}