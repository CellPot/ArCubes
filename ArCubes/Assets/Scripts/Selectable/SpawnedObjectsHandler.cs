using System.Collections.Generic;
using Collectable;
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
    private CollectablesHandler collectableHandler;

    private ObjectPool<SelectableObject> objectsPool;
    private List<SelectableObject> activeSelectables = new();

    public List<SelectableObject> ActiveSelectables => activeSelectables;

    public void Initialize(IARHitProvider hitProvider, UIHandler uiHandler, CollectablesHandler collectableHandler)
    {
        planeHitProvider = hitProvider;
        this.uiHandler = uiHandler;
        this.collectableHandler = collectableHandler;

        planeHitProvider.OnARRaycastHit += OnPlaneHitAction;
        objectsPool = new ObjectPool<SelectableObject>(() =>
            ObjectsFactory.Create(selectablePrefab, parentForSpawned), initialPoolSize);
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
        activeSelectables.Add(newObject);
        SpawnCollectables(newObject.transform.position);
        CheckActiveAmount();
    }

    private SelectableObject GetNewObjectFromPool(Vector3 position, Vector3 normal)
    {
        var newObject = objectsPool.GetObjectFromPool();
        newObject.gameObject.PresetSpawnedActive(position, normal);
        newObject.OnDeletionTriggered += DestroyPoolObject;
        return newObject;
    }

    private void DestroyPoolObject(SelectableObject poolObject)
    {
        if (activeSelectables.Contains(poolObject))
        {
            poolObject.gameObject.PresetSpawnedNonActive(parentForSpawned);
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

    private void SpawnCollectables(Vector3 position)
    {
        for (var i = 0; i < collectableHandler.CollectablesPerInteractable; i++)
        {
            var point = SpawnUtility.GetRandomPointInAnnulus(new Vector2(position.x,
                position.z), 1.5f, 3f);
            collectableHandler.SpawnObject(new Vector3(point.x, position.y, point.y), Vector3.up);
        }
    }

    private void CheckActiveAmount()
    {
        uiHandler.SetControlsActiveState(activeSelectables.Count > 0);
    }

    private static bool IsCorrectTypeOfPlane(ARPlane plane, bool isVerticalAllowed) =>
        isVerticalAllowed || plane.alignment == PlaneAlignment.HorizontalUp;
}