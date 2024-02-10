using TMPro;
using UnityEngine;

namespace UI
{
    public class UIHandler : MonoBehaviour
    {
        [SerializeField] private StickUI stickUI;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private CanvasHitDetector hitDetector;

        public bool IsInteractorOverUI => hitDetector.IsPointerOverUI();

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