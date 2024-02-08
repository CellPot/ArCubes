using System.Collections.Generic;
using Selectable;
using UI;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SpawnedObjectsHandler : MonoBehaviour
{
    [SerializeField] private SelectableObject selectablePrefab;
    [SerializeField] private Transform parentForSpawned;
    [SerializeField] private bool spawnOnVertical = true;
    [SerializeField] private int initialPoolSize = 15;

    private IARHitProvider planeHitProvider;
    private UIHandler uiHandler;
    private ObjectPool<SelectableObject> objectsPool;
    private List<SelectableObject> activeSelectables = new();

    public List<SelectableObject> ActiveSelectables => activeSelectables;

    public void Initialize(IARHitProvider hitProvider, UIHandler handler)
    {
        planeHitProvider = hitProvider;
        uiHandler = handler;

        planeHitProvider.OnARRaycastHit += OnPlaneHitAction;
        objectsPool = new ObjectPool<SelectableObject>(() =>
            SurfacedObjectsFactory.Create(selectablePrefab, parentForSpawned), initialPoolSize);
    }

    public void OnDestroy()
    {
        planeHitProvider.OnARRaycastHit -= OnPlaneHitAction;
    }

    private void OnPlaneHitAction(ARRaycastHit hitInfo)
    {
        var plane = hitInfo.trackable as ARPlane;
        if (plane == null || !IsCorrectTypeOfPlane(plane, spawnOnVertical))
            return;

        var newObject = GetNewObjectFromPool(hitInfo.pose.position, plane.normal);
        CheckActiveAmount();
    }

    private SelectableObject GetNewObjectFromPool(Vector3 position, Vector3 normal)
    {
        var newObject = objectsPool.GetObjectFromPool();
        newObject.gameObject.PresetSelectableActive(position, normal);
        newObject.OnDeletionTriggered += DestroyPoolObject;
        activeSelectables.Add(newObject);
        return newObject;
    }

    private void DestroyPoolObject(SelectableObject poolObject)
    {
        if (activeSelectables.Contains(poolObject))
        {
            poolObject.gameObject.PresetSelectableNonActive(parentForSpawned);
            poolObject.ResetState();
            poolObject.OnDeletionTriggered -= DestroyPoolObject;
            objectsPool.ReturnObjectToPool(poolObject);
            activeSelectables.Remove(poolObject);
        }
        else
        {
            Destroy(poolObject.gameObject);
        }

        CheckActiveAmount();
    }

    private void CheckActiveAmount()
    {
        uiHandler.SetControlsActiveState(activeSelectables.Count > 0);
    }

    private static bool IsCorrectTypeOfPlane(ARPlane plane, bool isVerticalAllowed) =>
        isVerticalAllowed || plane.alignment == PlaneAlignment.HorizontalUp;
}