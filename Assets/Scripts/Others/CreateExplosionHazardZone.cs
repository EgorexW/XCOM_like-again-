using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class CreateExplosionHazardZone : MonoBehaviour{
    [BoxGroup("References")][Required][SerializeField] ExplosionEffect explosionEffect;
    [SerializeField] HazardSettings hazardSettings;
    
    CombatObject hazardCombatObject;

    void Awake(){
        var combatObject = GetComponent<ICombatObject>();
        combatObject.onInit.AddListener(CreateHazard);
        combatObject.onRemove.AddListener(RemoveHazard);
    }

    void RemoveHazard(ICombatObject arg0){
        if (hazardCombatObject != null){
            hazardCombatObject.Remove();
        }
    }

    public void CreateHazard(ICombatObject combatObject){
        var range = explosionEffect.Range;
        var center = combatObject.GetCenter();
        var nodes = combatObject.Grid().GetNodesInRadius(center, range);
        var gameObj = new GameObject("Hazard Zone");
        gameObj.transform.SetParent(transform);
        hazardCombatObject = gameObj.AddComponent<CombatObject>();
        HazardComponent hazardComponent = gameObj.AddComponent<HazardComponent>();
        hazardComponent.settings = hazardSettings;
        combatObject.CombatSystem.AddCombatObject(hazardCombatObject, nodes);
    }
}