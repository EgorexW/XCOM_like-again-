using Sirenix.OdinInspector;
using UnityEngine;

public class CombatReportCreator : MonoBehaviour{
    [BoxGroup("References")][Required][SerializeField] CombatReportData  combatReportData;
    
    [BoxGroup("References")][Required][SerializeField] PayoutManager payoutManager;

    public void CreateReport(){
        var combatReport = new CombatReport{
            payout = payoutManager.GetPayout()
        };
        combatReportData.AddCombatReport(combatReport);
    }
}