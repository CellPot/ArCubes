using System;
using UI;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit;

namespace Interaction
{
    public class SceneInteractionHandler : MonoBehaviour, IARHitProvider
    {
        private XRBaseControllerInteractor controllerInteractor;
        private UIHandler uiHandler;

        private IARInteractor arInteractor;
        private bool hasFocusOnSelected;
        private bool selectActivatedOnUI;

        public event Action<ARRaycastHit> OnARRaycastHit;

        public void Initialize(XRBaseControllerInteractor controllerInteractor, IARInteractor arInteractor,
            UIHandler uiHandler)
        {
            this.controllerInteractor = controllerInteractor;
            this.arInteractor = arInteractor;
            this.uiHandler = uiHandler;
        }

        private void Update()
        {
            var spawnConditionsMet = GetSpawnConditionCompliance();

            if (spawnConditionsMet && arInteractor.TryGetCurrentARRaycastHit(out var raycastHit))
            {
                OnARRaycastHit?.Invoke(raycastHit);
            }
        }

        private bool GetSpawnConditionCompliance()
        {
            var shouldSpawn = false;
            if (IsSelectActivatedThisFrame())
            {
                hasFocusOnSelected = IsSelectionPresent();
                selectActivatedOnUI = uiHandler.IsInputFocusedOnUI;
            }
            else if (IsSelectActionActive())
            {
                hasFocusOnSelected = hasFocusOnSelected || IsSelectionPresent();
            }
            else if (IsSelectDeactivatedThisFrame())
            {
                shouldSpawn = !IsSelectionPresent() && !hasFocusOnSelected && !selectActivatedOnUI;
                selectActivatedOnUI = false;
            }

            return shouldSpawn;
        }

        private bool IsSelectActionActive() =>
            controllerInteractor.xrController.currentControllerState.selectInteractionState.active;

        private bool IsSelectDeactivatedThisFrame() =>
            controllerInteractor.xrController.currentControllerState.selectInteractionState.deactivatedThisFrame;

        private bool IsSelectActivatedThisFrame() =>
            controllerInteractor.xrController.currentControllerState.selectInteractionState.activatedThisFrame;

        private bool IsSelectionPresent() =>
            controllerInteractor.hasSelection;
    }


    public interface IARHitProvider
    {
        event Action<ARRaycastHit> OnARRaycastHit;
    }
}