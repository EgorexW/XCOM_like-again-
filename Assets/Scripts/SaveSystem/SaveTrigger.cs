using Sirenix.OdinInspector;
using UnityEngine;

public class SaveTrigger : MonoBehaviour{
    [BoxGroup("References")] [Required] [SerializeField] SaveSystem saveSystem;

    protected void Start(){
        saveSystem.Save();
    }
}
