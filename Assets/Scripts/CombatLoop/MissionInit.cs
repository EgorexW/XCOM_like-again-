using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class MissionInit : MonoBehaviour
{
    public abstract void InitMission(CombatContent content);
}
