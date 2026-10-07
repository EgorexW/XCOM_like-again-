using Sirenix.OdinInspector;
using UnityEngine;

class CampaignManager : MonoBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] CampaingSequence campaingSequence;
    [BoxGroup("References")][Required][SerializeField] CampaignStateHolder campaingStateHolder;
    
    public CampaignState State => campaingStateHolder.State;

    public Mission GetCurrentMission(){
        var day = campaingSequence.GetStep(campaingStateHolder.State.Progression.Step);
        return day.Mission;
    }

    public void IncrementStep(){
        State.Progression.IncrementStep();
        var step = State.Progression.Step;
        if (step >= campaingSequence.Lenght){
            State.Progression.SetCampaignResult(new CampaignResult{
                isVictory = true,
                reason = "All missions completed."
            });
        }
    }
}