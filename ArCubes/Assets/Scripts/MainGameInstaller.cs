using Audio;
using Collectable;
using Interaction;
using Selectable;
using Selectable.Movement;
using Tracking;
using UI;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MainGameInstaller : MonoBehaviour
{
    [SerializeField] private SceneInteractionHandler interactionHandler;
    [SerializeField] private SpawnedObjectsHandler objectsHandler;
    [SerializeField] private CollectablesHandler collectablesHandler;
    [SerializeField] private ImageTrackingHandler imgTrackingHandler;
    [SerializeField] private SelectableMovementHandler movementHandler;
    [SerializeField] private AudioHandler audioHandler;

    [SerializeField] private XRBaseControllerInteractor controllerInteractor;
    [SerializeField] private UIHandler uiHandler;

    private void Awake()
    {
        interactionHandler.Initialize(controllerInteractor, controllerInteractor as IARInteractor, uiHandler);
        collectablesHandler.Initialize(uiHandler, audioHandler);
        objectsHandler.Initialize(interactionHandler, uiHandler, collectablesHandler);
        imgTrackingHandler.Initialize(movementHandler);
    }
}