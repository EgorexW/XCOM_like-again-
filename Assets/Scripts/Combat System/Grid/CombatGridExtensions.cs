using System.Collections.Generic;
using UnityEngine;

public static class CombatGridExtensions{
    public static bool InStraightLine(this CombatGridNode node1, CombatGridNode node2){
        if (node1.x != node2.x && node1.y != node2.y){
            return false;
        }
        return true;
    }

    public static bool LineUnobstructed(this CombatGridNode node1, CombatGridNode node2,
        GridOccupancyType obstructionType, List<ICombatObject> objectsToIgnore = null){
        objectsToIgnore ??= new List<ICombatObject>();
        foreach (var node in node1.GetNodesInBetween(node2))
            if (!node.CanAcceptObject(obstructionType, objectsToIgnore)){
                return false;
            }
        return true;
    }

    public static List<CombatGridNode> GetNodesInBetween(this CombatGridNode node1, CombatGridNode node2){
        var nodes = new List<CombatGridNode>();

        int x = node1.x;
        int y = node1.y;
        int x1 = node2.x;
        int y1 = node2.y;

        int dx = Mathf.Abs(x1 - x);
        int dy = Mathf.Abs(y1 - y);
    
        int stepX = x < x1 ? 1 : -1;
        int stepY = y < y1 ? 1 : -1;

        // Notice we don't multiply err by 2 down in the loop anymore
        int error = dx - dy;
        dx *= 2;
        dy *= 2;

        while (true){
            var node = node1.grid.GetNode(new Vector2Int(x, y));
        
            // Add the node (and make sure we don't add duplicates from the magic block)
            if (node != null && !nodes.Contains(node)){
                nodes.Add(node);
            }

            // We reached the target node! Break immediately.
            if (x == x1 && y == y1) {
                break;
            }

            // SUPERCOVER LOGIC: We process X and Y steps independently
            if (error > 0) {
                x += stepX;
                error -= dy;
            } 
            else if (error < 0) {
                y += stepY;
                error += dx;
            } 
            else { 
                // THE MAGIC BLOCK: error == 0 (Perfect Diagonal)
                // The line crosses the exact corner of 4 tiles. We check them all.
                var corner1 = node1.grid.GetNode(new Vector2Int(x + stepX, y));
                var corner2 = node1.grid.GetNode(new Vector2Int(x, y + stepY));
            
                if (corner1 != null && corner1 != node2) nodes.Add(corner1);
                if (corner2 != null && corner2 != node2) nodes.Add(corner2);

                x += stepX;
                y += stepY;
                error -= dy;
                error += dx;
            }
        }

        // Clean up
        nodes.Remove(node1);
        nodes.Remove(node2);

        return nodes;
    }

    public static float GetDistance(this CombatGridNode node1, CombatGridNode node2){
        float dx = node1.x - node2.x;
        float dy = node1.y - node2.y;
        return Mathf.Sqrt(dx * dx + dy * dy);
    }

    public static List<Direction> GetDirections(this CombatGridNode attackerNode, CombatGridNode targetNode, int diagonalThreshold = 0){
        var directions = new List<Direction>();
            
        var dx = attackerNode.x - targetNode.x;
        var dy = attackerNode.y - targetNode.y;
        
        if (Mathf.Abs(dx) + diagonalThreshold >= Mathf.Abs(dy)){
            if (dx > 0){
                directions.Add(Direction.Left);
            }
            else if (dx < 0){
                directions.Add(Direction.Right);
            }
        } 
        if (Mathf.Abs(dy) + diagonalThreshold >= Mathf.Abs(dx)){
            if (dy > 0){
                directions.Add(Direction.Down);
            }
            else if (dy < 0){
                directions.Add(Direction.Up);
            }
        }

        return directions;
    }
    
    public static List<CombatGridNode> GetNodesInRadius(this CombatGridNode centerNode, float radius){
        return GetNodesInRadius(centerNode.grid, centerNode.GetPos(), radius);
    }
    
    public static List<CombatGridNode> GetNodesInRadius(this CombatGrid grid, Vector2 point, float radius){
        var nodes = new List<CombatGridNode>();

        // Strictly respect the float point for the bounding box
        int minX = Mathf.FloorToInt(point.x - radius);
        int maxX = Mathf.CeilToInt(point.x + radius);
        int minY = Mathf.FloorToInt(point.y - radius);
        int maxY = Mathf.CeilToInt(point.y + radius);

        // Square the radius once to avoid running Mathf.Sqrt inside the loop
        float radiusSqr = radius * radius;

        for (int x = minX; x <= maxX; x++){
            for (int y = minY; y <= maxY; y++){
                var targetNode = grid.GetNode(new Vector2Int(x, y));
                if (targetNode == null){
                    continue;
                }
                
                // Float distance check against the exact Vector2 point
                float dx = point.x - targetNode.x;
                float dy = point.y - targetNode.y;
                
                if ((dx * dx + dy * dy) <= radiusSqr){
                    nodes.Add(targetNode);
                }
            }
        }
        return nodes;
    }
    
    public static List<CombatGridNode> GetNeighborNodes(this CombatGridNode node, bool includeDiagonals = false){
        var neighbors = new List<CombatGridNode>();
        for (int x = -1; x <= 1; x++){
            for (int y = -1; y <= 1; y++){
                if (x == 0 && y == 0) {
                    continue;
                }
                
                if (!includeDiagonals && Mathf.Abs(x) == Mathf.Abs(y)){
                    continue;
                }
                
                var targetNode = node.grid.GetNode(new Vector2Int(node.x + x, node.y + y));
                if (targetNode == null){
                    continue;
                }

                neighbors.Add(targetNode);
            }
        }

        return neighbors;
    }

    public static Vector2 GetCenter(this List<CombatGridNode> nodes){
        if (nodes == null || nodes.Count == 0){
            Debug.LogError("No grid nodes found");
            return Vector2.zero; 
        }

        float sumX = 0;
        float sumY = 0;

        foreach (var node in nodes){
            sumX += node.x;
            sumY += node.y;
        }

        return new Vector2(sumX / nodes.Count, sumY / nodes.Count);
    }

    public static CombatGridNode GetCenterNode(this List<CombatGridNode> nodes){
        if (nodes == null || nodes.Count == 0){
            Debug.LogError("No grid nodes found");
            return null; // FIXED: You were missing this return!
        }
        
        Vector2 exactCenter = nodes.GetCenter();

        CombatGridNode closestNode = null;
        float closestDistanceSqr = float.MaxValue;
        
        // Track how many nodes share the exact same distance to the center
        int tieCount = 1; 

        foreach (var node in nodes) {
            float dx = node.x - exactCenter.x;
            float dy = node.y - exactCenter.y;
            float distSqr = dx * dx + dy * dy; // Sqr magnitude
            
            if (Mathf.Approximately(distSqr, closestDistanceSqr)) {
                tieCount++;
            }
            else if (distSqr < closestDistanceSqr) {
                closestDistanceSqr = distSqr;
                closestNode = node;
                tieCount = 1; // We found a new true closest, reset the tie breaker
            }
        }

        // THE WARNING:
        if (tieCount > 1) {
            Debug.LogWarning($"[Grid Paranoia] GetCenterNode was called on a shape with no objective center! Tie between {tieCount} nodes. Defaulting to node {closestNode.x},{closestNode.y}.");
        }

        return closestNode;
    }
    
    public static CombatGrid PrimaryGrid(this List<CombatGridNode> nodes){
        if (nodes == null || nodes.Count == 0) return null;
        // The "Center of Mass" dictates which grid this object officially belongs to
        CombatGrid primaryGrid = nodes.GetCenterNode().grid;

        // PARANOIA CHECK: Are we straddling two different dimensions?
        foreach (var node in nodes) {
            if (node.grid != primaryGrid) {
                Debug.LogWarning($"{nodes} are occupying nodes across multiple different grids! Proceed with caution.");
                break;
            }
        }

        return primaryGrid;
    }

    #region Components

    public static List<HazardComponent> GetHazards(this CombatGridNode node){
        var hazards = new List<HazardComponent>();
        foreach (var combatObject in node.GetCombatObjects()){
            var hazard = combatObject.GetCombatComponent<HazardComponent>();
            if (hazard != null){
                hazards.Add(hazard);
            }
        }
        return hazards;
    }

    #endregion

    #region Attacks

    public static bool IsProtectedFrom(this CombatGridNode node, Direction attackDirection){
        foreach (var obj in node.GetCombatObjects()){
            var cover = obj.GetCombatComponent<CoverComponent>();
            if (cover != null){
                if (cover.Direction == attackDirection){
                    return true;
                }
            }
        }
        return false;
    }


    public static bool CanAttack(this CombatGridNode attackerNode, CombatGridNode targetNode, List<ICombatObject> objectsToIgnore = null){
        objectsToIgnore ??= new List<ICombatObject>();
        if (!attackerNode.LineUnobstructed(targetNode, GridOccupancyType.Character, objectsToIgnore)){
            return false;
        }
        var attackDirections = GetDirections(attackerNode, targetNode);

        // Debug.Log($"Attacking from {attackerNode.x},{attackerNode.y} to {targetNode.x},{targetNode.y} in directions {attackDirections}");

        foreach (var direction in attackDirections){
            if (targetNode.IsProtectedFrom(direction.Opposite())){
                return false;
            }
        }

        return true;
    }

    #endregion
}