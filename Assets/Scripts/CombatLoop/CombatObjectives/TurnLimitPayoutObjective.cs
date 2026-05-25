using UnityEngine;

public class TurnLimitPayoutObjective : PayoutObjective{
    [SerializeField] int maxPayout = 100;
    [SerializeField] int startDecreasingTurn = 5;
    [SerializeField] int zeroPayoutTurn = 10;

    public override void UpdateObjective(CombatSystem combatSystem){
        // Use the new RoundCount on the TurnSystem
        int currentTurn = combatSystem.TurnSystem.RoundCount;

        if (currentTurn <= startDecreasingTurn){
            SetPayout(maxPayout);
        }
        else if (currentTurn >= zeroPayoutTurn){
            SetPayout(0);
        }
        else{
            float progress = (float)(currentTurn - startDecreasingTurn) / (zeroPayoutTurn - startDecreasingTurn);
            int currentPayout = Mathf.RoundToInt(Mathf.Lerp(maxPayout, 0, progress));
            SetPayout(currentPayout);
        }
    }
}
