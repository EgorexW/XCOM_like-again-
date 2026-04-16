using Nrjwolf.Tools.AttachAttributes;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

class CombatObjectUI : UIElement{
    [BoxGroup("References")] [GetComponent] [SerializeField] RectTransform rectTransform;

    [BoxGroup("References")] [Required] [SerializeField] HealthComponentUI healthComponentUI;
    [BoxGroup("References")] [Required] [SerializeField] CombatUnitUI combatUnitUI;
    [BoxGroup("References")][Required][SerializeField] SuspectComponentUI suspectComponentUI;

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
        var suspectComponent = combatObject.GetCombatComponent<SuspectComponent>();
        if (suspectComponent != null){
            suspectComponentUI.Show(suspectComponent);
        }
        else{
            suspectComponentUI.Hide();
        }
    }
}