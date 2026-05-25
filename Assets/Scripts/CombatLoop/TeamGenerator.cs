using UnityEngine;

public abstract class TeamGenerator : MonoBehaviour{
    public TeamFlag teamFlag;
    public MapNodeType targetNodeType;

    public abstract Team GenerateTeam();
}