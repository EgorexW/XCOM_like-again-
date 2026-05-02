using Sirenix.OdinInspector;
using UnityEngine;

class CombatUnitUI : CombatObjectUI{
    [BoxGroup("References")] [Required] [SerializeField] CountUI collapseablePool;

    public void Show(Unit unit){
        base.Show();
        collapseablePool.SetCount(Mathf.RoundToInt(unit.ActionPoints));
    }

    public override void SetCombatObject(ICombatObject combatObject){
            if (combatObject is Unit unit){
                Show(unit);
            }
            else{
                Hide();
            }
    }
}