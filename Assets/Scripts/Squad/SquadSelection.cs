using UnityEngine;
using UnityEngine.Serialization;

public class SquadSelection : MonoBehaviour {
    [SerializeField] SquadData squadData;
    [FormerlySerializedAs("playerResourcesData")] [SerializeField] ResourcesData resourcesData;

    public void AddMemberToSquad(SquadMember member){
        squadData.AddMember(member);
    }

    public void RemoveMemberFromSquad(SquadMember member){
        squadData.RemoveMember(member);
    }

    public SquadData Squad => squadData;
    public ResourcesData Resources => resourcesData;
}