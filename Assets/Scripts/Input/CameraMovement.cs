using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

class CameraMovement : MonoBehaviour{
    public float speed = 5f;

    Vector2 movement = Vector2.zero;

    [FoldoutGroup("Events")] public UnityEvent onMove;

    protected void Update(){
        var move = movement * (speed * Time.deltaTime);
        if (move.sqrMagnitude > 0){
            onMove.Invoke();
        }
        transform.Translate(move);
    }

    public void SetMovementInput(Vector2 inputVector){
        movement = inputVector.normalized;
    }
}