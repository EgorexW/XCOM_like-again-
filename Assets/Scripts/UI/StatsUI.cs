using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class StatsUI : UIElement{
    [BoxGroup("References")][Required][SerializeField] CampaignState CampaignState;
    
    [BoxGroup("References")] [Required] [SerializeField] TextMeshProUGUI moneyText;
    // [BoxGroup("References")] [Required] [SerializeField] TextMeshProUGUI retirementRateText;
    
    
    void UpdateUI(){
        moneyText.text = $"Money: ${CampaignState.Money}";
        // retirementRateText.text = $"Retirement Rate: {CampaignState.RetirementRate():P1}";
    }

    void Start(){
        CampaignState.onChanged  += UpdateUI;
        UpdateUI();
    }

    void OnResourcesChanged(CampaignState arg0){
        UpdateUI();
    }

    public override void Show(){
        base.Show();
        UpdateUI();
    }
    
    protected void OnDestroy() {
        if (CampaignState != null) {
            CampaignState.onChanged  -= UpdateUI;
        }
    }
}
