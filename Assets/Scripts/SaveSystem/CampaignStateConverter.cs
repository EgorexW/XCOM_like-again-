using System.Linq;

public static class CampaignStateConverter{
    public static SquadMemberSaveData ToSaveData(this SquadMember member, EquipmentAssetRegistry equipmentRegistry,
        UnitPrefabAssetRegistry unitRegistry){
        return new SquadMemberSaveData{
            name = member.Name,
            combatPrefabGuid = unitRegistry.GetGuid(member.CombatPrefab),
            equipmentGuids = member.Equipment.Select(equipmentRegistry.GetGuid).ToList(),
            upkeepCost = member.UpkeepCost,
            missionsCompleted = member.MissionsCompleted,
            retirementThreshold = member.RetirementThreshold,
            alive = member.alive
        };
    }

    public static SquadMember ToSquadMember(this SquadMemberSaveData save, EquipmentAssetRegistry equipmentRegistry,
        UnitPrefabAssetRegistry unitRegistry){
        var equipment = save.equipmentGuids.Select(equipmentRegistry.GetAsset).ToList();
        var member = new SquadMember(save.name, unitRegistry.GetAsset(save.combatPrefabGuid), equipment, save.upkeepCost,
            save.retirementThreshold){
            alive = save.alive
        };
        member.SetMissionsCompleted(save.missionsCompleted);
        return member;
    }

    public static CampaignSaveData ToSaveData(this CampaignState state, EquipmentAssetRegistry equipmentRegistry,
        UnitPrefabAssetRegistry unitRegistry){
        var save = new CampaignSaveData{
            money = state.Money.Value,
            members = new MembersSaveData{
                activeMembers = state.Members.ActiveMembers.Select(m => m.ToSaveData(equipmentRegistry, unitRegistry)).ToList(),
                retiredMembers = state.Members.RetiredMembers.Select(m => m.ToSaveData(equipmentRegistry, unitRegistry)).ToList(),
                deadMembers = state.Members.DeadMembers.Select(m => m.ToSaveData(equipmentRegistry, unitRegistry)).ToList()
            },
            equipment = new EquipmentSaveData{
                equipmentGuids = state.Equipment.Equipment.Select(equipmentRegistry.GetGuid).ToList()
            }
        };
        var activeMembersList = state.Members.ActiveMembers.ToList();
        save.squad.activeMemberIndices = state.Squad.Members
            .Select(member => activeMembersList.IndexOf(member))
            .Where(index => index >= 0)
            .ToList();
        return save;
    }

    public static CampaignState ToCampaignState(this CampaignSaveData save, EquipmentAssetRegistry equipmentRegistry,
        UnitPrefabAssetRegistry unitRegistry){
        var state = new CampaignState();

        state.Money.SetValue(save.money);

        foreach (var guid in save.equipment.equipmentGuids) state.Equipment.AddEquipment(equipmentRegistry.GetAsset(guid));

        var activeMembers = save.members.activeMembers.Select(m => m.ToSquadMember(equipmentRegistry, unitRegistry)).ToList();
        foreach (var member in activeMembers) state.Members.AddMember(member);
        foreach (var memberSave in save.members.retiredMembers) state.Members.RetireMember(memberSave.ToSquadMember(equipmentRegistry, unitRegistry));
        foreach (var memberSave in save.members.deadMembers) state.Members.DieMember(memberSave.ToSquadMember(equipmentRegistry, unitRegistry));

        foreach (var index in save.squad.activeMemberIndices) state.Squad.AddMember(activeMembers[index]);

        return state;
    }
}
