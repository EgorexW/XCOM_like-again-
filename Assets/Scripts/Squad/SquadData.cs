using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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

    public void DeepCopy(SquadData initSquadData){
        Clear();
        foreach (var member in initSquadData.SquadMembers){
            AddMember(member.Copy());
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
    public List<UnitModifierFactory> modifiers;
    
    [HideInEditorMode] public bool alive = true;
}

public static class SquadExtensions{
    public static SquadMember Copy(this SquadMember member){
        return new SquadMember{
            combatPrefab = member.combatPrefab,
            name = member.name,
            modifiers = new List<UnitModifierFactory>(member.modifiers),
            alive = member.alive
        };
    }
}