using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    private PlacedObjectTypeSO _placedObjectTypeSO;
    private Vector2Int _gridOrigin;

    public static PlacedObject Create(Vector3 worldPosition, Vector2Int origin, PlacedObjectTypeSO placedObjectTypeSO)
    {
        Transform placedObjectTransform = Instantiate(placedObjectTypeSO.Prefab, worldPosition, Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(), 0));

        PlacedObject placedObject = placedObjectTransform.GetComponent<PlacedObject>();
        placedObject.Setup(placedObjectTypeSO, origin);
        return placedObject;
    }


    public List<Vector2Int> GetGridPositionList()
    {
        return _placedObjectTypeSO.GetGridPositionList(_gridOrigin);
    }

    public void Demolish()
    {
        Destroy(gameObject);
    }

    public override string ToString()
    {
        return _placedObjectTypeSO.Name;
    }

    private void Setup(PlacedObjectTypeSO placedObjectTypeSO, Vector2Int origin)
    {
        _placedObjectTypeSO = placedObjectTypeSO;
        _gridOrigin = origin;
    }
}
