using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] CampaignStateHolder campaignStateHolder;
    [BoxGroup("References")] [Required] [SerializeField] SaveSystem saveSystem;

    [SerializeField] [SceneObjectsOnly] protected string sceneName;

    protected void Awake(){
        var campaignState = saveSystem.Load();
        campaignStateHolder.SetCampaignState(campaignState);
        SceneManager.LoadScene(sceneName);
    }
}