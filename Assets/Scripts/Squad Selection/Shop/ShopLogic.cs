using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class ShopLogic : MonoBehaviour{
    [FormerlySerializedAs("shopLogicData")] [BoxGroup("Data")] [Required] [SerializeField] ShopData shopData;
    [BoxGroup("References")][Required][SerializeField] CampaignManager campaignManager;

    [FoldoutGroup("Debug")][ShowInInspector] Shop shop;
    public CampaignState CampaignState => campaignManager.State;

    public Shop GetShop(){
        if (shop == null){
            Debug.LogError($"Shop is null!");
        }
        return shop;
    }

    public void PurchaseItem(ShopItem shopItem){
        if (CampaignState.Money.Value < shopItem.price){
            Debug.LogWarning($"Not enough money to purchase {shopItem}! Have {CampaignState.Money.Value}.");
            return;
        }
        CampaignState.Money.ChangeValue(-shopItem.price);
        CampaignState.Equipment.AddEquipment(shopItem.items);
        shop.ItemPurchased(shopItem);
        Debug.Log($"Purchased {shopItem}");
    }


    public void GenerateShop(){
        shop = new Shop();
        foreach (var item in shopData.ItemsForSale){
            shop.AddItem(item.ToShopItem());
        }
        GenerateBundles();
    }

    void GenerateBundles(){
        for (int i = 0; i < shopData.BundlesNr; i++){
            GenerateBundle();
        }
    }

    void GenerateBundle(){
        var itemCount = shopData.BundleItemCount.Random();
        var items = new List<Equipment>();
        var standardPrice = 0f;
        for (int i = 0; i < itemCount; i++){
            items.Add(shopData.ItemsForSale.Random());
            standardPrice += items[i].StandardPrice;
        }
        int price = Mathf.RoundToInt(standardPrice * shopData.BundleDiscount.Random());
        shop.AddItem(new ShopItem(price, items, 1));
    }
}
