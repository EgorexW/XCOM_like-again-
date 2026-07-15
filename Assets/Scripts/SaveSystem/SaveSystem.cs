using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuSaveSystemBasePath)]
public class SaveSystem : ScriptableObject{
    [BoxGroup("References")] [Required] [SerializeField] ResourcesData resourcesData;
    [BoxGroup("References")] [Required] [SerializeField] RecruitIntakeData recruitIntakeData;
    [BoxGroup("References")] [Required] [SerializeField] EquipmentAssetRegistry equipmentRegistry;
    [BoxGroup("References")] [Required] [SerializeField] UnitPrefabAssetRegistry unitRegistry;

    [Title("Default Save")]
    [BoxGroup("References")] [Required] [SerializeField] ResourcesData initResourcesData;
    [SerializeField] int startingSquadSize = 5;

    public SaveData Data{ get; private set; } = new();

    public void Save(){
        Data.resources = resourcesData.ToSaveData(equipmentRegistry, unitRegistry);
        Data.recruitIntake.missionsCompletedSinceLastDelivery = recruitIntakeData.MissionsCompletedSinceLastDelivery;
        File.WriteAllText(GetPath(), JsonUtility.ToJson(Data, true));
    }

    public void Load(){
        var path = GetPath();
        Data = File.Exists(path) ? JsonUtility.FromJson<SaveData>(File.ReadAllText(path)) : CreateDefaultSaveData();
        resourcesData.LoadFrom(Data.resources, equipmentRegistry, unitRegistry);
        recruitIntakeData.SetMissionsCompletedSinceLastDelivery(Data.recruitIntake.missionsCompletedSinceLastDelivery);
    }

    SaveData CreateDefaultSaveData(){
        var defaultResources = CreateInstance<ResourcesData>();
        defaultResources.DeepCopy(initResourcesData);

        if (startingSquadSize > 0){
            recruitIntakeData.GenerateRecruits(defaultResources, startingSquadSize);
        }

        var defaultData = new SaveData{
            resources = defaultResources.ToSaveData(equipmentRegistry, unitRegistry)
        };
        Destroy(defaultResources);
        return defaultData;
    }

    public static string GetPath(){
        return $"{Application.persistentDataPath}/{Application.productName} save {SaveProfile.CurrentProfile:00}.json";
    }
}
