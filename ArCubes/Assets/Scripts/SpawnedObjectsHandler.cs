using System.Collections.Generic;
using Selectable;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SpawnedObjectsHandler : MonoBehaviour
{
    [SerializeField] private SelectableObject selectablePrefab;
    [SerializeField] private Transform parentForSpawned;
    [SerializeField] private bool spawnOnVertical = true;
    [SerializeField] private int initialPoolSize = 15;

    private IARHitProvider _planeHitProvider;
    private ObjectPool<SelectableObject> _pool;
    private List<SelectableObject> activeSelectables = new();

    public List<SelectableObject> ActiveSelectables => activeSelectables;

    public void Initialize(IARHitProvider hitProvider)
    {
        _planeHitProvider = hitProvider;
        _planeHitProvider.OnARRaycastHit += OnPlaneHitAction;
        _pool = new ObjectPool<SelectableObject>(() =>
            SurfacedObjectsFactory.Create(selectablePrefab, parentForSpawned), initialPoolSize);
    }

    public void OnDestroy()
    {
        _planeHitProvider.OnARRaycastHit -= OnPlaneHitAction;
    }

    private void OnPlaneHitAction(ARRaycastHit hitInfo)
    {
        var plane = hitInfo.trackable as ARPlane;
        if (plane == null || !IsCorrectTypeOfPlane(plane, spawnOnVertical))
            return;

        var newObject = GetNewObjectFromPool(hitInfo.pose.position, plane.normal);
    }

    private SelectableObject GetNewObjectFromPool(Vector3 position, Vector3 normal)
    {
        var newObject = _pool.GetObjectFromPool();
        newObject.gameObject.PresetSelectableActive(position, normal);
        newObject.OnDeletionTriggered += OnObjectDeletionTriggered;
        activeSelectables.Add(newObject);
        return newObject;
    }

    private void OnObjectDeletionTriggered(SelectableObject selectableObject)
    {
        DestroyPoolObject(selectableObject);
    }

    private void DestroyPoolObject(SelectableObject poolObject)
    {
        if (activeSelectables.Contains(poolObject))
        {
            poolObject.gameObject.PresetSelectableNonActive(parentForSpawned);
            poolObject.ResetState();
            poolObject.OnDeletionTriggered -= OnObjectDeletionTriggered;
            _pool.ReturnObjectToPool(poolObject);
            activeSelectables.Remove(poolObject);
        }
        else
        {
            Destroy(poolObject.gameObject);
        }
    }

    private static bool IsCorrectTypeOfPlane(ARPlane plane, bool isVerticalAllowed) =>
        isVerticalAllowed || plane.alignment == PlaneAlignment.HorizontalUp;
}