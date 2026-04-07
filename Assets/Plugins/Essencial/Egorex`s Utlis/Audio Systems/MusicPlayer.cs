using UnityEngine;

public class MusicPlayer : MonoBehaviour{
    static MusicPlayer i;

    protected void Awake(){
        if (i != null){
            Destroy(gameObject);
        }
        else{
            i = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    protected void Update(){
        AudioManager.i.Play("Music");
    }
}