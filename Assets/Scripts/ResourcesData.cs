using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = StringKeys.AssetMenuPlayerResourcesDataBasePath)]
public class ResourcesData : ScriptableObject{
    [SerializeField] List<SquadMember> members;
    [SerializeField] List<Equipment> equipment;

    [FoldoutGroup("Events")] public UnityEvent<ResourcesData> onChanged = new();

    public IReadOnlyList<SquadMember> Members  => members;
    public IReadOnlyList<Equipment> Equipment => equipment;
    
    public void DeepCopy(ResourcesData init){
        Clear();
        foreach (var member in init.Members){
            AddMember(member);
        }
        foreach (var equipmentPiece in init.Equipment){
            AddEquipment(equipmentPiece);
        }
    }

    public void AddEquipment(Equipment equipment){
        this.equipment.Add(equipment);
        onChanged.Invoke(this);
    }

    public void AddMember(SquadMember member){
        members.Add(member);
        member.onChanged.AddListener(OnMemberChanged);
        onChanged.Invoke(this);
    }

    void OnMemberChanged(SquadMember arg0){
        onChanged.Invoke(this);
    }

    void Clear(){
        foreach (var member in members.Copy()){
            RemoveMember(member);
        }
        equipment.Clear();
        onChanged.Invoke(this);
    }
    public void RemoveMember(SquadMember member){
        members.Remove(member);
        member.onChanged.RemoveListener(OnMemberChanged);
        onChanged.Invoke(this);
    }

    public void RemoveEquipment(Equipment equipment1){
        equipment.Remove(equipment1);
        onChanged.Invoke(this);
    }
}