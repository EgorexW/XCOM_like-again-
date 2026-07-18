using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class ShopLogic : MonoBehaviour{
    [FormerlySerializedAs("shopData")] [BoxGroup("Data")] [Required] [SerializeField] ShopLogicData shopLogicData;
    [BoxGroup("Data")] [Required] [SerializeField] ResourcesData resourcesData;
    
    [SerializeField] bool generateOnAwake = true;

    protected void Awake(){
        if (generateOnAwake){
            GenerateShop();
        }
    }

    Shop shop;
    // public ResourcesData ResourcesData => resourcesData;
    public Shop Shop => shop;

    public void PurchaseItem(ShopItem shopItem){
        if (resourcesData.Money < shopItem.price){
            Debug.LogWarning($"Not enough money to purchase {shopItem}! Need {shopItem.price}, have {resourcesData.Money}.");
            return;
        }
        resourcesData.ChangeMoney(-shopItem.price);
        resourcesData.AddEquipment(shopItem.items);
        Debug.Log($"Purchased {shopItem} for {shopItem.price}!");
    }

    public void GenerateShop(){
        shop = new Shop();
        foreach (var item in shopLogicData.ItemsForSale){
            shop.AddItem(item.StandardPrice, item);
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
        shop.AddItem(price, items);
    }
}

public class Shop{
    List<ShopItem> shopItems  = new();
    
    public IReadOnlyList<ShopItem> ShopItems => shopItems.AsReadOnly();
    public int Count => shopItems.Count;

    public void AddItem(int price, Equipment item){
        AddItem(new ShopItem(price, new List<Equipment>{item}));
    }
    
    public void AddItem(int price, List<Equipment> items){
        AddItem(new ShopItem(price, items));
    }

    void AddItem(ShopItem shopItem){
        shopItems.Add(shopItem);
    }
}

public struct ShopItem{
    public readonly int price;
    public readonly List<Equipment> items;

    public ShopItem(int price, List<Equipment> items){
        this.price = price;
        this.items = items;
    }
}
