using UnityEngine;
using UnityEngine.InputSystem;

namespace Selectable.Movement
{
    public class SelectableMovementHandler : MonoBehaviour
    {
        [SerializeField] private SpawnedObjectsHandler objectsHandler;
        [SerializeField] private InputActionReference actionReference;
        [SerializeField] private float movementSpeed = 1f;

        private Vector2 inputValue;

        private void Start()
        {
            actionReference.action.performed += OnInputPerformed;
            actionReference.action.canceled += OnInputCanceled;
        }

        private void OnDestroy()
        {
            actionReference.action.performed -= OnInputPerformed;
            actionReference.action.canceled -= OnInputCanceled;
        }

        private void OnInputPerformed(InputAction.CallbackContext inputCallback) =>
            UpdateInputValue(inputCallback);

        private void OnInputCanceled(InputAction.CallbackContext inputCallback) =>
            UpdateInputValue(inputCallback);

        private void UpdateInputValue(InputAction.CallbackContext inputCallback)
        {
            if (inputCallback.valueType == typeof(Vector2))
                inputValue = inputCallback.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            if (inputValue != Vector2.zero)
                MoveObjects();
        }

        private void MoveObjects()
        {
            var movement = new Vector3(inputValue.x, 0, inputValue.y).normalized * (Time.deltaTime * movementSpeed);
            foreach (var selectable in objectsHandler.ActiveSelectables)
                selectable.Move(movement);
        }
    }
}