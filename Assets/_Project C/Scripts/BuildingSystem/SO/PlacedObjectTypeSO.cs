using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Building System/Placed Object", fileName = "PlacedObject")]
public class PlacedObjectTypeSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public Transform Prefab { get; set; }
    [field: SerializeField] public Transform Visuals { get; set; }
    [field: SerializeField] public int Width { get; set; }
    [field: SerializeField] public int Height { get; set; }
    [field: SerializeField] public Dir Direction { get; set; }

    public enum Dir
    {
        Down,
        Left,
        Up,
        Right,
    }

    public void GetNextDir()
    {
        switch (Direction)
        {
            default:
            case Dir.Down:
                Direction = Dir.Left;
                break;
            case Dir.Left:
                Direction = Dir.Up;
                break;
            case Dir.Up:
                Direction = Dir.Right;
                break;
            case Dir.Right:
                Direction = Dir.Down;
                break;
        }
    }

    public int GetRotationAngle()
    {
        switch (Direction)
        {
            default:
            case Dir.Down:
                return 0;
            case Dir.Left:
                return 90;
            case Dir.Up:
                return 180;
            case Dir.Right:
                return 270;
        }
    }

    public Vector2Int GetRotationOffset()
    {
        switch (Direction)
        {
            default:
            case Dir.Down:
                return new Vector2Int(0, 0);
            case Dir.Left:
                return new Vector2Int(0, Width);
            case Dir.Up:
                return new Vector2Int(Width, Height);
            case Dir.Right:
                return new Vector2Int(Height, 0);
        }
    }

    public List<Vector2Int> GetGridPositionList(Vector2Int offset)
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        switch (Direction)
        {
            default:
            case Dir.Down:
            case Dir.Up:
                for (int row = 0; row < Width; ++row)
                {
                    for (int col = 0; col < Height; ++col)
                    {
                        gridPositionList.Add(offset + new Vector2Int(row, col));
                    }
                }
                break;
            case Dir.Left:
            case Dir.Right:
                for (int col = 0; col < Height; ++col)
                {
                    for (int row = 0; row < Width; ++row)
                    {
                        gridPositionList.Add(offset + new Vector2Int(col, row));
                    }
                }
                break;
        }

        return gridPositionList;
    }
}
