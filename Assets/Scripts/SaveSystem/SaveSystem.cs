using System.IO;
using Nrjwolf.Tools.AttachAttributes;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = StringKeys.AssetMenuSaveSystemBasePath)]
public class SaveSystem : ScriptableObject{
    [BoxGroup("References")][Required][SerializeField] CampaignStateHolder campaignStateHolder;
    
    [BoxGroup("References")] [Required] [SerializeField] EquipmentAssetRegistry equipmentRegistry; //TODO
    [BoxGroup("References")] [Required] [SerializeField] UnitPrefabAssetRegistry unitRegistry;
    
    [SerializeField] CampaignState defaultState;

    public void Save(CampaignState campaign){
        File.WriteAllText(GetPath(), JsonUtility.ToJson(campaign, true));
    }

    CampaignState InnerLoad(){
        var path = GetPath();
        if (File.Exists(path))
        {
            return JsonUtility.FromJson<CampaignState>(File.ReadAllText(path));
        }
        Save(defaultState);
        Debug.LogWarning($"No save data found at {path}. Creating a new save with default state.");
        return Load();
    }

    public CampaignState Load(){
        // var state = InnerLoad();
        var state = defaultState;
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