using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Init : MonoBehaviour{
    [SerializeField] SquadData squadData;
    [SerializeField] SquadData initSquadData;
    [FormerlySerializedAs("playerResourcesData")] [SerializeField] ResourcesData resourcesData;
    [FormerlySerializedAs("initPlayerResourcesData")] [SerializeField] ResourcesData initResourcesData;

    [SerializeField] [SceneObjectsOnly] protected string sceneName;

    protected void Awake(){
        squadData.DeepCopy(initSquadData);
        resourcesData.DeepCopy(initResourcesData);
        SceneManager.LoadScene(sceneName);
    }
}