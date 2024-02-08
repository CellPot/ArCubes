using UnityEngine;
using UnityEngine.InputSystem.OnScreen;

namespace UI
{
    public class UIHandler : MonoBehaviour
    {
        [SerializeField] private OnScreenStick movementStick;

        private RectTransform stickRectTransform;

        public bool IsInputFocusedOnUI => stickRectTransform.anchoredPosition != Vector2.zero;

        private void Awake()
        {
            stickRectTransform = movementStick.GetComponent<RectTransform>();
        }
    }
}