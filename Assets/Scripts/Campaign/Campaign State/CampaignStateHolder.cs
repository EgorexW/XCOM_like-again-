
    using UnityEngine;

    [CreateAssetMenu(menuName = StringKeys.AssetMenuCampaignStateBasePath)]
    
    // TODO idealy only accessed by CapaignManager
    public class CampaignStateHolder : ScriptableObject{
        [SerializeField] CampaignState campaignState;
        
        public CampaignState State => campaignState;
        
        public void SetCampaignState(CampaignState newCampaignState){
            campaignState = newCampaignState;
        }
    } 