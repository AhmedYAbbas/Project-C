using UnityEngine;

public class GridObject
{
    private GridXZ<GridObject> _grid;
    private int _row;
    private int _col;
    private PlacedObject _placedObject;

    public GridObject(GridXZ<GridObject> grid, int row, int col)
    {
        _grid = grid;
        _row = row;
        _col = col;
        _placedObject = null;
    }

    public override string ToString()
    {
        return _row + ", " + _col + '\n' + _placedObject?.ToString();
    }

    public void SetPlacedObject(PlacedObject placedObject)
    {
        _placedObject = placedObject;
        _grid.TriggerGridObjectChanged(_row, _col);
    }

    public void ClearPlacedObject()
    {
        _placedObject = null;
        _grid.TriggerGridObjectChanged(_row, _col);
    }

    public PlacedObject GetPlacedObject()
    {
        return _placedObject;
    }

    public bool CanBuild()
    {
        return _placedObject == null;
    }
}
