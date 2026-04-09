using UnityEngine;

public class ExplosionEffect : CombatEffect{
    [SerializeField] float range = 1;
    [SerializeField] int damage = 1;
    
    public override void Execute(){
        if (!HasNode){
            return;
        }
        foreach (var node in targetNode.GetNodesInRadius(range)){
            foreach (var obj in node.GetCombatObjects()){
                var health = obj.GetCombatComponent<HealthComponent>();
                if (health != null){
                    health.TakeDamage(damage);
                }
            }
            
        }
    }
}