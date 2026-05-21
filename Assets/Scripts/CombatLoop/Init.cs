using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Init : MonoBehaviour{
    [SerializeField] SquadData squadData;
    [FormerlySerializedAs("playerResourcesData")] [SerializeField] ResourcesData resourcesData;
    [FormerlySerializedAs("initPlayerResourcesData")] [SerializeField] ResourcesData initResourcesData;

    [SerializeField] [SceneObjectsOnly] protected string sceneName;

    [Title("Initial Squad Generation")]
    [SerializeField] RecruitIntakeData recruitIntakeData;
    [SerializeField] int startingSquadSize = 5;

    protected void Awake(){
        squadData.Clear();
        resourcesData.DeepCopy(initResourcesData);
        
        if (recruitIntakeData != null && startingSquadSize > 0){
            recruitIntakeData.GenerateRecruits(resourcesData, startingSquadSize);
        }

        SceneManager.LoadScene(sceneName);
    }
}