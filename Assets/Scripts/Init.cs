using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour
{
    [SerializeField] SquadData squadData;
    [SerializeField] SquadData initSquadData;
    
    [SerializeField] [SceneObjectsOnly] protected string sceneName; 

    void Awake(){
        squadData.Copy(initSquadData);
        SceneManager.LoadScene(sceneName);
    }
}
