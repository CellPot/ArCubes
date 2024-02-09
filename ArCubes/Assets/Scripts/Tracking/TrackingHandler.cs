using System.Collections;
using Network;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Tracking
{
    public class TrackingHandler : MonoBehaviour
    {
        [SerializeField] private ARTrackedImageManager manager;
        [SerializeField] private ImageProvider provider;
        [SerializeField] private GameObject prefab;

        private void Awake()
        {
            StartCoroutine(nameof(ImgLoadingRoutine));
            provider.OnTextureLoaded += OnLoaded;
        }

        private IEnumerator ImgLoadingRoutine()
        {
            //TODO: move to config
            yield return StartCoroutine(provider.LoadImageFromURL("https://mix-ar.ru/content/ios/marker.jpg"));
        }

        private void OnLoaded(Texture2D obj)
        {
            AddImage(obj);
        }

        void OnEnable()
        {
            manager.trackedImagesChanged += ImagesChanged;
        }
        private void OnDisable()
        {
            manager.trackedImagesChanged += ImagesChanged;
        }
        
        public void AddImage(Texture2D imageToAdd)
        {
            if (!DoesSupportMutableImageLibraries()) return;
            
            if (manager.referenceLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary)
            {
                mutableLibrary.ScheduleAddImageWithValidationJob(
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