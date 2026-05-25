using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class AISurrenderActionCreator : BasicAIActionCreator{
    [SerializeField] float baseSurrenderScore = -60f;
    [SerializeField] float outnumberModifier = 20f;
    [SerializeField] float pacifiedTeammateModifier = 5f;
    [SerializeField] Vector2 characterModifierRange = new Vector2(-25, 25f);

    float characterModifier = 0f;

    protected void Awake(){
        characterModifier = characterModifierRange.Random();
    }

    protected override AIAction GetAIAction(AIContext context, UnitAction action){
        if (action.ValidateAction() != UnitActionValidation.Valid){
            return AIAction.Invalid;
        }
        
        int activeAllies = 0;
        foreach (var ally in context.allies){
            if (!ally.Flags.HasFlag(CombatObjectFlags.Pacified)){
                activeAllies++;
            }
        }
        int activeEnemies = 0;
        foreach (var enemy in context.enemies){
            if (!enemy.Flags.HasFlag(CombatObjectFlags.Pacified)){
                activeEnemies++;
            }
        }

        var outnumberRatio = (float)activeEnemies / activeAllies;
        
        int pacifiedCount = 0;
        foreach (var ally in context.allies){
            if (ally.Flags.HasFlag(CombatObjectFlags.Pacified)){
                pacifiedCount++;
            }
        }
        
        float finalSurrenderScore = baseSurrenderScore + (outnumberRatio * outnumberModifier) + (pacifiedCount * pacifiedTeammateModifier) + characterModifier;

        if (context.debug){
            Debug.Log($"[AISurrenderActionCreator] Unit: {context.unit.Name}, activeAllies: {activeAllies}, activeEnemies: {activeEnemies}, outnumberRatio: {outnumberRatio}, pacifiedTeammatesCount: {pacifiedCount}, baseSurrenderScore: {baseSurrenderScore}, finalSurrenderScore: {finalSurrenderScore}");
        }

        if (finalSurrenderScore > 0f){
            return new AIAction(action, null, finalSurrenderScore);
        }

        return AIAction.Invalid;
    }
}
