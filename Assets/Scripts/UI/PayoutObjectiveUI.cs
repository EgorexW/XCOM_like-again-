using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class PayoutObjectiveUI : UIElement {
    [BoxGroup("References")] [Required] [SerializeField] TextMeshProUGUI text;

    private PayoutObjective objective;

    public void Show(PayoutObjective objectiveTmp) {
        base.Show();
        RemoveObjective();

        this.objective = objectiveTmp;

        
            this.objective.onPayoutChanged.AddListener(OnPayoutChanged);
        
        
        UpdateUI();
    }

    void RemoveObjective(){
        this.objective?.onPayoutChanged.RemoveListener(OnPayoutChanged);
    }

    private void OnPayoutChanged(int newPayout) {
        UpdateUI();
    }

    public void UpdateUI() {
        text.text = objective.GetDescription();
    }

     public override void Hide() {
        base.Hide();
        RemoveObjective();
    }
}
