using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.Utilities;
using Object = UnityEngine.Object;

public class SurfacedSpawner
{
    public event Action<GameObject> OnObjectSpawned;

    public bool TrySpawn(ARRaycastHit hit, GameObject prefab, bool isVerticalAllowed = true)
    {
        var plane = hit.trackable as ARPlane;
        if (plane == null || !IsCorrectTypeOfPlane(plane, isVerticalAllowed))
            return false;
        
        var newObject = Object.Instantiate(prefab);
        newObject.transform.position = hit.pose.position;
        BurstMathUtility.ProjectOnPlane(newObject.transform.forward, plane.normal, out var projectedForward);
        newObject.transform.rotation = Quaternion.LookRotation(projectedForward, plane.normal);
        OnObjectSpawned?.Invoke(newObject);

        return true;
    }

    private static bool IsCorrectTypeOfPlane(ARPlane plane, bool isVerticalAllowed) =>
        isVerticalAllowed || plane.alignment == PlaneAlignment.HorizontalUp;
}