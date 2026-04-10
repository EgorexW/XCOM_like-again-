using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

class ActionTileUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] TextMeshProUGUI actionName;
    [BoxGroup("References")] [Required] [SerializeField] Button selectButton;
    [BoxGroup("References")] [Required] [SerializeField] TextMeshProUGUI uses;
    [BoxGroup("References")][Required][SerializeField] Image supressedIcon;

    UnitAction action;
    UnityAction<UnitAction> onSelect;

    protected void Awake(){
        selectButton.onClick.AddListener(OnSelect);
    }

    void OnSelect(){
        onSelect?.Invoke(action);
    }

    public void SetAction(UnitAction action, UnityAction<UnitAction> onSelect){
        actionName.SetText(action.name);
        this.action = action;
        this.onSelect = onSelect;
        var validation = action.CanExecute();
        supressedIcon.gameObject.SetActive(false);
        selectButton.interactable = true;
        switch (validation){
            case UnitActionValidation.SupressedByStatus:
                selectButton.interactable = false;
                supressedIcon.gameObject.SetActive(true);
                break;
            case UnitActionValidation.NotEnoughActionPoints:
                selectButton.interactable = false;
                break;
        }
        if (action.LimitedUses){
            uses.SetText($"x{action.UsesLeft}");
        }
        else{
            uses.SetText("");
        }
    }
}