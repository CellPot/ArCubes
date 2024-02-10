using System;
using Collectable;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Selectable
{
    public class SelectableObject : MonoBehaviour, IDeletable<SelectableObject>
    {
        [SerializeField] private Rigidbody rBody;
        [SerializeField] private XRGrabInteractable interactable;
        [SerializeField] private SelectableVisuals visuals;

        private const string CollectableTag = "Collectable";
        private const float SelectionTimeToDelete = 1f;
        private bool isDeletionTriggered;
        private float selectionTimer;

        public event Action<SelectableObject> OnDeletionTriggered;
        public event Action<CollectableObject> OnCollectableHit;

        public void Move(Vector3 movement)
        {
            rBody.MovePosition(transform.position + movement);
        }

        public void ResetState()
        {
            isDeletionTriggered = false;
            selectionTimer = 0;
            visuals.ResetMaterial();
        }

        private void Awake()
        {
            if (rBody == null)
                rBody = GetComponent<Rigidbody>();
            interactable.selectEntered?.AddListener(OnSelectEntered);
            interactable.selectExited?.AddListener(OnSelectExited);
        }

        private void Update()
        {
            UpdateSelectionTimer();
        }

        private void UpdateSelectionTimer()
        {
            if (interactable.isSelected)
            {
                selectionTimer += Time.deltaTime;
                CheckSelectionTime(selectionTimer);
            }
            else
            {
                selectionTimer = 0;
            }
        }

        private void OnDestroy()
        {
            interactable.selectEntered?.RemoveListener(OnSelectEntered);
            interactable.selectExited?.RemoveListener(OnSelectExited);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(CollectableTag))
            {
                if (other.TryGetComponent<CollectableObject>(out var collectable))
                {
                    OnCollectableHit?.Invoke(collectable);
                    collectable.Remove();
                }
            }
        }

        private void CheckSelectionTime(float time)
        {
            if (time >= SelectionTimeToDelete && !isDeletionTriggered)
            {
                isDeletionTriggered = true;
                OnDeletionTriggered?.Invoke(this);
            }
        }

        private void OnSelectEntered(SelectEnterEventArgs arg0)
        {
            visuals.SwitchColor();
        }

        private void OnSelectExited(SelectExitEventArgs selectExitEventArgs)
        {
            visuals.SwitchColor();
        }
    }
}