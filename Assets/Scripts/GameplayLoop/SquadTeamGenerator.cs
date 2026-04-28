using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SquadTeamGenerator : TeamGenerator{
    [BoxGroup("References")][Required][SerializeField] SquadData squadData;
    
    [SerializeField] UnitsTurnTaker turnTaker;
    
    public override Team GenerateTeam(){
        var combatObjects = new List<ICombatObject>();
        foreach (var member in squadData.SquadMembers){
            combatObjects.Add(AddTeamMember(member));
        }
        return new Team(combatObjects);
    }
    
    ICombatObject AddTeamMember(SquadMember member){
        var gameObj = Instantiate(member.combatPrefab, transform);
        var combatObject = gameObj.GetComponent<ICombatObject>();
        if (combatObject == null){
            Debug.LogError(
                $"The prefab {gameObj.name} does not have a component that implements ICombatObject. Skipping this prefab.");
            Destroy(gameObj);
            return null;
        }
        var squadMemberComponent = combatObject.GetCombatComponent<SquadMemberComponent>();
        if (squadMemberComponent == null){
            Debug.LogError(
                $"The prefab {gameObj.name} does not have a SquadMemberComponent. This component is required for squad members. Skipping this prefab.");
            Destroy(gameObj);
            return null;
        }
        squadMemberComponent.SetMember(member);
        if (turnTaker != null){
            if (combatObject is CombatUnit combatUnit){
                turnTaker.AddUnit(combatUnit);
            }
        }
        return combatObject;
    }
}