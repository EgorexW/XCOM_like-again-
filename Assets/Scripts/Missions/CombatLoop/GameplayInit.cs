using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class GameplayInit : MonoBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] CombatInit combatInit;
    [BoxGroup("References")] [Required] [SerializeField] CampaignManager campaignManager;
    [BoxGroup("References")] [Required] [SerializeField] PayoutManager payoutManager;

    protected void Start(){
        var mission = campaignManager.GetCurrentMission();
        if (mission == null) {
            Debug.LogError("No selectedMissionType provided to GameplayInit!");
            return;
        }

        var missionInit = Instantiate(mission.missionInitPrefab);
        var content = new CombatContent();
        
        missionInit.InitMission(content);
        
        foreach(var obj in content.payoutObjectives){
            payoutManager.AddObjective(obj);
        }
        
        combatInit.InitCombatSystem(content);
    }
}