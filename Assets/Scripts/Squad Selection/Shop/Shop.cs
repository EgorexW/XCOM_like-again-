using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.Events;

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

public static class ShopExtensions{
    public static ShopItem ToShopItem(this Equipment equipment){
        return new ShopItem(equipment.StandardPrice, new List<Equipment>{equipment});
    }

    public static ShopItem ToShopItem(this Equipment equipment, int price){
        return new ShopItem(price, new List<Equipment>{equipment});
    }
}