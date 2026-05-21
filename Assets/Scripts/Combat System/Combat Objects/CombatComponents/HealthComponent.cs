using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class HealthComponent : CombatComponent{
    [SerializeField] int maxHealth = 1;
    [SerializeField] Vector2Int startingHealth = Vector2Int.one;
    [SerializeField] List<CombatEffect> onDeathEffects;

    [FoldoutGroup("Events")] public UnityEvent<HealthComponent> onHealthChanged;
    public int Health{ get; private set; }
    public int MaxHealth => maxHealth;


    protected void Start(){
        Health = Random.Range(startingHealth.x, startingHealth.y + 1);
    }

    public Damage LastDamage { get; private set; }

    public void TakeDamage(Damage damage){
        Health -= damage.value;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
        LastDamage = damage;
        onHealthChanged?.Invoke(this);
        if (Health <= 0){
            Die();
        }
    }

    public void Die(bool ignoreBleedOut = false){
        // if (!ignoreBleedOut){
            var bleedOut = CombatObject.GetCombatComponent<BleedOutComponent>();
            if (bleedOut != null){
                bleedOut.BleedOut();
                return;
            }
        // }

        Debug.Log($"{CombatObject.Name} died.", this);
        foreach (var effect in onDeathEffects){
            effect.targetNode = CombatObject.GetCenterNode();
            effect.Execute();
        }
        CombatObject.Remove();
    }
}

[Serializable]
public struct Damage{
    public int value;
    public ICombatObject source;
}