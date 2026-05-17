using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class ExplosionEffect : CombatEffect{
    [SerializeField] float range = 1;
    [FormerlySerializedAs("damage")] [SerializeField] int damageValue = 1;
    [SerializeField] List<UnitModifierFactory> statusEffects;
    [SerializeField] [BoxGroup("Spawn Settings")] GameObject prefabToSpawn;

    // public float Range => range;

    public override void Execute(){
        if (!HasNode){
            Debug.LogWarning("ExplosionEffect executed without a target node.");
            return;
        }
        var damage = new Damage(){
            value = this.damageValue,
            source = sourceObject
        };
        foreach (var node in GetAffectedNodes(targetNode)){
            // Debug.Log($"ExplosionEffect hitting node {node.GetPos()}");
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
                node.Spawn(prefabToSpawn);
            }
        }
    }

    public List<CombatGridNode> GetAffectedNodes(CombatGridNode targetNodeTmp){
        var nodes = targetNodeTmp.GetNodesInRadius(range);
        foreach (var node in nodes.Copy()){
            if (!node.LineUnobstructed(targetNodeTmp, GridBlockingFlags.ExplosionBlocker)){
                nodes.Remove(node);
            }
        }
        return nodes;
    }
}