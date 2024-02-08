using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SpawnedObjectsHandler : MonoBehaviour
{
    [SerializeField] private GameObject spawnablePrefab;
    [SerializeField] private Transform parentForSpawned;
    [SerializeField] private bool spawnOnVertical = true;
    [SerializeField] private int initialPoolSize = 15;
    
    private IARHitProvider _planeHitProvider;
    private ObjectPool<GameObject> _pool;
    private List<GameObject> spawnedObjects = new ();

    public void Initialize(IARHitProvider hitProvider)
    {
        _planeHitProvider = hitProvider;
        _planeHitProvider.OnARRaycastHit += OnPlaneHitAction;
        _pool = new ObjectPool<GameObject>(() =>
            SurfacedObjectsFactory.Create(spawnablePrefab, parentForSpawned), initialPoolSize);
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
        newObject.PresetSelectableActive(hitInfo.pose.position, plane.normal);
    }

    private GameObject GetNewObjectFromPool(Vector3 position, Vector3 normal)
    {
        var newObject = _pool.GetObjectFromPool();
        newObject.PresetSelectableActive(position, normal);
        spawnedObjects.Add(newObject);
        return newObject;
    }

    private void DestroyObject(GameObject poolObject)
    {
        if (spawnedObjects.Contains(poolObject))
        {
            poolObject.PresetSelectableNonActive();
            _pool.ReturnObjectToPool(poolObject);
            spawnedObjects.Remove(poolObject);
        }
        else
        {
            Destroy(poolObject);
        }
    }

    private static bool IsCorrectTypeOfPlane(ARPlane plane, bool isVerticalAllowed) =>
        isVerticalAllowed || plane.alignment == PlaneAlignment.HorizontalUp;
}