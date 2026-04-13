using UnityEngine;
using UnityEngine.Serialization;

public class AmmoComponent : CombatComponent{
    [SerializeField] int magazineSize = 6;
    [SerializeField] int magazines = 3;

    public int CurrentLoadedAmmo{ get; private set; }
    public bool IsFull => CurrentLoadedAmmo == magazineSize;
    public int Magazines => magazines;
    public int MagazineSize => magazineSize;

    private void Awake() {
        Reload();
    }

    public void ConsumeAmmo(int cost) {
        CurrentLoadedAmmo -= cost;
    }

    public void Reload() {
        if (magazines <= 0){
            Debug.Log("Out of Ammo!");
            return;
        }
        CurrentLoadedAmmo = magazineSize;
        magazines -= 1;
    }
}