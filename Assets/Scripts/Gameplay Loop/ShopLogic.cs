using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ShopLogic : MonoBehaviour{
    [FormerlySerializedAs("shopData")] [BoxGroup("Data")] [Required] [SerializeField] ShopLogicData shopLogicData;
    [BoxGroup("References")][Required][SerializeField] CampaignStateHolder  campaignStateHolder;
    
    [SerializeField] bool generateOnAwake = true;

    protected void Awake(){
        if (generateOnAwake){
            GenerateShop();
        }
    }

    [FoldoutGroup("Debug")][ShowInInspector] Shop shop;
    // public CampaignState CampaignState => CampaignState;
    public Shop Shop {
        get {
            if (shop == null){
                Debug.LogWarning($"Shop is null!");
            }
            return shop;
        }
    }

    public void PurchaseItem(ShopItem shopItem){
        if (campaignStateHolder.State.Money.Value < shopItem.price){
            Debug.LogWarning($"Not enough money to purchase {shopItem}! Have {campaignStateHolder.State.Money.Value}.");
            return;
        }
        campaignStateHolder.State.Money.ChangeValue(-shopItem.price);
        campaignStateHolder.State.Equipment.AddEquipment(shopItem.items);
        shop.ItemPurchased(shopItem);
        Debug.Log($"Purchased {shopItem}");
    }


    public void GenerateShop(){
        shop = new Shop();
        foreach (var item in shopLogicData.ItemsForSale){
            shop.AddItem(item.ToShopItem());
        }
        GenerateBundles();
    }

    void GenerateBundles(){
        for (int i = 0; i < shopLogicData.BundlesNr; i++){
            GenerateBundle();
        }
    }

    void GenerateBundle(){
        var itemCount = shopLogicData.BundleItemCount.Random();
        var items = new List<Equipment>();
        var standardPrice = 0f;
        for (int i = 0; i < itemCount; i++){
            items.Add(shopLogicData.ItemsForSale.Random());
            standardPrice += items[i].StandardPrice;
        }
        int price = Mathf.RoundToInt(standardPrice * shopLogicData.BundleDiscount.Random());
        shop.AddItem(new ShopItem(price, items, 1));
    }
}

public class Shop{
    [ShowInInspector] List<ShopItem> shopItems  = new();
    
    public IReadOnlyList<ShopItem> ShopItems => shopItems.AsReadOnly();
    public int Count => shopItems.Count;

    [FoldoutGroup("Events")] public UnityEvent onUpdate = new();

    public void AddItem(ShopItem shopItem){
        shopItems.Add(shopItem);
        onUpdate.Invoke();
    }

    public void RemoveItem(ShopItem shopItem){
        shopItems.Remove(shopItem);
        onUpdate.Invoke();
    }

    public void ItemPurchased(ShopItem shopItem){
        if (!shopItem.stock.HasValue){
            return;
        }
        shopItem.stock -= 1;
        if (shopItem.stock.Value <= 0){
            RemoveItem(shopItem);
        }
        onUpdate.Invoke();
    }
}

public class ShopItem{
    public readonly int price;
    public readonly List<Equipment> items;
    public int? stock;

    public ShopItem(int price, List<Equipment> items, int? stock = null){
        this.price = price;
        this.items = items;
        this.stock = stock;
    }

    public override string ToString(){
        if (items == null || items.Count == 0) return $"Empty Item ({price}$)";
        var names = new List<string>();
        foreach (var item in items){
            if (item != null) names.Add(item.name);
        }
        return $"{string.Join(", ", names)} ({price}$)";
    }
}

public static class ShopLogicExtensions{
    public static ShopItem ToShopItem(this Equipment equipment){
        return new ShopItem(equipment.StandardPrice, new List<Equipment>{equipment});
    }

    public static ShopItem ToShopItem(this Equipment equipment, int price){
        return new ShopItem(price, new List<Equipment>{equipment});
    }
}
