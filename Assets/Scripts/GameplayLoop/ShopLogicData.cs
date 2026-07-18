 using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuShopDataBasePath)]
public class ShopLogicData : ScriptableObject{
    [SerializeField] List<Equipment> itemsForSale;
    [SerializeField] int bundlesNr = 1;
    [SerializeField] Vector2Int bundleItemCount = new Vector2Int(3, 3);
    [SerializeField] Vector2 bundleDiscount = new Vector2(0.5f, 0.5f);

    public IReadOnlyList<Equipment> ItemsForSale => itemsForSale;
    public int BundlesNr => bundlesNr;
    public Vector2Int BundleItemCount => bundleItemCount;
    public Vector2 BundleDiscount => bundleDiscount;
}
