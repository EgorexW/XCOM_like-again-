using UnityEngine;
using UnityEngine.Events;

public class EveryXSeconds : MonoBehaviour
{
    [SerializeField] float x = 3;

    float lastTriggerTime;

    [SerializeField] UnityEvent onTrigger;

    void Awake()
    {
        lastTriggerTime = Time.time;
    }

    void Update()
    {
        while (Time.time - lastTriggerTime >= x){
            lastTriggerTime += x;
            onTrigger.Invoke();
        }
    }
}