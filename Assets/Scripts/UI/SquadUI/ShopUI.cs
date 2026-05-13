using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class ShopUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] ObjectsPool shopItemsPool;
    [BoxGroup("References")] [Required] [SerializeField] TextMeshProUGUI moneyText;
    [BoxGroup("References")] [Required] [SerializeField] ShopLogic shopLogic;

    ResourcesData ResourcesData => shopLogic.ResourcesData;
    
    protected void Awake(){
        shopItemsPool.onCreateObject.AddListener(OnCreateShopItemUI);
    }

    protected void Start(){
        ResourcesData.onChanged.AddListener(OnResourcesChanged);
        UpdateMoneyUI();
        UpdateShopItems();
    }

    void OnCreateShopItemUI(GameObject arg0){
        var shopItemUI = arg0.GetComponent<ShopItemUI>();
        shopItemUI.onClicked.AddListener(OnShopItemClicked);
    }

    void OnShopItemClicked(Equipment arg0){
        shopLogic.PurchaseItem(arg0);
    }

    void UpdateShopItems(){
        var itemsForSale = shopLogic.ShopData.ItemsForSale;
        var count = itemsForSale.Count;
        shopItemsPool.SetCount(count);
        for (var i = 0; i < count; i++){
            var item = itemsForSale[i];
            var obj = shopItemsPool.GetActiveObject(i);
            var shopItemUI = obj.GetComponent<ShopItemUI>();
            shopItemUI.Show(item);
        }
    }

    void UpdateMoneyUI(){
        moneyText.text = $"Money: ${ResourcesData.Money}";
    }

    void OnResourcesChanged(ResourcesData arg0){
        UpdateMoneyUI();
    }

    public override void Show(){
        base.Show();
        UpdateMoneyUI();
        UpdateShopItems();
    }

    protected void OnDestroy() {
        if (ResourcesData != null) {
            ResourcesData.onChanged.RemoveListener(OnResourcesChanged);
        }
    }
}
