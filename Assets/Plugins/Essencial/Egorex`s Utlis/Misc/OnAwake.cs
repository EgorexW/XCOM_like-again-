using UnityEngine;
using UnityEngine.Events;

public class OnAwake : MonoBehaviour{
    public UnityEvent onAwake;
    readonly bool onStart = false;

    protected void Awake(){
        if (!onStart){
            onAwake.Invoke();
        }
    }

    protected void Start(){
        if (onStart){
            onAwake.Invoke();
        }
    }
}