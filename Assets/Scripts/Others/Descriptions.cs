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

    #region Combat Object
    
    public static string GetDescription(this ICombatObject combatObject){
        var description = "";
        description += combatObject.GetHealthDescription();
        description += combatObject.GetAmmoDescription();
        description += combatObject.GetActionPointsDescription();
        description += combatObject.GetActionsDescription();
        description += combatObject.GetStatusesDescription();
        description += combatObject.GetSuspectStateDescription();
        description += combatObject.GetTeamDescription();
        description += combatObject.GetDelayedEffectDescription();
        return description.TrimStart();
    }
    public static Message GetMessage(this ICombatObject combatObject){
        var message = new Message(){
            header = combatObject.Name,
            description = combatObject.GetDescription(),
        };
        return message;
    }
    static string GetHealthDescription(this ICombatObject combatObject){
        var health = combatObject.GetCombatComponent<HealthComponent>();
        if (health == null) return "";
        var bar = health.IsDead ? "DEAD" : $"{health.Health}/{health.MaxHealth}";
        return $"Health: {bar}\n";
    }

    static string GetAmmoDescription(this ICombatObject combatObject){
        var ammo = combatObject.GetCombatComponent<AmmoComponent>();
        if (ammo == null) return "";
        var loadedLabel = ammo.IsEmpty ? "EMPTY" : $"{ammo.CurrentLoadedAmmo}/{ammo.MagazineSize}";
        return $"Ammo: {loadedLabel}, {ammo.Magazines} mag(s) remaining\n";
    }

    static string GetActionPointsDescription(this ICombatObject combatObject){
        if (combatObject is not Unit unit) return "";
        return $"Action Points: {unit.ActionPoints}\n";
    }

    static string GetActionsDescription(this ICombatObject combatObject){
        if (combatObject is not Unit unit) return "";
        if (unit.UnitActions.Count == 0) return "";
        var names = new System.Text.StringBuilder();
        foreach (var action in unit.UnitActions){
            if (names.Length > 0) names.Append(", ");
            names.Append(action.ActionInfo.Name);
        }
        return $"Actions: {names}\n";
    }

    static string GetStatusesDescription(this ICombatObject combatObject){
        if (combatObject is not Unit unit) return "";
        if (unit.ActiveStatuses.Count == 0) return "";
        var names = new System.Text.StringBuilder();
        foreach (var status in unit.ActiveStatuses){
            if (names.Length > 0) names.Append(", ");
            names.Append(status.Info.Name);
        }
        return $"Statuses: {names}\n";
    }

    static string GetSuspectStateDescription(this ICombatObject combatObject){
        var suspect = combatObject.GetCombatComponent<SuspectComponent>();
        if (suspect == null) return "";
        return $"Suspect: {suspect.SuspectState}\n";
    }

    static string GetTeamDescription(this ICombatObject combatObject){
        var teamsSystem = combatObject.CombatSystem?.TeamsSystem;
        if (teamsSystem == null) return "";
        Team team = null;
        try{ team = teamsSystem.GetTeam(combatObject); } catch{ return ""; }
        if (team == null) return "";
        return $"Team: {team.Flags}\n";
    }

    static string GetDelayedEffectDescription(this ICombatObject combatObject){
        var delayed = combatObject.GetCombatComponent<DelayedEffectComponent>();
        if (delayed == null) return "";
        return $"Activates in: {delayed.DurationLeft} turn(s)\n";
    }


    
    #endregion

    public static string GetDescription(this PayoutObjective objective){
        var description = objective.Description.Trim();
        description += $": {objective.Payout}";
        return description.Trim();
    }
}