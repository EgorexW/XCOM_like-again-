using System;
using System.Collections.Generic;

[Serializable]
public class CampaignSaveData{
    public MembersSaveData members = new();
    public EquipmentSaveData equipment = new();
    public int money;
    public SquadSaveData squad = new();
}

[Serializable]
public class MembersSaveData{
    public List<SquadMemberSaveData> activeMembers = new();
    public List<SquadMemberSaveData> retiredMembers = new();
    public List<SquadMemberSaveData> deadMembers = new();
}

[Serializable]
public class SquadMemberSaveData{
    public string name;
    public string combatPrefabGuid;
    public List<string> equipmentGuids = new();
    public int upkeepCost;
    public int missionsCompleted;
    public int retirementThreshold;
    public bool alive;
}

[Serializable]
public class EquipmentSaveData{
    public List<string> equipmentGuids = new();
}

[Serializable]
public class SquadSaveData{
    // Indices into MembersSaveData.activeMembers - squad members are always a subset of the active roster.
    public List<int> activeMemberIndices = new();
}
