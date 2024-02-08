using UnityEngine;

public class MainGameInstaller : MonoBehaviour
{
    [SerializeField] private SceneInteractionHandler interactionHandler;
    [SerializeField] private SpawnedObjectsHandler objectsHandler;

    private void Awake()
    {
        objectsHandler.Initialize(interactionHandler, new SurfacedSpawner());
    }
}