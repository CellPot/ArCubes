using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Utilities;

namespace Common.Objects
{
    public static class SpawnUtility
    {
        public static void PresetSpawnedActive(this GameObject newObject, Vector3 position, Vector3 normal)
        {
            newObject.transform.position = position;
            BurstMathUtility.ProjectOnPlane(newObject.transform.forward, normal, out var projectedForward);
            newObject.transform.rotation = projectedForward != Vector3.zero
                ? Quaternion.LookRotation(projectedForward, normal)
                : Quaternion.identity;
            newObject.SetActive(true);
        }

        public static void PresetSpawnedNonActive(this GameObject newObject, Transform parent)
        {
            newObject.SetActive(false);
            newObject.transform.SetParent(parent);
        }

        public static Vector2 GetRandomPointInAnnulus(Vector2 origin, float minRadius, float maxRadius)
        {
            var randomDirection = Random.insideUnitCircle.normalized;
            var minRadius2 = minRadius * minRadius;
            var maxRadius2 = maxRadius * maxRadius;
            var randomDistance = Mathf.Sqrt(Random.value * (maxRadius2 - minRadius2) + minRadius2);
            return origin + randomDirection * randomDistance;
        }
    }
}