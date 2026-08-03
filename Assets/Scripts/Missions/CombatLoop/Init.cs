using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour{
    // [SerializeField] SquadData squadData;
    [BoxGroup("References")] [Required] [SerializeField] SaveSystem saveSystem;

    [SerializeField] [SceneObjectsOnly] protected string sceneName;

    protected void Awake(){
        saveSystem.Load();
        SceneManager.LoadScene(sceneName);
    }
}