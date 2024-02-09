using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Web;

namespace Tracking
{
    public class TrackingHandler : MonoBehaviour
    {
        [SerializeField] private ARTrackedImageManager manager;
        [SerializeField] private GameObject prefab;

        private void Awake()
        {
            StartCoroutine(nameof(ImgLoadingRoutine));
        }

        private void OnEnable()
        {
            manager.trackedImagesChanged += OnImagesChanged;
        }

        private void OnDisable()
        {
            manager.trackedImagesChanged += OnImagesChanged;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        private IEnumerator ImgLoadingRoutine()
        {
            //TODO: move to config
            var url = "https://mix-ar.ru/content/ios/marker.jpg";
            yield return StartCoroutine(WebUtils.LoadImageFromURL(url,
                LoadOperationCallback));
        }

        private void LoadOperationCallback(Texture2D texture2D)
        {
            if (texture2D == null)
            {
                Debug.Log("Texture couldn't be loaded");
            }
            else
            {
                Debug.Log("Texture is loaded");
                AddImage(texture2D);
            }
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

        private void OnImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
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

            if (eventArgs.removed.Count > 0)
                Debug.Log($"Removed {eventArgs.removed.Count}");
        }
    }
}