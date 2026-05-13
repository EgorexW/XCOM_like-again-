using Sirenix.OdinInspector;
using UnityEngine;

public class ShopLogic : MonoBehaviour{
    [BoxGroup("Data")] [Required] [SerializeField] ShopData shopData;
    [BoxGroup("Data")] [Required] [SerializeField] ResourcesData resourcesData;

    public ShopData ShopData => shopData;
    public ResourcesData ResourcesData => resourcesData;

    public void PurchaseItem(Equipment item){
        if (resourcesData.Money < item.StandardPrice){
            Debug.LogWarning($"Not enough money to purchase {item.name}! Need {item.StandardPrice}, have {resourcesData.Money}.");
            return;
        }
        resourcesData.ChangeMoney(-item.StandardPrice);
        resourcesData.AddEquipment(item);
        Debug.Log($"Purchased {item.name} for {item.StandardPrice}!");
    }
}
