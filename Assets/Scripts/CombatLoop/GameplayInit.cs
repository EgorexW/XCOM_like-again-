using Sirenix.OdinInspector;
using UnityEngine;

public class GameplayInit : MonoBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] CombatInit combatInit;
    [BoxGroup("References")] [Required] [SerializeField] GameRunState gameRunState;
    [BoxGroup("References")] [Required] [SerializeField] PayoutManager payoutManager;

    protected void Start(){
        if (gameRunState.selectedMissionType == null) {
            Debug.LogError("No selectedMissionType provided to GameplayInit!");
            return;
        }

        var missionType = gameRunState.selectedMissionType;

        var missionInit = Instantiate(missionType.missionInitPrefab);
        var content = new CombatContent();
        
        missionInit.InitMission(content, payoutManager);
        
        combatInit.InitCombatSystem(content);
    }
}