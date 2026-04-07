using Sirenix.OdinInspector;
using UnityEngine;

class CombatUnitUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] CountUI collapseablePool;

    public void Show(CombatUnit unit){
        base.Show();
        collapseablePool.SetCount(Mathf.RoundToInt(unit.ActionPoints));
    }
}