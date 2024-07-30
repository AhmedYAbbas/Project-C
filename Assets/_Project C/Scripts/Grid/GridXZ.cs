using CodeMonkey.Utils;
using System;
using TMPro;
using UnityEngine;

public class GridXZ<TGridObject>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int z;
    }

    public int Width => _width;
    public int Height => _height;
    public float CellSize => _cellSize;

    private int _width;
    private int _height;
    private float _cellSize;
    private Vector3 _origin;
    private TGridObject[] _gridArray;
    private TextMeshProUGUI[] _debugTextArray;

    public GridXZ(int width, int height, float cellSize, Vector3 origin, Func<GridXZ<TGridObject>, int, int, TGridObject> createGridObjectFunc)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _origin = origin;

        _gridArray = new TGridObject[width * height];
        for (int z = 0; z < height; ++z)
        {
            for (int x = 0; x < width; ++x)
                _gridArray[z * width + x] = createGridObjectFunc(this, x, z);
        }

        bool showDebug = true;
        if (showDebug)
        {
            GameObject canvas = new GameObject("Text_Canvas", typeof(Canvas));
            _debugTextArray = new TextMeshProUGUI[width * height];
            for (int z = 0; z < _height; z++)
            {
                for (int x = 0; x < _width; x++)
                {
                    _debugTextArray[z * _width + x] = Utils.CreateWorldText(_gridArray[z * _width + x]?.ToString(), canvas.transform, GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * .5f, 2, Color.white, TextAlignmentOptions.Center);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs args) =>
            {
                _debugTextArray[args.z * _width + args.x].text = _gridArray[args.z * _width + args.x]?.ToString();
            };
        }
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * _cellSize + _origin;
    }

    public Vector2Int GetXZ(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - _origin).x / _cellSize);
        int z = Mathf.FloorToInt((worldPosition - _origin).z / _cellSize);
        return ValidateGridPosition(x, z);
    }

    public void TriggerGridObjectChanged(int x, int z)
    {
        OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, z = z });
    }

    public void SetGridObject(int x, int z, TGridObject value)
    {
        if (x >= 0 && z >= 0 && x < _width && z < _height)
        {
            _gridArray[z * _width + x] = value;
            TriggerGridObjectChanged(x, z);
        }
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        Vector2Int xz = GetXZ(worldPosition);
        SetGridObject(xz.x, xz.y, value);
    }

    public TGridObject GetGridObject(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < _width && z < _height)
            return _gridArray[z * _width + x];
        else
            return default;
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        Vector2Int xz = GetXZ(worldPosition);
        return GetGridObject(xz.x, xz.y);
    }

    public Vector2Int ValidateGridPosition(int x, int z)
    {
        return new Vector2Int(
            Mathf.Clamp(x, 0, _width - 1),
            Mathf.Clamp(z, 0, _height - 1)
        );
    }
} 
