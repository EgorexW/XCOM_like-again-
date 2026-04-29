using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ExplosionEffect : CombatEffect{
    [SerializeField] float range = 1;
    [SerializeField] int damage = 1;
    [SerializeField] List<UnitModifierFactory> statusEffects;
    [SerializeField][BoxGroup("Spawn Settings")] GameObject prefabToSpawn;
    [SerializeField][BoxGroup("Spawn Settings")][ShowIf("HasPrefab")] InsertTurnTakerType insertTurnTakerType;

    bool HasPrefab => prefabToSpawn != null;

    public float Range => range;

    public override void Execute(){
        if (!HasNode){
            Debug.LogWarning("ExplosionEffect executed without a target node.");
            return;
        }
        foreach (var node in targetNode.GetNodesInRadius(range)){
            foreach (var obj in node.GetCombatObjects()){
                var health = obj.GetCombatComponent<HealthComponent>();
                if (health != null){
                    health.TakeDamage(damage);
                }
                if (obj is Unit unit){
                    foreach (var statusEffect in statusEffects) unit.ApplyModifier(statusEffect.Create());
                }
            }
            if (prefabToSpawn != null){
                node.Spawn(prefabToSpawn, insertTurnTakerType);
            }
        }
    }
}