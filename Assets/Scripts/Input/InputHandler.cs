using Nrjwolf.Tools.AttachAttributes;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour{
    [GetComponent] [SerializeField] PlayerInput playerInput;

    [BoxGroup("References")] [Required] [SerializeField] PlayerTurnUI playerTurnUI;
    [BoxGroup("References")] [Required] [SerializeField] CameraMovement cameraMovement;

    protected void Awake(){
        playerInput.onActionTriggered += OnActionTriggered;
    }

    void OnActionTriggered(InputAction.CallbackContext obj){
        if (!obj.performed){
            return;
        }
        if (obj.action.actionMap.name != "Player"){
            return;
        }
        if (obj.action.name.StartsWith("Slot")){
            var numberStr = obj.action.name.Replace("Slot", "");
            if (int.TryParse(numberStr, out var slotNumber)){
                OnActionSlotPerformed(slotNumber);
            }
            return;
        }
        switch (obj.action.name){
            case "Select":
                OnSelectPerformed();
                break;
            case "Move":
                OnMovePerformed(obj);
                break;
            case "Confirm":
                OnConfirmPerformed();
                break;
            default:
                Debug.LogWarning("Unhandled action: " + obj.action.name);
                break;
        }
    }

    void OnActionSlotPerformed(int slot){
        slot -= 1; // Convert to 0-based index
        playerTurnUI.OnSlotSelected(slot);
    }

    void OnConfirmPerformed(){
        playerTurnUI.OnConfirm();
    }

    void OnMovePerformed(InputAction.CallbackContext callbackContext){
        var value = callbackContext.ReadValue<Vector2>();
        cameraMovement.SetMovementInput(value);
    }

    bool _selectTriggeredThisFrame;

    public void OnSelectPerformed(){
        _selectTriggeredThisFrame = true;
    }

    protected void Update(){
        if (_selectTriggeredThisFrame){
            _selectTriggeredThisFrame = false;
            if (EventSystem.current.IsPointerOverGameObject()){
                return;
            }
            playerTurnUI.OnSelect();
        }
    }
}