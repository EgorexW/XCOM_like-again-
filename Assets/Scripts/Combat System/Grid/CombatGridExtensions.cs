using System.Collections.Generic;
using UnityEngine;

public static class CombatGridExtensions{
    public static bool InStraightLine(this CombatGridNode node1, CombatGridNode node2){
        if (node1.x != node2.x && node1.y != node2.y){
            return false;
        }
        return true;
    }

    public static bool LineUnobstructed(this CombatGridNode node1, CombatGridNode node2){
        foreach (var node in node1.GetNodesInBetween(node2))
            if (node.IsOccupied){
                return false;
            }
        return true;
    }

    public static List<CombatGridNode> GetNodesInBetween(this CombatGridNode node1, CombatGridNode node2){
        var nodes = new List<CombatGridNode>();

        var x0 = node1.x;
        var y0 = node1.y;
        var x1 = node2.x;
        var y1 = node2.y;

        var dx = Mathf.Abs(x1 - x0);
        var dy = Mathf.Abs(y1 - y0);

        var stepX = x0 < x1 ? 1 : -1;
        var stepY = y0 < y1 ? 1 : -1;

        // We multiply by 2 so we can do perfect integer math without decimals
        var error = dx - dy;
        var dx2 = dx * 2;
        var dy2 = dy * 2;

        var x = x0;
        var y = y0;

        while (true){
            nodes.Add(node1.grid.GetNode(new Vector2Int(x, y)));

            // We reached the target!
            if (x == x1 && y == y1){
                break;
            }

            // Step Horizontally
            if (error > 0){
                x += stepX;
                error -= dy2;
            }
            // Step Vertically
            else if (error < 0){
                y += stepY;
                error += dx2;
            }
            // THE MAGIC BLOCK: error == 0
            // The line passes exactly through a 4-way intersection!
            else{
                // 1. Grab the Horizontal Corner
                var cornerX1 = x + stepX;
                var cornerY1 = y;
                nodes.Add(node1.grid.GetNode(new Vector2Int(cornerX1, cornerY1)));

                // 2. Grab the Vertical Corner
                var cornerX2 = x;
                var cornerY2 = y + stepY;
                nodes.Add(node1.grid.GetNode(new Vector2Int(cornerX2, cornerY2)));

                // 3. Finally, move the main tracker diagonally
                x += stepX;
                y += stepY;
                error -= dy2;
                error += dx2;
            }
        }

        nodes.Remove(node1);
        nodes.Remove(node2);

        return nodes;
    }

    public static float GetDistance(this CombatGridNode node1, CombatGridNode node2){
        float dx = node1.x - node2.x;
        float dy = node1.y - node2.y;
        return Mathf.Sqrt(dx * dx + dy * dy);
    }

    public static Direction GetDirection(CombatGridNode attackerNode, CombatGridNode targetNode){
        var dx = attackerNode.x - targetNode.x;
        var dy = attackerNode.y - targetNode.y;

        if (Mathf.Abs(dx) > Mathf.Abs(dy)){
            return dx > 0 ? Direction.Left : Direction.Right;
        }
        return dy > 0 ? Direction.Up : Direction.Down;
    }
    
    public static List<CombatGridNode> GetNodesInRadius(this CombatGridNode centerNode, float radius){
        var nodes = new List<CombatGridNode>();
        int boundingBox = Mathf.CeilToInt(radius);

        for (int x = centerNode.x - boundingBox; x <= centerNode.x + boundingBox; x++){
            for (int y = centerNode.y - boundingBox; y <= centerNode.y + boundingBox; y++){
                var targetNode = centerNode.grid.GetNode(new Vector2Int(x, y));
                if (targetNode == null){
                    continue;
                }
                if (centerNode.GetDistance(targetNode) <= radius){
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


    public static bool CanAttack(this CombatGridNode attackerNode, CombatGridNode targetNode){
        if (!attackerNode.LineUnobstructed(targetNode)){
            return false;
        }
        var attackDirection = GetDirection(attackerNode, targetNode);

        if (targetNode.IsProtectedFrom(attackDirection.Opposite())){
            return false;
        }

        return true;
    }

    #endregion
}