using Nrjwolf.Tools.AttachAttributes;
using Sirenix.OdinInspector;
using UnityEngine;

class CombatObjectUI : UIElement{
    [BoxGroup("References")] [GetComponent] [SerializeField] RectTransform rectTransform;

    [BoxGroup("References")] [Required] [SerializeField] HealthComponentUI healthComponentUI;
    [BoxGroup("References")] [Required] [SerializeField] CombatUnitUI combatUnitUI;

    public void SetCombatObject(ICombatObject combatObject){
        var screenPos = Camera.main.WorldToScreenPoint(combatObject.WorldPosition());
        rectTransform.position = screenPos;
        var healthComponent = combatObject.GetCombatComponent<HealthComponent>();
        if (healthComponent != null){
            healthComponentUI.Show(healthComponent);
        }
        else{
            healthComponentUI.Hide();
        }
        if (combatObject is CombatUnit unit){
            combatUnitUI.Show(unit);
        }
        else{
            combatUnitUI.Hide();
        }
    }
}