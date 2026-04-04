using System;
using UnityEngine;

public class CoverComponent : CombatComponent{
    [SerializeField] Direction direction;

    public Direction Direction => direction;


#if UNITY_EDITOR
    [SerializeField] Transform coverSprite;
    
    void OnValidate() {
        if (combatObject != null && combatObject.OccupiesTile) {
            Debug.LogWarning($"CombatObject {combatObject.name} has a CoverComponent but also occupies a tile.");
        }

        // Delay the visual update until Unity is done validating
        UnityEditor.EditorApplication.delayCall += UpdateVisuals;
    }

    private void UpdateVisuals() {
        // Since delayCall happens later, the object might have been deleted in the meantime. 
        // We check if 'this' is null to prevent console errors.
        if (this == null || coverSprite == null) return;

        // 2. Apply the rotation
        coverSprite.localRotation = Quaternion.Euler(0, 0, direction.Angle());
        UnityEditor.EditorUtility.SetDirty(coverSprite);
    }
#endif
}