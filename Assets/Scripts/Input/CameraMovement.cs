using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

class CameraMovement : MonoBehaviour{
    [FormerlySerializedAs("speed")] [SerializeField] float inputMovementSpeed = 5f;
    [SerializeField] float moveToMovementSpeed = 15f;

    Vector2 movement = Vector2.zero;
    Vector2? targetPosition;

    [FoldoutGroup("Events")] public UnityEvent onMove;

    protected void Update(){
        Vector2 move;
        if (targetPosition.HasValue){
            var nextStep = Vector2.MoveTowards(transform.position, targetPosition.Value, moveToMovementSpeed * Time.deltaTime);
            move = nextStep - (Vector2)transform.position;
        }
        else{
            move = movement * (inputMovementSpeed * Time.deltaTime);
        }
        if (move.sqrMagnitude > 0){
            onMove.Invoke();
        }
        transform.Translate(move);
    }

    public void SetMovementInput(Vector2 inputVector){
        movement = inputVector.normalized;
        if (movement != Vector2.zero){
            targetPosition = null;
        }
    }

    public void MoveTo(Vector2 position){
        targetPosition = position;
        SetMovementInput(Vector2.zero);
    }
}