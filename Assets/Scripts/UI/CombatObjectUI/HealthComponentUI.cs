using Sirenix.OdinInspector;
using UnityEngine;

class HealthComponentUI : CombatObjectUI{
    [BoxGroup("References")] [Required] [SerializeField] CountUI collapseablePool;

    public void Show(HealthComponent healthComponent){
        base.Show();
        var health = Mathf.RoundToInt(healthComponent.Health);
        collapseablePool.SetCount(health);
    }

    public override void SetCombatObject(ICombatObject combatObject){
        var healthComponent = combatObject.GetCombatComponent<HealthComponent>();
        if (healthComponent != null){
            Show(healthComponent);
        }
        else{
            Hide();
        }
    }
}