using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuShopDataBasePath)]
public class ShopData : ScriptableObject{
    [SerializeField] List<Equipment> itemsForSale;

    public IReadOnlyList<Equipment> ItemsForSale => itemsForSale;
}
