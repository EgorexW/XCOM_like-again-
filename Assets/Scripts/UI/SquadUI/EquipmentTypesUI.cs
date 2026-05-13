using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

class EquipmentTypesUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] ObjectsPool objectsPool;

    [FoldoutGroup("Events")] public UnityEvent<Equipment> onEquipmentTypeClicked;

    void Awake(){
        objectsPool.onCreateObject.AddListener(OnCreateObject);
    }

    public void Show(IReadOnlyList<Equipment> equipmentTypes){
        base.Show();
        objectsPool.SetCount(equipmentTypes.Count);
        for (var i = 0; i < equipmentTypes.Count; i++){
            var equipmentType = equipmentTypes[i];
            var obj = objectsPool.GetActiveObject(i);
            var equipmentTypeUI = obj.GetComponent<EquipmentTypeUI>();
            equipmentTypeUI.Show(equipmentType);
        }
    }

    void OnCreateObject(GameObject arg0){
        arg0.GetComponent<EquipmentTypeUI>().onClicked.AddListener(OnEquipmentTypeClicked);
    }

    void OnEquipmentTypeClicked(Equipment arg0){
        onEquipmentTypeClicked.Invoke(arg0);
    }
}