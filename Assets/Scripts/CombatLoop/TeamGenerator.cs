using UnityEngine;

public abstract class TeamGenerator : MonoBehaviour{
    public TeamFlag teamFlag;
    
    public abstract Team GenerateTeam();
}