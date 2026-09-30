using Sirenix.OdinInspector;
using UnityEngine;

class CampaignManager : MonoBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] CampaingSequence campaingSequence;
    [BoxGroup("References")][Required][SerializeField] CampaignStateHolder campaingStateHolder;

    public Mission GetCurrentMission(){
        throw new System.NotImplementedException();
    }
}