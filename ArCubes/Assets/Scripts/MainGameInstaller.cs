using UnityEngine;

public class MainGameInstaller : MonoBehaviour
{
    [SerializeField] private SceneInteractionHandler interactionHandler;

    private void Awake()
    {
        var objectsHandler = new SelectableObjectsHandler(interactionHandler);
    }
}