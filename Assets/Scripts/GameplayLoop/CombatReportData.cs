using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = StringKeys.AssetMenuCombatReportDataBasePath)]
class CombatReportData : ScriptableObject{
    List<CombatReport> combatReports  = new();
    
    public CombatReport LastCombatReport{ get; private set; }

    public void AddCombatReport(CombatReport combatReport){
        LastCombatReport = combatReport;
        combatReports.Add(combatReport);
    }
}


class CombatReport{
    public int payout;
}
