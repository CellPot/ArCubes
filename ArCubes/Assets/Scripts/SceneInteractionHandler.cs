using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit;

public class SceneInteractionHandler : MonoBehaviour, IPlaneHitProvider
{
    public event Action<ARPlane> OnPlaneRaycastHit;

    [SerializeField] private XRBaseControllerInteractor controllerInteractor;
    [SerializeField] private bool spawnOnlyOnHorizontalUp;

    private IARInteractor arInteractor;
    private bool hasFocusOnSelected;

    private void Awake()
    {
        arInteractor = controllerInteractor as IARInteractor;
    }

    private void Update()
    {
        var spawnConditionsMet = GetSpawnConditionCompliance();

        if (spawnConditionsMet && arInteractor.TryGetCurrentARRaycastHit(out var raycastHit))
        {
            var plane = raycastHit.trackable as ARPlane;
            if (plane == null)
                return;
            OnPlaneRaycastHit?.Invoke(plane);
            if (!IsCorrectTypeOfPlane(plane))
                return;

            Debug.Log("SPAWN");
            // m_ObjectSpawner.TrySpawnObject(arRaycastHit.pose.position, arPlane.normal);
        }
    }


    private bool GetSpawnConditionCompliance()
    {
        var shouldSpawn = false;
        if (IsSelectActivatedThisFrame())
            hasFocusOnSelected = IsSelectionPresent();
        else if (IsSelectActionActive())
            hasFocusOnSelected = hasFocusOnSelected || IsSelectionPresent();
        else if (IsSelectDeactivatedThisFrame())
            shouldSpawn = !IsSelectionPresent() && !hasFocusOnSelected;

        return shouldSpawn;
    }

    private bool IsCorrectTypeOfPlane(ARPlane plane) =>
        !spawnOnlyOnHorizontalUp || plane.alignment == PlaneAlignment.HorizontalUp;

    private bool IsSelectActionActive() =>
        controllerInteractor.xrController.currentControllerState.selectInteractionState.active;

    private bool IsSelectDeactivatedThisFrame() =>
        controllerInteractor.xrController.currentControllerState.selectInteractionState.deactivatedThisFrame;

    private bool IsSelectActivatedThisFrame() =>
        controllerInteractor.xrController.currentControllerState.selectInteractionState.activatedThisFrame;

    private bool IsSelectionPresent() =>
        controllerInteractor.hasSelection;
}

public interface IPlaneHitProvider
{
    event Action<ARPlane> OnPlaneRaycastHit;
}