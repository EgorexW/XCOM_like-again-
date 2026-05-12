using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = StringKeys.AssetMenuCombatReportDataBasePath)]
class CombatReportData : ScriptableObject{
    CombatReport lastCombatReport;
    
    List<CombatReport> combatReports  = new();

    public void AddCombatReport(CombatReport combatReport){
        lastCombatReport = combatReport;
        combatReports.Add(combatReport);
    }
}


class CombatReport{
    public int payout;
}
