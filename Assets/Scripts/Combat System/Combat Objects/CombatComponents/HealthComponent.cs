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
    public bool IsDead => Health <= 0;


    protected void Start(){
        Health = Random.Range(startingHealth.x, startingHealth.y + 1);
    }

    public Damage LastDamage { get; private set; }

    public void TakeDamage(Damage damage){
        if (IsDead){
            Debug.LogWarning($"{CombatObject.Name} is already dead and cannot take more damage.", this);
            return;
        }
        Health -= damage.value;
        LastDamage = damage;
        onHealthChanged?.Invoke(this);
        if (Health <= 0){
            Die();
        }
    }

    void Die(){
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