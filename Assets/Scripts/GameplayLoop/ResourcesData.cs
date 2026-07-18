using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = StringKeys.AssetMenuPlayerResourcesDataBasePath)]
public class ResourcesData : ScriptableObject{
    [SerializeField] List<SquadMember> members = new();
    [SerializeField] List<SquadMember> retiredMembers = new();
    [SerializeField] List<SquadMember> deadMembers = new();
    [SerializeField] List<Equipment> equipment= new();
    [SerializeField] int money;

    [FoldoutGroup("Events")] public UnityEvent<ResourcesData> onChanged = new();

    public IReadOnlyList<SquadMember> Members => members;
    public IReadOnlyList<SquadMember> RetiredMembers => retiredMembers;
    public IReadOnlyList<SquadMember> DeadMembers =>  deadMembers;
    public IReadOnlyList<Equipment> Equipment => equipment;
    public int Money => money;

    public void DeepCopy(ResourcesData init){
        Clear();
        foreach (var member in init.Members) AddMember(member.Copy());
        foreach (var member in init.RetiredMembers) AddRetiredMember(member);
        foreach (var member in init.DeadMembers) AddDeadMember(member);
        foreach (var equipmentPiece in init.Equipment) AddEquipment(equipmentPiece);
        money = init.Money;
    }

    void AddDeadMember(SquadMember member){
        deadMembers.Add(member);
        onChanged.Invoke(this);
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

    public void Clear(){
        foreach (var member in members.Copy()) RemoveMember(member);
        equipment.Clear();
        retiredMembers.Clear();
        onChanged.Invoke(this);
    }

    public void SetMoney(int amount){
        money = amount;
        onChanged.Invoke(this);
    }

    public void RemoveMember(SquadMember member){
        foreach (var equipmentPiece in new List<Equipment>(member.Equipment)){
            member.RemoveEquipment(equipmentPiece);
            AddEquipment(equipmentPiece);
        }
        members.Remove(member);
        member.onChanged.RemoveListener(OnMemberChanged);
        onChanged.Invoke(this);
    }

    public void FireMember(SquadMember member){
        RemoveMember(member);
    }

    public void AddRetiredMember(SquadMember member){
        retiredMembers.Add(member);
        onChanged.Invoke(this);
    }

    public void RetireMember(SquadMember member){
        RemoveMember(member);
        AddRetiredMember(member);
    }

    public void DieMember(SquadMember member){
        RemoveMember(member);
        AddDeadMember(member);
    }

    public void RemoveEquipment(Equipment equipment1){
        equipment.Remove(equipment1);
        onChanged.Invoke(this);
    }

    public void ChangeMoney(int amount){
        money += amount;
        onChanged.Invoke(this);
    }

    public void AddEquipment(List<Equipment> equipments){
        foreach (var equipmentPiece in equipments){
            AddEquipment(equipmentPiece);
        }
    }
}