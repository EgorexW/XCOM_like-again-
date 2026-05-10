using UnityEngine;
using UnityEngine.Serialization;

public class SquadSelection : MonoBehaviour {
    [SerializeField] SquadData squadData;
    [FormerlySerializedAs("playerResourcesData")] [SerializeField] ResourcesData resourcesData;

    public void AddMemberToSquad(SquadMember member){
        squadData.AddMember(member);
        resourcesData.RemoveMember(member);
    }

    public void RemoveMemberFromSquad(SquadMember member){
        squadData.RemoveMember(member);
        resourcesData.AddMember(member);
        EmptyMember(member);
    }

    void EmptyMember(SquadMember member){
        foreach (var equipment in member.Equipment.Copy()){
            RemoveEquipmentFromSquadMemeber(member, equipment);
        }
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