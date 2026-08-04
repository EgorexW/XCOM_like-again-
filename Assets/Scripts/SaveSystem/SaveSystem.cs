using System.IO;
using Nrjwolf.Tools.AttachAttributes;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuSaveSystemBasePath)]
public class SaveSystem : ScriptableObject{
    [BoxGroup("References")][Required][SerializeField] CampaignStateHolder campaignStateHolder;
    
    [BoxGroup("References")] [Required] [SerializeField] EquipmentAssetRegistry equipmentRegistry;
    [BoxGroup("References")] [Required] [SerializeField] UnitPrefabAssetRegistry unitRegistry;

    [SerializeField] CampaignState defaultState;

    public void Save(CampaignState campaign){
        var saveData = campaign.ToSaveData(equipmentRegistry, unitRegistry);
        File.WriteAllText(GetPath(), JsonUtility.ToJson(saveData, true));
    }

    CampaignState InnerLoad(){
        var path = GetPath();
        if (File.Exists(path))
        {
            var saveData = JsonUtility.FromJson<CampaignSaveData>(File.ReadAllText(path));
            return saveData.ToCampaignState(equipmentRegistry, unitRegistry);
        }
        Save(defaultState);
        Debug.LogWarning($"No save data found at {path}. Creating a new save with default state.");
        return InnerLoad();
    }

    public CampaignState Load(){
        var state = InnerLoad();
        campaignStateHolder.SetCampaignState(state);
        return state;
    }

    public static string GetPath(){
        return $"{Application.persistentDataPath}/{Application.productName} save {SaveProfile.CurrentProfile:00}.json";
    }

    public void Save(){
        Save(campaignStateHolder.State);
    }
}