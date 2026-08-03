using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class GameplayInit : MonoBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] CombatInit combatInit;
    [FormerlySerializedAs("gameRunState")] [BoxGroup("References")] [Required] [SerializeField] CampaingState campaingState;
    [BoxGroup("References")] [Required] [SerializeField] PayoutManager payoutManager;

    protected void Start(){
        if (campaingState.selectedMission == null) {
            Debug.LogError("No selectedMissionType provided to GameplayInit!");
            return;
        }

        var missionType = campaingState.selectedMission;

        var missionInit = Instantiate(missionType.missionInitPrefab);
        var content = new CombatContent();
        
        missionInit.InitMission(content);
        
        foreach(var obj in content.payoutObjectives){
            payoutManager.AddObjective(obj);
        }
        
        combatInit.InitCombatSystem(content);
    }
}