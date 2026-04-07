using UnityEngine;

public class EditorOnlyGameObject : MonoBehaviour{
    readonly bool destroy = false;

    protected void Awake(){
        if (destroy){
            Destroy(gameObject);
            return;
        }
        gameObject.SetActive(false);
    }
}