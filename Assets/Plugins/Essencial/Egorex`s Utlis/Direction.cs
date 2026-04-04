using UnityEngine;

public enum Direction
{
    Left = 1 << 0,
    Right = 1 << 1,
    Up = 1 << 2,
    Down = 1 << 3
}

public static class DirectionExtensions{
    public static Vector2 Vector(this Direction direction){
        return direction switch{
            Direction.Left => Vector2.left,
            Direction.Right => Vector2.right,
            Direction.Up => Vector2.up,
            Direction.Down => Vector2.down,
            _ => Vector2.zero
        };
    }
    public static float Angle(this Direction direction){
        return direction switch {
            Direction.Right => 0f,
            Direction.Up => 90f,
            Direction.Left => 180f,
            Direction.Down => 270f,
            _ => 0f
        };
    }
    public static Direction Opposite(this Direction direction){
        return direction switch {
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            _ => direction
        };
    }
}