using UnityEngine;
using UnityEngine.Events;

public class CallbackTriggerCollider : MonoBehaviour{
    public UnityEvent<Collider2D> callback;

    protected void OnTriggerEnter2D(Collider2D other){
        callback.Invoke(other);
    }
}