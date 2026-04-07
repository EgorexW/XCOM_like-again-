using UnityEngine;
using UnityEngine.Events;

public class EveryXSeconds : MonoBehaviour{
    [SerializeField] float x = 3;

    float lastTriggerTime;

    [SerializeField] UnityEvent onTrigger;

    protected void Awake(){
        lastTriggerTime = Time.time;
    }

    protected void Update(){
        while (Time.time - lastTriggerTime >= x){
            lastTriggerTime += x;
            onTrigger.Invoke();
        }
    }
}