using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EquipmentTypeUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] Button button;
    [BoxGroup("References")] [Required] [SerializeField] Image icon;

    Equipment equipment;

    [FoldoutGroup("Events")] public UnityEvent<Equipment> onClicked;

    void Awake(){
        button.onClick.AddListener(OnButtonClicked);
    }

    public void Show(Equipment equipmentTmp){
        equipment = equipmentTmp;
        base.Show();
        icon.sprite = equipment.Icon;
    }

    void OnButtonClicked(){
        onClicked.Invoke(equipment);
    }
}