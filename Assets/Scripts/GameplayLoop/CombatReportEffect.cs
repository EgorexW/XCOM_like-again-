using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class CombatReportEffect : MonoBehaviour
{
    [SerializeField][Required] SquadData squadData;
    [SerializeField][Required] ResourcesData resourcesData;
    [SerializeField][Required] CombatReportData combatReportData;

    void Awake(){
        Report();
    }

    void Report(){
        foreach (var member in squadData.SquadMembers.ToList()){
            if (member.alive){
                continue;
            }
            Debug.Log($"{member.Name} is dead.");
            squadData.RemoveMember(member);
        }
        var lastCombatReport = combatReportData.LastCombatReport;
        resourcesData.ChangeMoney(lastCombatReport.payout);
    }
}