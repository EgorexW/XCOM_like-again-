using System.Collections.Generic;
using Nrjwolf.Tools.AttachAttributes;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Base class for all sub-UIs that belong to a single combat object card.
/// </summary>
public abstract class CombatObjectUI : UIElement{
    public abstract void SetCombatObject(ICombatObject combatObject);
}

/// <summary>
/// Legacy container kept for backward-compat with old prefabs.
/// New setups should use CombatObjectCard instead.
/// The TooltipTrigger dependency has been removed — use CombatObjectCard
/// events (onPointerEnter / onPointerExit) for hover behaviour.
/// </summary>
class MainCombatObjectUI : CombatObjectUI{
    [BoxGroup("References")] [GetComponent] [SerializeField] RectTransform rectTransform;

    List<CombatObjectUI> uiChildren;

    protected void Awake(){
        uiChildren = new List<CombatObjectUI>(GetComponentsInChildren<CombatObjectUI>(true));
        uiChildren.Remove(this);
    }

    public override void SetCombatObject(ICombatObject combatObject){
        rectTransform.position = combatObject.GetCenter();
        foreach (var ui in uiChildren) ui.SetCombatObject(combatObject);
    }
}