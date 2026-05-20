using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

class CameraMovement : MonoBehaviour{
    [FormerlySerializedAs("speed")] [SerializeField] float inputMovementSpeed = 5f;
    [SerializeField] float moveToTime = 0.5f;

    Vector2 movement = Vector2.zero;

    [FoldoutGroup("Events")] public UnityEvent onMove;

    protected void Update(){
        Vector2 move = movement * (inputMovementSpeed * Time.deltaTime);
        
        if (move.sqrMagnitude > 0){
            onMove.Invoke();
            transform.Translate(move);
        }
    }

    public void SetMovementInput(Vector2 inputVector){
        movement = inputVector.normalized;
        if (movement != Vector2.zero){
            LeanTween.cancel(gameObject);
        }
    }

    public void MoveTo(Vector2 position){
        if (movement != Vector2.zero){
            return;
        }
        
        float distance = Vector2.Distance(transform.position, position);
        if (distance <= 0.001f) return;
        
        LeanTween.cancel(gameObject);
        LeanTween.move(gameObject, new Vector3(position.x, position.y, transform.position.z), moveToTime)
            .setEase(LeanTweenType.easeInOutSine)
            .setOnUpdate((float val) => {
                onMove.Invoke();
            });
    }
}