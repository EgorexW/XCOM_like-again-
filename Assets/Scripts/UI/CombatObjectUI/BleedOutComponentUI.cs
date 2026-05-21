using Sirenix.OdinInspector;
using UnityEngine;

class BleedOutComponentUI : CombatObjectUI{
    [BoxGroup("References")] [Required] [SerializeField] CountUI collapseablePool;

    public void Show(BleedOutComponent bleedOutComponent){
        base.Show();
        collapseablePool.SetCount(bleedOutComponent.TurnsLeft);
    }

    public override void SetCombatObject(ICombatObject combatObject){
        var bleedOutComponent = combatObject.GetCombatComponent<BleedOutComponent>();
        if (bleedOutComponent != null && bleedOutComponent.IsBleedingOut){
            Show(bleedOutComponent);
        }
        else{
            Hide();
        }
    }
}
