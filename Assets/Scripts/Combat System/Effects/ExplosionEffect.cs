using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : CombatEffect{
    [SerializeField] float range = 1;
    [SerializeField] int damage = 1;
    [SerializeField] List<UnitStatusEffectCreator> statusEffects;
    
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
                if (obj is CombatUnit unit){
                    foreach (var statusEffect in statusEffects){
                        unit.ApplyStatus(statusEffect.CreateStatus());
                    }
                }
            }
        }
    }
}