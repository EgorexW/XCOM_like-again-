using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SquadSelectionEmbark : MonoBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] CampaignManager campaignManager;
    [SerializeField] string sceneName;
    
    public void TryEmbark(){
        if (EmbarkValidation() != EmbrakValidation.Valid){
            return;
        }
        SceneManager.LoadScene(sceneName);
    }

    public EmbrakValidation EmbarkValidation(){
        EmbrakValidation validation = 0;
        var squadSize = campaignManager.State.Squad.Members.Count;
        if (squadSize < 1){
            validation |= EmbrakValidation.NoSquadMembers;
        }
        return validation;
    }
}

[System.Flags]
public enum EmbrakValidation{
    Valid = 0,
    NoSquadMembers = 1 << 0,
}
