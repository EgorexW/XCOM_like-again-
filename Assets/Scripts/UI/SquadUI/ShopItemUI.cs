using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopItemUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] Button button;
    [BoxGroup("References")] [Required] [SerializeField] Image singleIcon;
    [BoxGroup("References")] [Required] [DisableInPlayMode][SerializeField] Transform multipleIconsParent;
    [BoxGroup("References")] [Required] [SerializeField] TextMeshProUGUI priceText;

    ShopItem shopItem;

    [FoldoutGroup("Events")] public UnityEvent<ShopItem> onClicked;
    List<Image> multipleIcons;

    protected void Awake(){
        multipleIcons = new List<Image>(multipleIconsParent.GetComponentsInChildren<Image>());
        button.onClick.AddListener(OnButtonClicked);
    }

    public void Show(ShopItem shopItemTmp){
        this.shopItem = shopItemTmp;
        base.Show();
        if (shopItem.items.Count == 1){
            singleIcon.gameObject.SetActive(true);
            multipleIcons.ForEach(icon => icon.gameObject.SetActive(false));
            singleIcon.sprite = shopItem.items[0].Icon;
        } else {
            singleIcon.gameObject.SetActive(false);
            if (shopItem.items.Count > multipleIcons.Count){
                Debug.LogWarning($"Bundle size ({shopItem.items.Count}) exceeds available UI slots ({multipleIcons.Count}) for {shopItem}!");
            }
            for (var i = 0; i < multipleIcons.Count; i++){
                if (i < shopItem.items.Count){
                    multipleIcons[i].gameObject.SetActive(true);
                    multipleIcons[i].sprite = shopItem.items[i].Icon;
                } else {
                    multipleIcons[i].gameObject.SetActive(false);
                }
            }
        }
        priceText.text = $"{shopItem.price}$";
    }

    void OnButtonClicked(){
        onClicked.Invoke(shopItem);
    }
}
