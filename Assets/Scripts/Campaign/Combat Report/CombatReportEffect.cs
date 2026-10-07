using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatReportEffect : MonoBehaviour{
    // [BoxGroup("References")] [Required] [SerializeField] CampaignStateHolder campaignStateHolder; 
    [BoxGroup("References")][Required][SerializeField] CampaignManager campaignManager;
    
    [SerializeField] [Required] CombatReportData combatReportData;
    // [SerializeField] RecruitIntakeData recruitIntakeData; // Optional reference, but if assigned it triggers intake

    protected void Awake(){
        Report();
    }

    void Report(){
        foreach (var member in campaignManager.State.Squad.Members.ToList()){
            if (member.alive){
                continue;
            }
            Debug.Log($"{member.Name} is dead.");
            campaignManager.State.DieMember(member);
        }
        int totalUpkeep = 0; // TODO Remove
        foreach (var rosterMember in campaignManager.State.Members.ActiveMembers){
            totalUpkeep += rosterMember.UpkeepCost;
        }
        
        campaignManager.IncrementStep();
        
        var lastCombatReport = combatReportData.LastCombatReport;
        int netPayout = lastCombatReport.payout - totalUpkeep;
        
        Debug.Log($"Mission Payout: {lastCombatReport.payout}, Total Upkeep: {totalUpkeep}, Net: {netPayout}");
        campaignManager.State.Money.ChangeValue(netPayout);

        //TODO temporary
        if (campaignManager.State.Progression.State == CampaignProgressionState.Completed){
            SceneManager.LoadScene("End Campaign");
        }
    }
}