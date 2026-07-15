using System.Linq;

public static class SaveDataExtensions{
    public static SquadMemberSaveData ToSaveData(this SquadMember member, EquipmentAssetRegistry equipmentRegistry,
        UnitPrefabAssetRegistry unitRegistry){
        return new SquadMemberSaveData{
            name = member.Name,
            unitGuid = unitRegistry.GetGuid(member.CombatPrefab),
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
        var member = new SquadMember(save.name, unitRegistry.GetAsset(save.unitGuid), equipment, save.upkeepCost,
            save.retirementThreshold){
            alive = save.alive
        };
        member.SetMissionsCompleted(save.missionsCompleted);
        return member;
    }

    public static ResourcesSaveData ToSaveData(this ResourcesData resourcesData, EquipmentAssetRegistry equipmentRegistry,
        UnitPrefabAssetRegistry unitRegistry){
        return new ResourcesSaveData{
            money = resourcesData.Money,
            members = resourcesData.Members.Select(m => m.ToSaveData(equipmentRegistry, unitRegistry)).ToList(),
            retiredMembers = resourcesData.RetiredMembers.Select(m => m.ToSaveData(equipmentRegistry, unitRegistry)).ToList(),
            equipmentGuids = resourcesData.Equipment.Select(equipmentRegistry.GetGuid).ToList()
        };
    }

    public static void LoadFrom(this ResourcesData resourcesData, ResourcesSaveData save,
        EquipmentAssetRegistry equipmentRegistry, UnitPrefabAssetRegistry unitRegistry){
        resourcesData.Clear();
        resourcesData.SetMoney(save.money);
        foreach (var memberSave in save.members) resourcesData.AddMember(memberSave.ToSquadMember(equipmentRegistry, unitRegistry));
        foreach (var memberSave in save.retiredMembers) resourcesData.AddRetiredMember(memberSave.ToSquadMember(equipmentRegistry, unitRegistry));
        foreach (var guid in save.equipmentGuids) resourcesData.AddEquipment(equipmentRegistry.GetAsset(guid));
    }
}
