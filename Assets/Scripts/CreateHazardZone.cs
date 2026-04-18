using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CreateHazardZone : MonoBehaviour{
    [SerializeField] ExplosionEffect explosionEffect;
    
    HazardCombatObjectExtensions hazardCombatObjectExtensions;

    void Awake(){
        var combatObject = GetComponent<ICombatObject>();
        combatObject.onInit.AddListener(CreateHazard);
        combatObject.onRemove.AddListener(RemoveHazard);
    }

    void RemoveHazard(ICombatObject arg0){
        if (hazardCombatObjectExtensions != null){
            hazardCombatObjectExtensions.Remove();
        }
    }

    public void CreateHazard(ICombatObject combatObject){
        var range = explosionEffect.Range;
        var center = combatObject.GetCenter();
        var nodes = combatObject.Grid().GetNodesInRadius(center, range);
        hazardCombatObjectExtensions = new HazardCombatObjectExtensions();
        combatObject.CombatSystem.AddCombatObject(hazardCombatObjectExtensions, nodes);
    }
}

public class HazardCombatObjectExtensions : CombatObjectExtensions{
    
}

public class HazardComponent : BaseCombatComponent
{
    [SerializeField] HazardFlags hazardFlags;
    [Range(0, 1)][SerializeField] float intensity;

    public HazardFlags HazardFlags => hazardFlags;
    public float Intensity => intensity;
}


public enum HazardFlags{
    None = 0,
    DamageSoon  = 1 << 0,
}