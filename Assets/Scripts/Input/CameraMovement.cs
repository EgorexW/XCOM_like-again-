using UnityEngine;

class CameraMovement : MonoBehaviour{
    public float speed = 5f;

    Vector2 movement = Vector2.zero;

    protected void Update()
    {
        var move = movement * (speed * Time.deltaTime);
        transform.Translate(move);
    }

    public void SetMovementInput(Vector2 inputVector)
    {
        movement = inputVector.normalized;
    }
}