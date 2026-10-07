using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class SquadSelection : MonoBehaviour{
    [BoxGroup("References")][Required][SerializeField] CampaignManager campaignManager;
    public CampaignState CampaignState => campaignManager.State;

    public void AddMemberToSquad(SquadMember member){
        CampaignState.Squad.AddMember(member);
    }

    public void RemoveMemberFromSquad(SquadMember member){ 
        CampaignState.Squad.RemoveMember(member);
        EmptyMember(member);
    }

    void EmptyMember(SquadMember member){
        foreach (var equipment in member.Equipment.Copy()) RemoveEquipmentFromSquadMemeber(member, equipment);
    }
    
    public void RemoveEquipmentFromSquadMemeber(SquadMember squadMember, Equipment equipment){
        squadMember.RemoveEquipment(equipment);
        CampaignState.Equipment.AddEquipment(equipment);
    }
    
    public void AddEquipmentToSquadMemeber(SquadMember squadMember, Equipment equipment){
        squadMember.AddEquipment(equipment);
        CampaignState.Equipment.RemoveEquipment(equipment);
    }
}