using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopItemUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] Button button;
    [BoxGroup("References")] [Required] [SerializeField] Image icon;
    [BoxGroup("References")] [Required] [SerializeField] TextMeshProUGUI priceText;

    Equipment equipment;

    [FoldoutGroup("Events")] public UnityEvent<Equipment> onClicked;

    void Awake(){
        button.onClick.AddListener(OnButtonClicked);
    }

    public void Show(Equipment equipmentTmp){
        equipment = equipmentTmp;
        base.Show();
        icon.sprite = equipment.Icon;
        priceText.text = $"{equipment.StandardPrice}$";
    }

    void OnButtonClicked(){
        onClicked.Invoke(equipment);
    }
}
