using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public abstract class TurnTaker : MonoBehaviour, ITurnTaker{
    public UnityAction<ITurnTaker> OnTurnCompleted{ get; set; }
    protected bool turnActive;

    [FoldoutGroup("Events")] public UnityEvent<ITurnTaker> onStartTurn{ get; } = new();

    [FoldoutGroup("Events")] public UnityEvent<ITurnTaker> onEndTurn{ get; } = new();

    public virtual void EndTurn(){
        // Debug.Log($"{this} ended their turn.");
        onEndTurn.Invoke(this);
        turnActive = false;
    }

    public virtual void StartTurn(){
        // Debug.Log($"{this} started turn.");
        onStartTurn.Invoke(this);
        turnActive = true;
    }

    public TurnSystem TurnSystem{ get; set; }

    public void CompleteTurn(){
        if (!turnActive){
            Debug.LogWarning("Trying to complete turn for " + this + " but their turn is not active.", this);
            return;
        }
        OnTurnCompleted?.Invoke(this);
    }
}