using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = StringKeys.AssetMenuSquadDataBasePath)]
public class SquadData : ScriptableObject{
    [SerializeField] List<SquadMember> squadMembers = new();

    [FoldoutGroup("Events")] public UnityEvent<SquadData> onChanged;

    public IReadOnlyList<SquadMember> SquadMembers => squadMembers.AsReadOnly();

    public void Clear(){
        foreach (var squadMember in squadMembers.Copy()) RemoveMember(squadMember);
        onChanged.Invoke(this);
    }

    public void AddMember(SquadMember member){
        squadMembers.Add(member);
        member.onChanged.AddListener(OnMemberChanged);
        onChanged.Invoke(this);
    }

    void OnMemberChanged(SquadMember arg0){
        onChanged.Invoke(this);
    }

    public void DeepCopy(SquadData initSquadData){
        Clear();
        foreach (var member in initSquadData.SquadMembers) AddMember(member.Copy());
    }

    public void RemoveMember(SquadMember member){
        squadMembers.Remove(member);
        member.onChanged.RemoveListener(OnMemberChanged);
        onChanged.Invoke(this);
    }
}

[Serializable]
public class SquadMember{
    [SerializeField] string name;
    [SerializeField] GameObject combatPrefab;
    [SerializeField] List<Equipment> equipment;
    [SerializeField] int upkeepCost;

    [HideInEditorMode] public bool alive = true;

    public SquadMember(string name, GameObject combatPrefabTmp, List<Equipment> equipmentTmp, int upkeepCost = 10){
        this.name = name;
        combatPrefab = combatPrefabTmp;
        equipment = equipmentTmp;
        this.upkeepCost = upkeepCost;
    }

    public string Name => name;
    public GameObject CombatPrefab => combatPrefab;
    public IReadOnlyList<Equipment> Equipment => equipment;
    public int UpkeepCost => upkeepCost;

    [HideInInspector] [FoldoutGroup("Events")] public UnityEvent<SquadMember> onChanged = new();

    public void RemoveEquipment(Equipment equipmentTmp){
        equipment.Remove(equipmentTmp);
        onChanged.Invoke(this);
    }

    public void AddEquipment(Equipment equipment1){
        equipment.Add(equipment1);
        onChanged.Invoke(this);
    }
}

public static class SquadExtensions{
    public static SquadMember Copy(this SquadMember member){
        return new SquadMember(member.Name, member.CombatPrefab, member.Equipment.ToList(), member.UpkeepCost){
            alive = member.alive
        };
    }
}