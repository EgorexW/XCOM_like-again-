using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class SquadMember{
    [SerializeField] string name;
    [SerializeField] GameObject combatPrefab;
    [SerializeField] List<Equipment> equipment;
    [SerializeField] int upkeepCost;
    [SerializeField] int missionsCompleted = 0;
    [SerializeField] int retirementThreshold = 20;

    [HideInEditorMode] public bool alive = true;

    public SquadMember(string name, GameObject combatPrefabTmp, List<Equipment> equipmentTmp, int upkeepCost = 10, int retirementThreshold = 20){
        this.name = name;
        combatPrefab = combatPrefabTmp;
        equipment = equipmentTmp;
        this.upkeepCost = upkeepCost;
        this.retirementThreshold = retirementThreshold;
        this.missionsCompleted = 0;
    }

    public string Name => name;
    public GameObject CombatPrefab => combatPrefab;
    public IReadOnlyList<Equipment> Equipment => equipment;
    public int UpkeepCost => upkeepCost;

    public int MissionsCompleted => missionsCompleted;
    public int RetirementThreshold => retirementThreshold;
    
    public void SetMissionsCompleted(int value){
        missionsCompleted = value;
        onChanged?.Invoke(this);
    }

    public void SetRetirementThreshold(int value){
        retirementThreshold = value;
        onChanged?.Invoke(this);
    }

    public event Action<SquadMember> onChanged;

    public void RemoveEquipment(Equipment equipmentTmp){
        equipment.Remove(equipmentTmp);
        onChanged?.Invoke(this);
    }

    public void AddEquipment(Equipment equipment1){
        equipment.Add(equipment1);
        onChanged?.Invoke(this);
    }

    public void IncrementMissionsCompleted(){
        missionsCompleted++;
        onChanged?.Invoke(this);
    }
}