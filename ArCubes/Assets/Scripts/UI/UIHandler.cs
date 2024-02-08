using UnityEngine;

namespace UI
{
    public class UIHandler : MonoBehaviour
    {
        [SerializeField] private StickUI stickUI;

        public bool IsInputFocusedOnUI => stickUI.gameObject.activeSelf && stickUI.IsInputFocusedOnElements;

        private void Awake()
        {
            stickUI.SetActiveState(false);
        }

        public void SetControlsActiveState(bool state)
        {
            stickUI.SetActiveState(state);
        }
    }
}