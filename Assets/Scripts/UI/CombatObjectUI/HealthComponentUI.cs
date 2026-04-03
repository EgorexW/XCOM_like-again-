using Sirenix.OdinInspector;
using UnityEngine;

class HealthComponentUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] CountUI collapseablePool;
    
    public void Show(HealthComponent healthComponent){
        base.Show();
        int health = Mathf.RoundToInt(healthComponent.Health);
        collapseablePool.SetCount(health);
    }
}