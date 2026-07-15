using System;
using System.Collections.Generic;

[Serializable]
public class SaveData{
    public ResourcesSaveData resources = new();
    public RecruitIntakeSaveData recruitIntake = new();
}

[Serializable]
public class ResourcesSaveData{
    public int money;
    public List<SquadMemberSaveData> members = new();
    public List<SquadMemberSaveData> retiredMembers = new();
    public List<string> equipmentGuids = new();
}

[Serializable]
public class SquadMemberSaveData{
    public string name;
    public string unitGuid;
    public List<string> equipmentGuids = new();
    public int upkeepCost;
    public int missionsCompleted;
    public int retirementThreshold;
    public bool alive;
}

[Serializable]
public class RecruitIntakeSaveData{
    public int missionsCompletedSinceLastDelivery;
}
