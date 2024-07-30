using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    [SerializeField] private BuildingSystem _buildingSystem;
    [SerializeField] private LayerMask _layer;
    private Transform _visuals;
    private PlacedObjectTypeSO placedObjectTypeSO;

    private void Start()
    {
        RefreshVisual();
        _buildingSystem.OnSelectedChanged += RefreshVisual;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = _buildingSystem.GetMouseSnappedWorldPosition();
        targetPosition.y = 1f;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);

        transform.rotation = Quaternion.Lerp(transform.rotation, _buildingSystem.GetPlacedObjectRotation(), Time.deltaTime * 15f);
    }

    private void RefreshVisual()
    {
        if (_visuals != null)
        {
            Destroy(_visuals.gameObject);
            _visuals = null;
        }

        PlacedObjectTypeSO placedObjectTypeSO = _buildingSystem.GetPlacedObjectTypeSO();

        if (placedObjectTypeSO != null)
        {
            _visuals = Instantiate(placedObjectTypeSO.Visuals, Vector3.zero, Quaternion.identity);
            _visuals.parent = transform;
            _visuals.localPosition = Vector3.zero;
            _visuals.localEulerAngles = Vector3.zero;
            SetLayerRecursive(_visuals.gameObject, 11);
        }
    }

    private void SetLayerRecursive(GameObject targetGameObject, LayerMask layer)
    {
        targetGameObject.layer = (int)layer;
        foreach (Transform child in targetGameObject.transform)
        {
            SetLayerRecursive(child.gameObject, layer);
        }
    }
}
