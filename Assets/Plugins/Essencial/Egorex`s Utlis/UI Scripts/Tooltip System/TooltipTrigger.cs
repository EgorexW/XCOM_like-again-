using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{
    [SerializeField] Message message;

    [SerializeField] float delay = 0.5f;

    LTDescr delayCall = new();

    protected void Reset(){
        message.header = gameObject.name;
    }

    protected void OnDisable(){
        Deactivate();
    }

    protected void OnMouseEnter(){
        Activate();
    }

    protected void OnMouseExit(){
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

    public void SetMessage(Message message, bool enable = true){
        this.message = message;
        if (enable){
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