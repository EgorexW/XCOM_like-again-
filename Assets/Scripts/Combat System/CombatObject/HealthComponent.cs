using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : CombatComponent{
    [SerializeField] float maxHealth = 1;
    float currentHealth;

    [FoldoutGroup("Events")]
    public UnityEvent<HealthComponent> onHealthChanged;
    public float Health => currentHealth;


    void Start(){
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage){
        currentHealth -= damage;
        onHealthChanged?.Invoke(this);
        if (currentHealth <= 0){
            Die();
        }
    }

    void Die(){
        Debug.Log($"{combatObject.Name} died.", combatObject as Object);
        combatObject.Remove();
    }
}