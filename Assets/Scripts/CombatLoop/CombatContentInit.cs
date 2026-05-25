using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

class CombatContentInit : MonoBehaviour{
    [Header("DEPRECATED: Copy variables to new objects and delete this script")]
    [BoxGroup("References")] [Required] [SerializeField] List<TeamGenerator> teamGenerators;
    [BoxGroup("References")] [Required] [SerializeField] List<TurnTaker> turnTakers;

    [BoxGroup("References")] [Required] [SerializeField] List<GameObject> levelPrefabs;

    // public CombatContent Init() logic moved to MissionInit
}