using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class PayoutObjectiveUI : UIElement{
    [BoxGroup("References")] [Required] [SerializeField] TextMeshProUGUI text;

    PayoutObjective objective;

    public void Show(PayoutObjective objectiveTmp){
        base.Show();
        RemoveObjective();

        objective = objectiveTmp;


        objective.onPayoutChanged.AddListener(OnPayoutChanged);


        UpdateUI();
    }

    void RemoveObjective(){
        objective?.onPayoutChanged.RemoveListener(OnPayoutChanged);
    }

    void OnPayoutChanged(int newPayout){
        UpdateUI();
    }

    public void UpdateUI(){
        text.text = objective.GetDescription();
    }

    public override void Hide(){
        base.Hide();
        RemoveObjective();
    }
}