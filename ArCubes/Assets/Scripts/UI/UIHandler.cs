using UnityEngine;

namespace UI
{
    public class UIHandler : MonoBehaviour
    {
        [SerializeField] private StickUI stickUI;

        public bool IsInputFocusedOnUI => stickUI.gameObject.activeSelf && stickUI.IsInputFocusedOnElements;

        public void SetControlsActiveState(bool state)
        {
            stickUI.gameObject.SetActive(state);
        }
    }
}