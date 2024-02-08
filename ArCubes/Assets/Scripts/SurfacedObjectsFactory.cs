using UnityEngine;
using Object = UnityEngine.Object;

public static class SurfacedObjectsFactory
{
    public static T Create<T>(T prefab, Transform parent) where T : MonoBehaviour
    {
        var newObject = Object.Instantiate(prefab.gameObject, parent);
        newObject.SetActive(false);

        return newObject.GetComponent<T>();
    }
}