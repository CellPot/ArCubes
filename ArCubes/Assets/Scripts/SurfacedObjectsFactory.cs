using UnityEngine;
using Object = UnityEngine.Object;

public static class SurfacedObjectsFactory
{
    public static GameObject Create(GameObject prefab, Transform parent)
    {
        var newObject = Object.Instantiate(prefab, parent);
        newObject.SetActive(false);

        return newObject;
    }
}