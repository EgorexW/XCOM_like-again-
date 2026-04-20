using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BasicLevel : Level{
    [SerializeField] List<Transform> spawnPoints;
    
    List<ITurnTaker> turnTakers;
    List<Team> teams;

    public override List<Vector2> GetSpawnPoints(int team){
        var teamSpawn = spawnPoints[team];
        var poses = new List<Vector2>();
        for (int i = 0; i < teamSpawn.childCount; i++){
            poses.Add(teamSpawn.GetChild(i).position);
        }
        return poses;
    }
}