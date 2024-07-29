using System;
using TMPro;
using UnityEngine;

public class Grid<TGridObject>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    public int Width => _width;
    public int Height => _height;
    public float CellSize => _cellSize;

    private int _width;
    private int _height;
    private float _cellSize;
    private Vector3 _origin;
    private TGridObject[] _gridArray;
    private TextMeshProUGUI[] _debugTexTArray;

    public Grid(int width, int height, float cellSize, Vector3 origin, Func<TGridObject> createGridObjectFunc)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _origin = origin;

        _gridArray = new TGridObject[width * height];
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
                _gridArray[y * width + x] = createGridObjectFunc();
        }

        OnGridObjectChanged += UpdateDebugTextOnGridObjectChange;

        bool showDebug = true;
        if (showDebug)
        {
            GameObject canvas = new GameObject("Text_Canvas", typeof(Canvas));
            _debugTexTArray = new TextMeshProUGUI[width * height];
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    _debugTexTArray[y * width + x] = Utils.CreateWorldText(_gridArray[y * width + x]?.ToString(), canvas.transform, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 2, Color.white, TextAlignmentOptions.Center);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }

    }

    ~Grid()
    {
        OnGridObjectChanged -= UpdateDebugTextOnGridObjectChange;
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * _cellSize + _origin;
    }

    public Vector2Int GetXY(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - _origin).x / _cellSize);
        int y = Mathf.FloorToInt((worldPosition - _origin).y / _cellSize);
        return new Vector2Int(x, y);
    }

    public void SetGridObject(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
        {
            _gridArray[y * _width + x] = value;
            //_debugTexTArray[y * _width + x].text = _gridArray[y * _width + x]?.ToString();
            OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {   
        Vector2Int xy = GetXY(worldPosition);
        SetGridObject(xy.x, xy.y, value);
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
            return _gridArray[y * _width + x];
        else
            return default;
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        Vector2Int xy = GetXY(worldPosition);
        return GetGridObject(xy.x, xy.y);
    }

    private void UpdateDebugTextOnGridObjectChange(object sender, OnGridObjectChangedEventArgs args)
    {
        _debugTexTArray[args.y * _width + args.x].text = _gridArray[args.y * _width + args.x]?.ToString();
    }
}
