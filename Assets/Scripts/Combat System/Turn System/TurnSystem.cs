using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnSystem : MonoBehaviour, ITurnSystem{
    readonly List<ITurnTaker> turnTakers = new();

    int index = -1;

    public int TurnTakersCount => turnTakers.Count;
    public ITurnTaker CurrentTurnTaker{
        get{
            if (index >= 0 && index < turnTakers.Count){
                return turnTakers[index];
            }
            // Debug.LogWarning($"TurnManager: Index {index} is out of bounds for TurnTakers (Count: {turnTakers.Count})");
            return null;
        }
    }

    public void AddTurnTaker(ITurnTaker turnTaker, InsertTurnTakerType insertTurnTakerType){
        switch (insertTurnTakerType){
            case InsertTurnTakerType.Next:
                var i1 = index + 1;
                turnTakers.Insert(i1, turnTaker);
                break;
            case InsertTurnTakerType.Last:
                var i2 = index;
                index += 1;
                if (i2 < 0){
                    i2 = 0;
                }
                turnTakers.Insert(i2, turnTaker);
                break;
        }
        turnTaker.OnTurnCompleted = TurnCompleted;
    }

    void TurnCompleted(ITurnTaker turnTaker){
        if (CurrentTurnTaker != turnTaker){
            Debug.LogWarning("Turn completed by " + turnTaker + " but current turn taker is " + CurrentTurnTaker, this);
            return;
        }
        NextTurn();
    }

    public void NextTurn(){
        CurrentTurnTaker?.EndTurn();
        index++;
        if (index >= TurnTakersCount){
            index = 0;
        }
        CurrentTurnTaker?.StartTurn();
    }
}

public enum InsertTurnTakerType{
    Next,
    Last
}

public interface ITurnTaker{
    public UnityAction<ITurnTaker> OnTurnCompleted{ get; set; }
    void EndTurn();

    void StartTurn();
    // void CompleteTurn();
    public UnityEvent<ITurnTaker> onStartTurn{ get; }
    public UnityEvent<ITurnTaker> onEndTurn{ get; }
}

public interface ITurnSystem{
    void NextTurn();
}