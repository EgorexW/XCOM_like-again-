using Nrjwolf.Tools.AttachAttributes;
using Sirenix.OdinInspector;
using UnityEngine;

class CombatObjectUI : UIElement{
    [BoxGroup("References")][GetComponent][SerializeField] RectTransform rectTransform;

    public void SetCombatObject(ICombatObject combatObject){
        var obj = combatObject.GameObject;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(obj.transform.position);
        rectTransform.position = screenPos;
    }
}