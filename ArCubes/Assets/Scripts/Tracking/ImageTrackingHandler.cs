using System.Collections;
using Common.Application;
using Common.Data;
using Selectable.Movement;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Web;

namespace Tracking
{
    public class ImageTrackingHandler : MonoBehaviour
    {
        [SerializeField] private ARTrackedImageManager manager;
        [SerializeField] private GameObject prefab;

        private SelectableMovementHandler movementHandler;
        private WebData webData;

        public void Initialize(SelectableMovementHandler movementHandler)
        {
            this.movementHandler = movementHandler;
            StartImageLoading();
        }

        private void StartImageLoading()
        {
            var loadResult = DataIOUtils<WebData>.LoadFromJson(ApplicationPath.WebDataPath, out webData);
            if (loadResult)
                StartCoroutine(nameof(ImgLoadingRoutine));
            else
                Debug.LogError("WebData path does not exist");
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
            if (webData == null)
                yield break;

            yield return StartCoroutine(WebUtils.LoadImageFromURL(webData.MarkerUrl,
                LoadOperationCallback));
        }

        private void LoadOperationCallback(Texture2D texture2D)
        {
            if (texture2D != null)
            {
                Debug.Log("Texture is loaded");
                AddImage(texture2D);
            }
            else
            {
                Debug.Log("Texture couldn't be loaded");
            }
        }

        private void AddImage(Texture2D imageToAdd)
        {
            if (!DoesSupportMutableImageLibraries()) return;

            if (manager.referenceLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary)
            {
                mutableLibrary.ScheduleAddImageWithValidationJob(
                    imageToAdd,
                    "Loaded Image", 0.1f);
            }
        }

        private bool DoesSupportMutableImageLibraries() => manager.descriptor.supportsMutableLibrary;

        private void OnImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            if (eventArgs.added.Count > 0)
            {
                foreach (var trackedImage in eventArgs.added)
                    Instantiate(prefab, trackedImage.transform);
            }

            movementHandler.SetSpeedMod(manager.trackables.count > 0 ? 2f : 1f);
        }
    }
}