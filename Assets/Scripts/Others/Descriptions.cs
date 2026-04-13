using UnityEngine;

public static class Descriptions{
    public static string GetDescription(this UnitAction unitAction){
        string description = unitAction.Description;
        description += $"Cost: {unitAction.GetCost()}";
        if (unitAction is TargetedUnitAction targetedUnitAction){
            description += $" Range: {targetedUnitAction.Range}";
        } 
        if (unitAction is AttackAction attackAction){
            description += $" Damage: {attackAction.Damage}";
        }
        if (unitAction is SpawnAction spawnAction){
            // actionDescription += $" Spawned Unit: {spawnAction.PrefabToSpawn.GetComponent<ICombatObject>().GetDescription()}";
        }
        return description;
    }

    public static string GetDescription(this ICombatObject combatObject){
        string description = "";
        var healthComponent = combatObject.GetCombatComponent<HealthComponent>();
        if (healthComponent != null){
            description += $" Health: {healthComponent.Health}/{healthComponent.MaxHealth}";
        }
        var ammoComponent = combatObject.GetCombatComponent<AmmoComponent>();
        if (ammoComponent != null){
            description += $" Ammo: {ammoComponent.CurrentLoadedAmmo}/{ammoComponent.MagazineSize}, {ammoComponent.Magazines} magazines";
        }
        return description.TrimStart();
    }
}