using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IIgnorePointerOverUI{
    [SerializeField] Message message;

    [SerializeField] float delay = 0.5f;

    LTDescr delayCall = new();

    protected void Reset(){
        message.header = gameObject.name;
    }

    protected void OnDisable(){
        Deactivate();
    }

    public void OnPointerEnter(PointerEventData eventData){
        Activate();
    }

    public void OnPointerExit(PointerEventData eventData){
        Deactivate();
    }

    void Activate(){
        if (!enabled){
            return;
        }
        delayCall = LeanTween.delayedCall(delay, () => { TooltipSystem.Show(message); });
    }

    void Deactivate(){
        LeanTween.cancel(delayCall.uniqueId);
        TooltipSystem.Hide();
    }

    public void SetMessage(Message messageTmp){
        message = messageTmp;
        if (message.header.IsNullOrWhitespace()){
            Disable();
        }
        else{
            Enable();
        }
    }

    public void Disable(){
        enabled = false;
    }

    public void Enable(){
        enabled = true;
    }
}