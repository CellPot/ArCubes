using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SpawnedObjectsHandler : MonoBehaviour
{
    [SerializeField] private GameObject spawnablePrefab; 
    
    private IARHitProvider _planeHitProvider;
    private SurfacedSpawner _surfacedSpawner;

    public void Initialize(IARHitProvider hitProvider, SurfacedSpawner spawner)
    {
        _planeHitProvider = hitProvider;
        _surfacedSpawner = spawner;
        _planeHitProvider.OnARRaycastHit += OnPlaneHitAction;
        _surfacedSpawner.OnObjectSpawned += OnSpawned;
    }

    private void OnPlaneHitAction(ARRaycastHit hitInfo)
    {
        _surfacedSpawner.TrySpawn(hitInfo,spawnablePrefab);
    }

    private void OnSpawned(GameObject spawned)
    {
        
    }

    public void OnDestroy()
    {
        _planeHitProvider.OnARRaycastHit -= OnPlaneHitAction;
        _surfacedSpawner.OnObjectSpawned -= OnSpawned;
    }
}