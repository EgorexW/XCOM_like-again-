using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class CombatReportEffect : MonoBehaviour{
    [SerializeField] [Required] SquadData squadData;
    [SerializeField] [Required] ResourcesData resourcesData;
    [SerializeField] [Required] CombatReportData combatReportData;
    [SerializeField] RecruitIntakeData recruitIntakeData; // Optional reference, but if assigned it triggers intake

    protected void Awake(){
        Report();
    }

    void Report(){
        foreach (var member in squadData.SquadMembers.ToList()){
            if (member.alive){
                continue;
            }
            Debug.Log($"{member.Name} is dead.");
            squadData.RemoveMember(member);
            resourcesData.RemoveMember(member);
        }
        int totalUpkeep = 0;
        foreach (var rosterMember in resourcesData.Members){
            totalUpkeep += rosterMember.UpkeepCost;
        }

        var lastCombatReport = combatReportData.LastCombatReport;
        int netPayout = lastCombatReport.payout - totalUpkeep;
        
        Debug.Log($"Mission Payout: {lastCombatReport.payout}, Total Upkeep: {totalUpkeep}, Net: {netPayout}");
        resourcesData.ChangeMoney(netPayout);

        if (recruitIntakeData != null){
            recruitIntakeData.OnMissionCompleted(resourcesData);
        }
    }
}