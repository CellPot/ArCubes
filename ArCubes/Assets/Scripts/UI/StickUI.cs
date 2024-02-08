using UnityEngine;
using UnityEngine.InputSystem.OnScreen;

namespace UI
{
    public class StickUI : MonoBehaviour
    {
        [SerializeField] private OnScreenStick movementStick;
        private RectTransform stickRectTransform;

        public bool IsInputFocusedOnElements => stickRectTransform.anchoredPosition != Vector2.zero;

        private void Awake()
        {
            stickRectTransform = movementStick.GetComponent<RectTransform>();
        }
    }
}