using UnityEngine;
using UnityEngine.Serialization;

public class SquadSelection : MonoBehaviour{
    [SerializeField] SquadData squadData;
    [FormerlySerializedAs("playerResourcesData")] [SerializeField] ResourcesData resourcesData;

    public void AddMemberToSquad(SquadMember member){
        squadData.AddMember(member);
        // Do not remove from resourcesData anymore
    }

    public void RemoveMemberFromSquad(SquadMember member){
        squadData.RemoveMember(member);
        // Do not add to resourcesData anymore, they never left
        EmptyMember(member);
    }

    void EmptyMember(SquadMember member){
        foreach (var equipment in member.Equipment.Copy()) RemoveEquipmentFromSquadMemeber(member, equipment);
    }

    public SquadData Squad => squadData;
    public ResourcesData Resources => resourcesData;

    public void RemoveEquipmentFromSquadMemeber(SquadMember squadMember, Equipment equipment){
        squadMember.RemoveEquipment(equipment);
        resourcesData.AddEquipment(equipment);
    }

    public void AddEquipmentToSquadMemeber(SquadMember squadMember, Equipment equipment){
        squadMember.AddEquipment(equipment);
        resourcesData.RemoveEquipment(equipment);
    }
}