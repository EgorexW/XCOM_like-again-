using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public abstract class TurnTaker : MonoBehaviour, ITurnTaker{
    [SerializeField] int priority;
    public int Priority => priority;
    public UnityAction<ITurnTaker> OnTurnCompleted{ get; set; }

    [FoldoutGroup("Events")]
    public UnityEvent onStartTurn;

    [FoldoutGroup("Events")]
    public UnityEvent onEndTurn;

    public virtual void OnEndTurn(){
        Debug.Log($"{this} ended their turn.");
        onEndTurn.Invoke();
    }

    public virtual void OnStartTurn(){
        Debug.Log($"{this} started turn.");
        onStartTurn.Invoke();
    }
    public void CompleteTurn(){
        OnTurnCompleted?.Invoke(this);
    }
}