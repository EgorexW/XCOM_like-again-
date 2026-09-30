using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SquadState{
    [SerializeField] List<SquadMember> squadMembers = new();

    public event Action onChanged;

    public IReadOnlyList<SquadMember> Members => squadMembers.AsReadOnly();

    public void Clear(){
        foreach (var squadMember in squadMembers.Copy()) RemoveMember(squadMember);
        onChanged?.Invoke();
    }

    public void AddMember(SquadMember member){
        squadMembers.Add(member);
        member.onChanged += OnMemberChanged;
        onChanged?.Invoke();
    }

    void OnMemberChanged(SquadMember arg0){
        onChanged?.Invoke();
    }

    public void RemoveMember(SquadMember member){
        squadMembers.Remove(member);
        member.onChanged -= OnMemberChanged;
        onChanged?.Invoke();
    }
}