using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Grenade : TurnTaker
{
    [SerializeField] int turnsToActivate = 1;
    [SerializeField] bool destroy = true;

    [FoldoutGroup("Events")]
    public UnityEvent onActivate = new();


    public override void StartTurn(){
        base.StartTurn();
        turnsToActivate -= 1;
        if (turnsToActivate <= 0){
            Activate();
        }
    }

    void Activate(){
        onActivate.Invoke();
        TurnSystem.RemoveTurnTaker(this);
        if (destroy){
            
        Destroy(gameObject);
        }
    }
}
