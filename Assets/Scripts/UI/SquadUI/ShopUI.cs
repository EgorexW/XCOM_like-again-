using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class ShopUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] ObjectsPool shopItemsPool;
    [BoxGroup("References")] [Required] [SerializeField] ShopLogic shopLogic;
    
    protected void Awake(){
        shopItemsPool.onCreateObject.AddListener(OnCreateShopItemUI);
    }

    protected void Start(){
        UpdateShopItems();
    }

    void OnCreateShopItemUI(GameObject arg0){
        var shopItemUI = arg0.GetComponent<ShopItemUI>();
        shopItemUI.onClicked.AddListener(OnShopItemClicked);
    }

    void OnShopItemClicked(ShopItem shopItem){
        shopLogic.PurchaseItem(shopItem);
    }

    void UpdateShopItems(){
        var shop = shopLogic.Shop;
        var count = shop.Count;
        shopItemsPool.SetCount(count);
        for (var i = 0; i < count; i++){
            var item = shop.ShopItems[i];
            var obj = shopItemsPool.GetActiveObject(i);
            var shopItemUI = obj.GetComponent<ShopItemUI>();
            shopItemUI.Show(item);
        }
    }

    public override void Show(){
        base.Show();
        UpdateShopItems();
    }
}
