using UI;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MainGameInstaller : MonoBehaviour
{
    [SerializeField] private SceneInteractionHandler interactionHandler;
    [SerializeField] private SpawnedObjectsHandler objectsHandler;
    [SerializeField] private XRBaseControllerInteractor controllerInteractor;
    [SerializeField] private UIHandler uiHandler;

    private void Awake()
    {
        interactionHandler.Initialize(controllerInteractor, controllerInteractor as IARInteractor, uiHandler);
        objectsHandler.Initialize(interactionHandler, uiHandler);
    }
}