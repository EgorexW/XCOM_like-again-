using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class CombatReportEffect : MonoBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] CampaignStateHolder campaignStateHolder; 
    
    [SerializeField] [Required] CombatReportData combatReportData;
    // [SerializeField] RecruitIntakeData recruitIntakeData; // Optional reference, but if assigned it triggers intake

    protected void Awake(){
        Report();
    }

    void Report(){
        foreach (var member in campaignStateHolder.State.Squad.Members.ToList()){
            if (member.alive){
                continue;
            }
            Debug.Log($"{member.Name} is dead.");
            campaignStateHolder.State.DieMember(member);
        }
        int totalUpkeep = 0;
        foreach (var rosterMember in campaignStateHolder.State.Members.ActiveMembers){
            totalUpkeep += rosterMember.UpkeepCost;
        }

        var lastCombatReport = combatReportData.LastCombatReport;
        int netPayout = lastCombatReport.payout - totalUpkeep;
        
        Debug.Log($"Mission Payout: {lastCombatReport.payout}, Total Upkeep: {totalUpkeep}, Net: {netPayout}");
        campaignStateHolder.State.Money.ChangeValue(netPayout);

        // if (recruitIntakeData != null){
        //     recruitIntakeData.OnMissionCompleted(CampaignState);
        // }
    }
}