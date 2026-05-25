using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class HealthComponent : CombatComponent{
    [SerializeField] int maxHealth = 1;
    [SerializeField] Vector2Int startingHealth = Vector2Int.one;

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

    public void Die(){
        var bleedOut = CombatObject.GetCombatComponent<BleedOutComponent>();
        if (bleedOut != null){
            bleedOut.BleedOut();
            return;
        }

        CombatObject.AddFlag(CombatObjectFlags.Dead);
        Debug.Log($"{CombatObject.Name} died.", this);
        CombatObject.Remove();
    }
}

[Serializable]
public struct Damage{
    public int value;
    public ICombatObject source;
}