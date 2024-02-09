using UnityEngine;
using Object = UnityEngine.Object;

namespace Common.Objects
{
    public static class ObjectsFactory
    {
        public static T Create<T>(T prefab, Transform parent) where T : MonoBehaviour
        {
            var newObject = Object.Instantiate(prefab.gameObject, parent);
            newObject.SetActive(false);

            return newObject.GetComponent<T>();
        }
    }
}