using System;
using Selectable;
using UnityEngine;

namespace Collectable
{
    public class CollectableObject : MonoBehaviour, IDeletable<CollectableObject>
    {
        [SerializeField] private float rotationSpeed = 100f;

        public event Action<CollectableObject> OnDeletionTriggered;

        public void Remove()
        {
            OnDeletionTriggered?.Invoke(this);
        }
        private void Update()
        {
            transform.Rotate(transform.up, rotationSpeed * Time.deltaTime);
        }
    }
}