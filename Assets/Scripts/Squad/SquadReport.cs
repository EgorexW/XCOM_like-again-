using System;
using System.Linq;
using UnityEngine;

public class SquadReport : MonoBehaviour
{
    [SerializeField] SquadData squadData;

    void Awake(){
        Report();
    }

    void Report(){
        foreach (var member in squadData.SquadMembers.ToList()){
            if (member.alive){
                continue;
            }
            Debug.Log($"{member.name} is dead.");
            squadData.RemoveMember(member);
        }
    }
}
