using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

class CombatContentInit : MonoBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] List<TeamGenerator> teamGenerators;
    [BoxGroup("References")] [Required] [SerializeField] List<TurnTaker> turnTakers;

    [BoxGroup("References")] [Required] [SerializeField] List<GameObject> levelPrefabs;

    public CombatContent Init(){
        var content = new CombatContent();
        foreach (var teamGenerator in teamGenerators) content.teams.Add(teamGenerator.GenerateTeam());
        content.turnTakers.AddRange(turnTakers);
        content.levelPrefab = levelPrefabs.Random();
        return content;
    }
}