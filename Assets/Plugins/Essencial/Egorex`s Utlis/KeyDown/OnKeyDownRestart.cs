using UnityEngine;
using UnityEngine.SceneManagement;

public class OnKeyDownRestart : OnKeyDownTrigger{
    [SerializeField] protected bool async;

    protected override void Trigger(){
        var sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        if (async){
            SceneManager.LoadSceneAsync(sceneBuildIndex);
        }
        else{
            SceneManager.LoadScene(sceneBuildIndex);
        }
    }
}