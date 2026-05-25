using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class TurnSystem : MonoBehaviour{
    readonly List<ITurnTaker> turnTakers = new();

    int currentIndex;
    bool isRunning;
    bool isTurnActive;

    public int TurnTakersCount => turnTakers.Count;
    public int RoundCount { get; private set; }

    [FoldoutGroup("Events")] public UnityEvent<ITurnTaker> onStartTurn;
    [FoldoutGroup("Events")] public UnityEvent<ITurnTaker> onEndTurn;
    [FoldoutGroup("Events")] public UnityEvent<int> onRoundPassed = new();

    protected void Update(){
        if (isRunning && !isTurnActive && TurnTakersCount > 0){
            NextTurn();
        }
    }

    public void StartSystem(){
        if (TurnTakersCount == 0){
            Debug.LogWarning("Tried to start battle with 0 turn takers.", this);
            return;
        }
        isRunning = true;
        RoundCount = 1;
        StartTurn();
    }

    public void Stop(){
        if (isTurnActive){
            EndTurn();
        }

        isRunning = false;
    }

    public ITurnTaker GetCurrentTurnTaker(){
        if (isRunning && currentIndex >= 0 && currentIndex < turnTakers.Count){
            return turnTakers[currentIndex];
        }
        Debug.LogWarning("GetCurrentTurnTaker called but turn system is not running or index is out of range.", this);
        return null;
    }

    public void AddTurnTaker(ITurnTaker turnTaker, InsertTurnTakerType insertType){
        // Debug.Log($"Adding {turnTaker} to turn system with insert type {insertType}", this);

        if (!isRunning){
            if (insertType == InsertTurnTakerType.Next){
                turnTakers.Insert(0, turnTaker);
            }
            else if (insertType == InsertTurnTakerType.Last){
                turnTakers.Add(turnTaker);
            }
        }
        else{
            if (insertType == InsertTurnTakerType.Next){
                turnTakers.Insert(currentIndex + 1, turnTaker);
            }
            else if (insertType == InsertTurnTakerType.Last){
                turnTakers.Insert(currentIndex, turnTaker);
                currentIndex++;
            }
        }

        turnTaker.OnTurnCompleted = TurnCompleted;
        turnTaker.TurnSystem = this;
    }

    public void RemoveTurnTaker(ITurnTaker turnTaker){
        var removedIndex = turnTakers.IndexOf(turnTaker);

        if (removedIndex == -1){
            Debug.LogWarning($"Attempted to remove turn taker {turnTaker} but it was not found.", this);
            return;
        }

        Debug.Log($"Removing {turnTaker}", this);

        if (TurnTakersCount < 2){
            Stop();
            turnTakers.Clear();
            return;
        }

        if (removedIndex == currentIndex){
            EndTurn();
        }

        turnTakers.RemoveAt(removedIndex);

        if (removedIndex <= currentIndex){
            currentIndex--;
        }
    }

    void TurnCompleted(ITurnTaker turnTaker){
        if (GetCurrentTurnTaker() != turnTaker || !isTurnActive){
            Debug.LogWarning($"TurnCompleted called by {turnTaker} but it's not their turn or turn is not active.",
                this);
            return;
        }
        EndTurn();
    }

    void NextTurn(){
        if (isTurnActive){
            Debug.LogWarning($"NextTurn called early! Ending current turn for {GetCurrentTurnTaker()}", this);
            EndTurn();
        }

        currentIndex++;
        StartTurn();
    }

    void StartTurn(){
        if (currentIndex >= TurnTakersCount){
            currentIndex = 0;
            RoundCount++;
            onRoundPassed.Invoke(RoundCount);
        }

        var current = GetCurrentTurnTaker();
        if (current == null){
            return;
        }
        isTurnActive = true;

        Debug.Log($"Starting turn for {current}", this);
        current.StartTurn();
        onStartTurn.Invoke(current);
    }

    void EndTurn(){
        isTurnActive = false;
        var current = GetCurrentTurnTaker();

        Debug.Log($"Ending turn for {current}", this);
        current?.EndTurn();
        onEndTurn.Invoke(current);
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
    public TurnSystem TurnSystem{ get; set; }
    public UnityEvent<ITurnTaker> onStartTurn{ get; }
    public UnityEvent<ITurnTaker> onEndTurn{ get; }
}