using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GridUITooltips : MonoBehaviour
{
    [BoxGroup("References")] [Required] [SerializeField] TileHoverContext tileHoverContext;

    [BoxGroup("References")] [Required] [SerializeField] GridUI coverAgainstUI;
    [BoxGroup("References")] [Required] [SerializeField] GridUI noLoSUI;

    [SerializeField] float evaluateRange = 20f;

    protected void Awake(){
        tileHoverContext.onHoverTile.AddListener(OnHoverTile);
        tileHoverContext.onHoverClear.AddListener(OnHoverClear);
    }

    void OnHoverClear(){
        gameObject.SetActive(false);
    }

    void OnHoverTile(CombatGridNode tile){
        gameObject.SetActive(true);
        var tiles = tile.GetNodesInRadius(evaluateRange);
        var coverAgainstTiles = new List<CombatGridNode>();
        var noLoSTiles = new List<CombatGridNode>();
        foreach (var tileTmp in tiles){
            if (!tileTmp.LineUnobstructed(tile, CombatObjectFlags.LoSBlocker)){
                noLoSTiles.Add(tileTmp);
                continue;
            }
            if (!tileTmp.CanShoot(tile)){
                coverAgainstTiles.Add(tileTmp);
            }
        }
        coverAgainstUI.MarkPositons(coverAgainstTiles);
        noLoSUI.MarkPositons(noLoSTiles);
    }
}
