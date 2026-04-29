using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

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

    public void TakeDamage(int damage){
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