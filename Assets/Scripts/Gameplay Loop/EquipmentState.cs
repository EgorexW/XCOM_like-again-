using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[Serializable]
public class EquipmentState{
    [FormerlySerializedAs("equipment")] [SerializeField] List<Equipment> equipmentList = new();
    
    public event Action onChanged;

    public IReadOnlyList<Equipment> Equipment => equipmentList;

    public void AddEquipment(Equipment equipment){
        this.equipmentList.Add(equipment);
        onChanged?.Invoke();
    }

    public void RemoveEquipment(Equipment equipment1){
        equipmentList.Remove(equipment1);
        onChanged?.Invoke();
    }

    public void AddEquipment(List<Equipment> equipments){
        foreach (var equipmentPiece in equipments){
            AddEquipment(equipmentPiece);
        }
    }
}