using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class OnKeyDown : MonoBehaviour{
    public Key actionKey = Key.Escape;
    public Optional<Key> modifierKey = new(Key.LeftCtrl, false);
    public UnityEvent onKeyDown = new();


    protected void Update(){
        if (Keyboard.current == null){
            return;
        }

        var actionPressed = Keyboard.current[actionKey].wasPressedThisFrame;
        var modifierPressed = !modifierKey.Enabled || Keyboard.current[modifierKey.Value].isPressed;

        if (actionPressed && modifierPressed){
            onKeyDown.Invoke();
        }
    }
}