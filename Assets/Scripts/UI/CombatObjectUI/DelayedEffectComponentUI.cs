using Sirenix.OdinInspector;
using UnityEngine;

public class DelayedEffectComponentUI : CombatObjectUI{
    [BoxGroup("References")] [Required] [SerializeField] CountUI collapseablePool;

    public void Show(DelayedEffectComponent component){
        base.Show();
        var value = Mathf.RoundToInt(component.DurationLeft);
        collapseablePool.SetCount(value);
    }

    public override void SetCombatObject(ICombatObject combatObject){
        var component = combatObject.GetCombatComponent<DelayedEffectComponent>();
        if (component != null){
            Show(component);
        }
        else{
            Hide();
        }
    }
}