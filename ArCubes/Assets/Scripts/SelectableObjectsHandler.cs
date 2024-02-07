using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SelectableObjectsHandler : IDisposable
{
    private readonly IPlaneHitProvider planeHitProvider;

    public SelectableObjectsHandler(IPlaneHitProvider planeHitProvider)
    {
        this.planeHitProvider = planeHitProvider;
        this.planeHitProvider.OnPlaneRaycastHit += OnPlaneHitAction;
    }

    private void OnPlaneHitAction(ARPlane plane)
    {
        Debug.Log("SPAWNER CALL TO SPAWN");
    }

    public void Dispose()
    {
        planeHitProvider.OnPlaneRaycastHit -= OnPlaneHitAction;
    }
}