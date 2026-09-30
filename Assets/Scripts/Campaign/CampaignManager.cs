using Sirenix.OdinInspector;
using UnityEngine;

class CampaignManager : MonoBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] CampaingSequence campaingSequence;
    [BoxGroup("References")][Required][SerializeField] CampaignStateHolder campaingStateHolder;

    public Mission GetCurrentMission(){
        var day = campaingSequence.GetStep(campaingStateHolder.State.Progression.Step);
        return day.Mission;
    }
}