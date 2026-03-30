using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnSystem : MonoBehaviour, ITurnSystem{
    List<ITurnTaker> turnTakers = new();

    int index = -1;
        
    public int TurnTakersCount => turnTakers.Count;
    public ITurnTaker CurrentTurnTaker
    {
        get
        {
            if (index >= 0 && index < turnTakers.Count){
                return turnTakers[index];
            }
            // Debug.LogWarning($"TurnManager: Index {index} is out of bounds for TurnTakers (Count: {turnTakers.Count})");
            return null;
        }
    }

    public void AddTurnTaker(ITurnTaker turnTaker){
        var currentTurnTaker = CurrentTurnTaker;
        turnTakers.Add(turnTaker);
        turnTaker.OnTurnCompleted = TurnCompleted;
        turnTakers.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        if (currentTurnTaker != null){
            index = turnTakers.IndexOf(currentTurnTaker);
        }
    }

    void TurnCompleted(ITurnTaker turnTaker){
        if (CurrentTurnTaker != turnTaker){
            Debug.LogWarning("Turn completed by " + turnTaker + " but current turn taker is " + CurrentTurnTaker, this);
            return;
        }
        NextTurn();
    }

    public void NextTurn(){
        CurrentTurnTaker?.OnEndTurn();
        index++;
        if (index >= TurnTakersCount){
            index = 0;
        }
        CurrentTurnTaker?.OnStartTurn();
    }

}

public interface ITurnTaker{
    public int Priority { get; }
    public UnityAction<ITurnTaker> OnTurnCompleted { get; set; }
    void OnEndTurn();
    void OnStartTurn();
}

public interface ITurnSystem{
    void NextTurn();
}