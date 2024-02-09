using System.Collections.Generic;
using Audio;
using UI;
using UnityEngine;

namespace Collectable
{
    public class CollectablesHandler : MonoBehaviour
    {
        [SerializeField] private CollectableObject prefab;
        [SerializeField] private Transform parentForSpawned;
        [SerializeField] private int initialPoolSize = 15;
        [SerializeField] private int collectablesPerInteractable = 3;

        private UIHandler uiHandler;
        private AudioHandler audioHandler;
        private ObjectPool<CollectableObject> objectsPool;
        private List<CollectableObject> activeObjects = new();
        private int score;

        public int CollectablesPerInteractable => collectablesPerInteractable;

        public void Initialize(UIHandler uiHandler, AudioHandler audioHandler)
        {
            this.uiHandler = uiHandler;
            this.audioHandler = audioHandler;
            objectsPool = new ObjectPool<CollectableObject>(() =>
                ObjectsFactory.Create(prefab, parentForSpawned), initialPoolSize);
        }

        public void SpawnObject(Vector3 position, Vector3 normal)
        {
            var newObject = GetNewObjectFromPool(position, normal);
            activeObjects.Add(newObject);
        }

        private CollectableObject GetNewObjectFromPool(Vector3 position, Vector3 normal)
        {
            var newObject = objectsPool.GetObjectFromPool();
            newObject.gameObject.PresetSpawnedActive(position, normal);
            newObject.OnDeletionTriggered += DestroyPoolObject;
            return newObject;
        }

        private void DestroyPoolObject(CollectableObject poolObject)
        {
            if (activeObjects.Contains(poolObject))
            {
                objectsPool.ReturnObjectToPool(poolObject);
                activeObjects.Remove(poolObject);
                poolObject.gameObject.PresetSpawnedNonActive(parentForSpawned);
                poolObject.OnDeletionTriggered -= DestroyPoolObject;
                score++;
                HandleCoinAddition();
            }
            else
            {
                Destroy(poolObject.gameObject);
            }
        }

        private void HandleCoinAddition()
        {
            audioHandler.PlayCoinSound();
            uiHandler.SetScore(score);
        }
    }
}