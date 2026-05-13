using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuEquipmentTypeBasePath)]
public class Equipment : ScriptableObject{
    [SerializeField] Sprite icon;
    [SerializeField] int standardPrice = 100;

    [SerializeField] [InlineEditor] UnitModifierFactory modifierFactory;

    public Sprite Icon => icon;

    public UnitModifier GetModifier(){
        return modifierFactory.Create();
    }
}