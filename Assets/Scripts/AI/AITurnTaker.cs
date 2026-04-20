using System.Collections;
using System.Linq;
using UnityEngine;

public class AITurnTaker : UnitsTurnTaker{
    const float TIME_BETWEEN_UNIT_TURNS = 0.5f;
    
    Coroutine turnResolveCoroutine;
    
    public override void EndTurn(){
        base.EndTurn();
        if (turnResolveCoroutine == null){
            return;
        }
        StopCoroutine(turnResolveCoroutine);
        turnResolveCoroutine = null;
    }

    public override void StartTurn(){
        base.StartTurn();
        turnResolveCoroutine = StartCoroutine(ResolveTurn());
    }

    IEnumerator ResolveTurn(){
        var combatUnits = Units.ToList();
        combatUnits.Shuffle();
        foreach (var unit in combatUnits){
            var aiBrain = unit.GetComponentInChildren<AIBrain>();
            if (aiBrain != null){
                yield return StartCoroutine(aiBrain.ResolveTurn());
            }
            else{
                Debug.LogWarning($"Unit {unit.Name} does not have an AIBrain component. Skipping turn resolution for this unit.");
            }
            yield return new WaitForSeconds(TIME_BETWEEN_UNIT_TURNS);
        }
        CompleteTurn();
    }

}