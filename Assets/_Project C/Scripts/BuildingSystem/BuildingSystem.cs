using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public event Action OnSelectedChanged;
    public event Action OnObjectPlaced;

    private GridXZ<GridObject> _grid;
    [SerializeField] private List<PlacedObjectTypeSO> _placedObjectTypeSOs = null;
    private PlacedObjectTypeSO _placedObjectTypeSO;

    private void Awake()
    {
        int gridWidth = 10;
        int gridHeight = 10;
        float cellSize = 10f;
        _grid = new GridXZ<GridObject>(gridWidth, gridHeight, cellSize, new Vector3(-50f, 0f, -50f), (GridXZ<GridObject> grid, int row, int col) => new GridObject(grid, row, col));

        _placedObjectTypeSO = null;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _placedObjectTypeSO != null)
        {
            Vector3 mouseWorldPosition = Utils.GetMouseWorldPositionWithZ();
            Vector2Int xz = _grid.GetXZ(mouseWorldPosition);

            Vector2Int placedObjectGridOrigin = new Vector2Int(xz.x, xz.y);

            // See if we can build or not
            List<Vector2Int> gridPositionList = _placedObjectTypeSO.GetGridPositionList(placedObjectGridOrigin);
            bool canBuild = true;
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                if (!_grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
                {
                    canBuild = false;
                    break;
                }
            }

            if (canBuild)
            {
                Vector2Int rotationOffset = _placedObjectTypeSO.GetRotationOffset();
                Vector3 placedObjectWorldPosition = _grid.GetWorldPosition(placedObjectGridOrigin.x, placedObjectGridOrigin.y) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * _grid.CellSize;

                GameDebug.LogMessage($"rotationOffset: {rotationOffset.x}, {rotationOffset.y}");
                GameDebug.LogMessage($"placedObjectWorldPosition: {placedObjectWorldPosition.x}, {placedObjectWorldPosition.y}, {placedObjectWorldPosition.z}");

                PlacedObject placedObject = PlacedObject.Create(placedObjectWorldPosition, placedObjectGridOrigin, _placedObjectTypeSO);

                foreach (Vector2Int gridPosition in gridPositionList)
                    _grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);

                OnObjectPlaced?.Invoke();
            }
            else
            {
                GameDebug.LogWarning("Cannot build here!");
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _placedObjectTypeSO.GetNextDir();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) { _placedObjectTypeSO = _placedObjectTypeSOs[0]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { _placedObjectTypeSO = _placedObjectTypeSOs[1]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { _placedObjectTypeSO = _placedObjectTypeSOs[2]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { _placedObjectTypeSO = _placedObjectTypeSOs[3]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { _placedObjectTypeSO = _placedObjectTypeSOs[4]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { _placedObjectTypeSO = _placedObjectTypeSOs[5]; RefreshSelectedObjectType(); }

        if (Input.GetKeyDown(KeyCode.Alpha0)) { DeselectObjectType(); }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPosition = Utils.GetMouseWorldPositionWithZ();
            if (_grid.GetGridObject(mouseWorldPosition) != null)
            {
                // Valid Grid Position
                PlacedObject placedObject = _grid.GetGridObject(mouseWorldPosition).GetPlacedObject();
                if (placedObject != null)
                {
                    // Demolish
                    placedObject.Demolish();

                    List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
                    foreach (Vector2Int gridPosition in gridPositionList)
                    {
                        _grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                    }
                }
            }
        }
    }

    private void DeselectObjectType()
    {
        _placedObjectTypeSO = null;
        RefreshSelectedObjectType();
    }

    private void RefreshSelectedObjectType()
    {
        OnSelectedChanged?.Invoke();
    }


    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        return _grid.GetXZ(worldPosition);
    }

    public Vector3 GetMouseSnappedWorldPosition()
    {
        Vector3 mouseWorldPosition = Utils.GetMouseWorldPositionWithZ();
        Vector2Int xz = _grid.GetXZ(mouseWorldPosition);

        if (_placedObjectTypeSO != null)
        {
            Vector2Int rotationOffset = _placedObjectTypeSO.GetRotationOffset();
            Vector3 placedObjectWorldPosition = _grid.GetWorldPosition(xz.x, xz.y) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * _grid.CellSize;
            return placedObjectWorldPosition;
        }
        else
            return mouseWorldPosition;
    }

    public Quaternion GetPlacedObjectRotation()
    {
        if (_placedObjectTypeSO != null)
            return Quaternion.Euler(0, _placedObjectTypeSO.GetRotationAngle(), 0);
        else
            return Quaternion.identity;
    }

    public PlacedObjectTypeSO GetPlacedObjectTypeSO()
    {
        return _placedObjectTypeSO;
    }
}