using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Tracking
{
    public class TrackingHandler : MonoBehaviour
    {
        [SerializeField] private ARTrackedImageManager manager;
        [SerializeField] private XRReferenceImageLibrary library;
        [SerializeField] private GameObject prefab;
        [SerializeField] private Texture2D tempImg;
        private AddReferenceImageJobState _job;
        
        void OnEnable()
        {
            manager.trackedImagesChanged += ImagesChanged;
            Invoke(nameof(TempLoadMethod),10f);
        }

        private void TempLoadMethod()
        {
            AddImage(tempImg);
        }
        private void OnDisable()
        {
            manager.trackedImagesChanged += ImagesChanged;
        }
        
        private void AddImage(Texture2D imageToAdd)
        {
            if (!DoesSupportMutableImageLibraries()) return;
            
            if (manager.referenceLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary)
            {
                _job = mutableLibrary.ScheduleAddImageWithValidationJob(
                    imageToAdd,
                    "New Image", 0.1f);
            }
        }

        private bool DoesSupportMutableImageLibraries() => manager.descriptor.supportsMutableLibrary;

        private void ImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            if (eventArgs.added.Count > 0)
            {
                Debug.Log($"Added {eventArgs.added.Count}");
                foreach (var trackedImage in eventArgs.added)
                {
                    Instantiate(prefab, trackedImage.transform);
                }
            }

            if (eventArgs.updated.Count > 0)
            {
                Debug.Log($"Updated {eventArgs.updated.Count}");
                foreach (var trackedImage in eventArgs.updated)
                {
                    
                }
            }

            if (eventArgs.removed.Count >0)
                Debug.Log($"Removed {eventArgs.removed.Count}");
        }
    }
}