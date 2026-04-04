using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public abstract class TurnTaker : MonoBehaviour, ITurnTaker{
    [SerializeField] int priority;
    public int Priority => priority;
    public UnityAction<ITurnTaker> OnTurnCompleted{ get; set; }

    [FoldoutGroup("Events")] public UnityEvent<ITurnTaker> onStartTurn{ get; } = new();

    [FoldoutGroup("Events")] public UnityEvent<ITurnTaker> onEndTurn{ get; } = new();

    public virtual void EndTurn(){
        Debug.Log($"{this} ended their turn.");
        onEndTurn.Invoke(this);
    }

    public virtual void StartTurn(){
        Debug.Log($"{this} started turn.");
        onStartTurn.Invoke(this);
    }
    public void CompleteTurn(){
        OnTurnCompleted?.Invoke(this);
    }
}