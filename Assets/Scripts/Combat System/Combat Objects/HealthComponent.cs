using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : CombatComponent{
    [SerializeField] float maxHealth = 1;

    [FoldoutGroup("Events")] public UnityEvent<HealthComponent> onHealthChanged;
    public float Health{ get; private set; }


    protected void Start(){
        Health = maxHealth;
    }

    public void TakeDamage(float damage){
        Health -= damage;
        onHealthChanged?.Invoke(this);
        if (Health <= 0){
            Die();
        }
    }

    void Die(){
        Debug.Log($"{combatObject.Name} died.", combatObject as Object);
        combatObject.Remove();
    }
}