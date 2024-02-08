using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

namespace UI
{
    public class StickUI : MonoBehaviour
    {
        [SerializeField] private OnScreenStick movementStick;
        [SerializeField] private Image image;
        private RectTransform stickRectTransform;

        public bool IsInputFocusedOnElements => stickRectTransform.anchoredPosition != Vector2.zero;

        public void SetActiveState(bool state)
        {
            image.enabled = state;
        }
        private void Awake()
        {
            stickRectTransform = movementStick.GetComponent<RectTransform>();
        }
    }
}