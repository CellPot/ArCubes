using UnityEngine;
using UnityEngine.InputSystem;

namespace Selectable.Movement
{
    public class SelectableMovementHandler : MonoBehaviour
    {
        [SerializeField] private SpawnedObjectsHandler objectsHandler;
        [SerializeField] private InputActionReference actionReference;
        [SerializeField] private float initialSpeed = 1f;

        private float movementSpeed = 1f;
        private Vector2 inputValue;

        public void SetSpeedMod(float value)
        {
            movementSpeed = initialSpeed * value;
        }

        private void Awake()
        {
            actionReference.action.performed += OnInputPerformed;
            actionReference.action.canceled += OnInputCanceled;
            SetSpeedMod(1);
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
            MoveObjects();
        }

        private void MoveObjects()
        {
            var movement = new Vector3(inputValue.x, 0, inputValue.y) * (Time.deltaTime * movementSpeed);
            foreach (var selectable in objectsHandler.ActiveSelectables)
                selectable.Move(movement);
        }
    }
}