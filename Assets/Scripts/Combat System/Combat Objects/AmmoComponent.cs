using UnityEngine;
using UnityEngine.Serialization;

public class AmmoComponent : CombatComponent{
    [SerializeField] int magazineSize = 6;
    [SerializeField] int magazines = 3;
    private int currentLoadedAmmo;

    public int CurrentLoadedAmmo => currentLoadedAmmo;
    public bool IsFull => currentLoadedAmmo == magazineSize;
    public int Magazines => magazines;

    private void Awake() {
        Reload();
    }

    public void ConsumeAmmo(int cost) {
        currentLoadedAmmo -= cost;
    }

    public void Reload() {
        if (magazines <= 0){
            Debug.Log("Out of Ammo!");
            return;
        }
        currentLoadedAmmo = magazineSize;
        magazines -= 1;
    }
}