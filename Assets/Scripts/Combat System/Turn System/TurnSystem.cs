using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class TurnSystem : MonoBehaviour, ITurnSystem{
    readonly List<ITurnTaker> turnTakers = new();

    int index = -1;

    public int TurnTakersCount => turnTakers.Count;

    [FoldoutGroup("Events")] public UnityEvent<ITurnTaker> onStartTurn;
    [FoldoutGroup("Events")] public UnityEvent<ITurnTaker> onEndTurn;
    
    bool turnActive = false;

    void Update(){
        if (!turnActive && index >= 0){
            NextTurn();
        }
    }

    public ITurnTaker GetCurrentTurnTaker(){
        if (index >= 0 && index < turnTakers.Count){
            return turnTakers[index];
        }
        return null;
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
        if (GetCurrentTurnTaker() != turnTaker){
            Debug.LogWarning("Turn completed by " + turnTaker + " but current turn taker is " + GetCurrentTurnTaker(),
                this);
            return;
        }
        EndTurn();
    }

    public void NextTurn(){
        if (turnActive){
            Debug.LogWarning("NextTurn called while turn is still active. Ending current turn for " + GetCurrentTurnTaker(), this);
            EndTurn();
        }
        index++;
        StartTurn();
    }

    void EndTurn(){
        turnActive = false;
        GetCurrentTurnTaker()?.EndTurn();
        Debug.Log("Ending turn for " + GetCurrentTurnTaker(), this);
        onEndTurn.Invoke(GetCurrentTurnTaker());
    }

    public void RemoveTurnTaker(ITurnTaker turnTaker){
        var removedIndex = turnTakers.IndexOf(turnTaker);
        Debug.Log("Removing " + turnTaker, this);
        if (removedIndex == -1){
            Debug.LogWarning($"Attempted to remove turn taker {turnTaker} but it was not found in the list.", this);
            return;
        }
        if (TurnTakersCount < 2){
            Stop();
            turnTakers.Clear();
            return;
        }
        bool startNewTurn = false;
        if (turnTaker == GetCurrentTurnTaker()){
            EndTurn();
            startNewTurn = true;
        }
        turnTakers.RemoveAt(removedIndex);
        if (startNewTurn){
            StartTurn();
        }
        if (removedIndex < index){
            index--;
        }
    }

    void StartTurn(){
        if (index >= TurnTakersCount){
            index = 0;
        }
        turnActive = true;
        GetCurrentTurnTaker()!.StartTurn();
        Debug.Log("Starting turn for " + GetCurrentTurnTaker(), this);
        onStartTurn.Invoke(GetCurrentTurnTaker());
    }

    public void Stop(){
        EndTurn();
        index = -1;
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

public interface ITurnSystem{
    void NextTurn();
}