using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Selectable
{
    public class SelectableObject : MonoBehaviour
    {
        [SerializeField] private Rigidbody rBody;
        [SerializeField] private XRGrabInteractable interactable;
        [SerializeField] private SelectableVisuals visuals;

        private const float SelectionTimeToDelete = 0.65f;
        private bool isDeletionTriggered;
        private float selectionTimer;

        public event Action<SelectableObject> OnDeletionTriggered;
        public XRGrabInteractable Interactable => interactable;

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
        }

        private void Update()
        {
            if (interactable.isSelected)
            {
                selectionTimer += Time.deltaTime;
                CheckSelectionTime(selectionTimer);
            }
            else
                selectionTimer = 0;
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

        private void OnDestroy()
        {
            interactable.selectEntered?.RemoveListener(OnSelectEntered);
        }
    }
}