using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Selectable
{
    public class SelectableObject : MonoBehaviour
    {
        [SerializeField] private Rigidbody rBody;
        [SerializeField] private XRGrabInteractable interactable;
        [SerializeField] private float movementSpeed = 5f;

        public XRGrabInteractable Interactable => interactable;

        private void Awake()
        {
            if (rBody == null)
                rBody = GetComponent<Rigidbody>();
        }

        public void SetMovementSpeed(float value)
        {
            movementSpeed = value;
        }

        public void Move(Vector3 movement)
        {
            rBody.MovePosition(transform.position + (movement * movementSpeed));
        }
    }
}
