using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] Vector2Int bounds = new Vector2Int(13, 13);
    
    [SerializeField] SpawnTable spawnTable;

    public void GenerateLevel(){
        var points = GetNodesInBoundingBox();
        foreach (var point in points){
            SpawnPoint(point);
        }
    }

    void SpawnPoint(Vector2Int point){
        var spawnable = spawnTable.GetGameObject();
        if (spawnable != null){
            Instantiate(spawnable, new Vector3(point.x, point.y, 0), Quaternion.identity, transform);
        }
    }

    public List<Vector2Int> GetNodesInBoundingBox()
    {
        List<Vector2Int> points = new List<Vector2Int>();
        
        Vector2 origin = new Vector2(transform.position.x, transform.position.y);

        Vector2 extents = (Vector2)bounds * 0.5f;
            var min = origin - extents;
            var max = origin + extents;
        

        int minX = Mathf.CeilToInt(min.x);
        int maxX = Mathf.FloorToInt(max.x);
        int minY = Mathf.CeilToInt(min.y);
        int maxY = Mathf.FloorToInt(max.y);

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                points.Add(new Vector2Int(x, y));
            }
        }

        return points;
    }
}
