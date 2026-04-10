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
                    index -= 1;
                    i2 = TurnTakersCount;
                }
                turnTakers.Insert(i2, turnTaker);
                break;
        }
        turnTaker.OnTurnCompleted = TurnCompleted;
        turnTaker.TurnSystem = this;
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

    public void RemoveTurnTaker(ITurnTaker turnTaker){
        int removedIndex = turnTakers.IndexOf(turnTaker);
        if (removedIndex == -1){
            Debug.LogWarning($"Attempted to remove turn taker {turnTaker} but it was not found in the list.", this);
            return;
        }
        if (TurnTakersCount < 2){
            index = -1;
            turnTakers.Clear();
            return;
        }
        if (removedIndex == index){
            NextTurn();
        }
        if (removedIndex < index){
            index--;
        }
        turnTakers.RemoveAt(removedIndex);
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
    public TurnSystem TurnSystem { get; set; }
    public UnityEvent<ITurnTaker> onStartTurn{ get; }
    public UnityEvent<ITurnTaker> onEndTurn{ get; }
}

public interface ITurnSystem{
    void NextTurn();
}