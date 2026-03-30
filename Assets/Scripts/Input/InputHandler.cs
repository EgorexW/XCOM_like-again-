using Nrjwolf.Tools.AttachAttributes;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour
{
    [GetComponent] [SerializeField] PlayerInput playerInput;
    
    [BoxGroup("References")][Required][SerializeField] PlayerTurnUI playerTurnUI;

    void Awake()
    {
        playerInput.onActionTriggered += OnActionTriggered;
    }

    void OnActionTriggered(InputAction.CallbackContext obj)
    {
        if (!obj.performed){
            return;
        }
        if (obj.action.actionMap.name != "Player"){
            return;
        }
        switch (obj.action.name){
            case "Select":
                OnSelectPerformed();
                break;
            default:
                Debug.LogWarning("Unhandled action: " + obj.action.name);
                break;
        }
    }

    void OnSelectPerformed(){
        playerTurnUI.OnSelect();
    }
}