using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Squad Data", menuName = "Data/Squad Data")]
public class SquadData : ScriptableObject {
    [SerializeField] List<SquadMember> squadMembers = new List<SquadMember>();

    public IReadOnlyList<SquadMember> SquadMembers => squadMembers.AsReadOnly();
    
    public void Clear() {
        squadMembers.Clear();
    }
    
    public void AddMember(SquadMember member) {
        squadMembers.Add(member);
    }

    public void Copy(SquadData initSquadData){
        Clear();
        foreach (var member in initSquadData.SquadMembers){
            AddMember(member);
        }
    }

    public void RemoveMember(SquadMember member){
        squadMembers.Remove(member);
    }
}

[Serializable]
public class SquadMember{
    [FormerlySerializedAs("prefab")] public GameObject combatPrefab;
    public string name;
    public bool alive = true;
}