using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Utilities;

public static class SelectableUtility
{
    public static void PresetSelectableActive(this GameObject newObject, Vector3 position, Vector3 normal)
    {
        newObject.transform.position = position;
        BurstMathUtility.ProjectOnPlane(newObject.transform.forward, normal, out var projectedForward);
        newObject.transform.rotation = Quaternion.LookRotation(projectedForward, normal);
        newObject.SetActive(true);
    }

    public static void PresetSelectableNonActive(this GameObject newObject, Transform parent)
    {
        newObject.SetActive(false);
        newObject.transform.SetParent(parent);
    }
}