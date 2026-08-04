using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[Serializable]
public class MembersState{
    [FormerlySerializedAs("members")] [SerializeField] List<SquadMember> activeMembers = new();
    [SerializeField] List<SquadMember> retiredMembers = new();
    [SerializeField] List<SquadMember> deadMembers = new();
    
    public event Action onChanged;

    public IReadOnlyList<SquadMember> ActiveMembers => activeMembers;
    public IReadOnlyList<SquadMember> RetiredMembers => retiredMembers;
    public IReadOnlyList<SquadMember> DeadMembers =>  deadMembers;
    
        
    public void AddMember(SquadMember member)    => MoveMemberTo(member, activeMembers);
    public void RetireMember(SquadMember member) => MoveMemberTo(member, retiredMembers);
    public void DieMember(SquadMember member)    => MoveMemberTo(member, deadMembers);
    public void RemoveMember(SquadMember member) => MoveMemberTo(member, destinationList: null);

    private void MoveMemberTo(SquadMember member, List<SquadMember> destinationList) {
        if (member == null) {
            Debug.LogError("Attempted to move a null member. Operation aborted.");
            return;
        }

        // // --- Equipment Stripping Logic ---
        // // Create a snapshot copy of the equipment list. This prevents a 
        // // "Collection was modified" exception while iterating over member.Equipment 
        // // as pieces are actively being removed.
        // var equipmentSnapshot = new List<Equipment>(member.Equipment);
        // foreach (var equipmentPiece in equipmentSnapshot) {
        //     member.RemoveEquipment(equipmentPiece); // Remove item from member's inventory
        //     AddEquipment(equipmentPiece);          // Transfer item back to the main squad inventory
        // }
        
        member.onChanged -= OnMemberChanged;
        
        activeMembers.Remove(member);
        retiredMembers.Remove(member);
        deadMembers.Remove(member);
        
        if (destinationList != null) {
            destinationList.Add(member);
            // if (destinationList == members) {
                member.onChanged  += OnMemberChanged;
            // }
        }
        
        onChanged?.Invoke();
    }

    private void OnMemberChanged(SquadMember arg0) => onChanged?.Invoke();
}
