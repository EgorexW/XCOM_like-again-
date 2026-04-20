using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : CombatComponent{
    [SerializeField] float maxHealth = 1;
    [SerializeField] List<CombatEffect> onDeathEffects;

    [FoldoutGroup("Events")] public UnityEvent<HealthComponent> onHealthChanged;
    public float Health{ get; private set; }
    public float MaxHealth => maxHealth;
    public bool IsDead => Health <= 0;


    protected void Start(){
        Health = maxHealth;
    }

    public void TakeDamage(float damage){
        if (IsDead){
            Debug.LogWarning($"{CombatObject.Name} is already dead and cannot take more damage.", this);
            return;
        }
        Health -= damage;
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