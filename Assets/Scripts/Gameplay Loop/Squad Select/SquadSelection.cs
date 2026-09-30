using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class SquadSelection : MonoBehaviour{
    [BoxGroup("References")][Required][SerializeField] CampaignStateHolder  campaignStateHolder;
    public CampaignState CampaignState => campaignStateHolder.State;

    public void AddMemberToSquad(SquadMember member){
        campaignStateHolder.State.Squad.AddMember(member);
    }

    public void RemoveMemberFromSquad(SquadMember member){ 
        campaignStateHolder.State.Squad.RemoveMember(member);
        EmptyMember(member);
    }

    void EmptyMember(SquadMember member){
        foreach (var equipment in member.Equipment.Copy()) RemoveEquipmentFromSquadMemeber(member, equipment);
    }
    
    public void RemoveEquipmentFromSquadMemeber(SquadMember squadMember, Equipment equipment){
        squadMember.RemoveEquipment(equipment);
        campaignStateHolder.State.Equipment.AddEquipment(equipment);
    }
    
    public void AddEquipmentToSquadMemeber(SquadMember squadMember, Equipment equipment){
        squadMember.AddEquipment(equipment);
        campaignStateHolder.State.Equipment.RemoveEquipment(equipment);
    }
}