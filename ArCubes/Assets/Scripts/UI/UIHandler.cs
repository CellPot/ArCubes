using TMPro;
using UnityEngine;

namespace UI
{
    public class UIHandler : MonoBehaviour
    {
        [SerializeField] private StickUI stickUI;
        [SerializeField] private TMP_Text scoreText;

        public bool IsInputFocusedOnUI => stickUI.IsInputFocusedOnElements;

        private void Awake()
        {
            stickUI.SetActiveState(false);
        }

        public void SetControlsActiveState(bool state)
        {
            stickUI.SetActiveState(state);
        }

        public void SetScore(int score)
        {
            scoreText.text = score.ToString();
        }
    }
}