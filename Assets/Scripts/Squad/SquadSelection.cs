using UnityEngine;

public class SquadSelection : MonoBehaviour {
    [SerializeField] SquadData squadData;

    public SquadData GetSquad(){
        return squadData;
    }
}