public static class Descriptions{
    public static string GetDescription(this UnitAction unitAction){
        var description = unitAction.ActionInfo.Description.Trim();
        description += "\n";
        description += $"Cost: {unitAction.GetCost()}";
        if (unitAction is TargetedUnitAction targetedUnitAction){
            description += $" Range: {targetedUnitAction.Range}";
        }
        if (unitAction is ShootAction attackAction){
            if (attackAction.DamageValue > 0){
                description += $" Damage: {attackAction.DamageValue}";
            }
            if (attackAction.AmmoCost > 0){
                description += $" Ammo: {attackAction.AmmoCost}";
            }
        }
        if (unitAction is SpawnAction spawnAction){
            // actionDescription += $" Spawned Unit: {spawnAction.PrefabToSpawn.GetComponent<ICombatObject>().GetDescription()}";
        }
        return description;
    }

    public static string GetDescription(this ICombatObject combatObject){
        var description = "";
        var healthComponent = combatObject.GetCombatComponent<HealthComponent>();
        if (healthComponent != null){
            description += $" Health: {healthComponent.Health}/{healthComponent.MaxHealth}";
        }
        var ammoComponent = combatObject.GetCombatComponent<AmmoComponent>();
        if (ammoComponent != null){
            description +=
                $" Ammo: {ammoComponent.CurrentLoadedAmmo}/{ammoComponent.MagazineSize}, {ammoComponent.Magazines} magazines";
        }
        return description.TrimStart();
    }

    public static string GetDescription(this PayoutObjective objective){
        var description = objective.Description.Trim();
        description += $": {objective.Payout}";
        return description.Trim();
    }
}