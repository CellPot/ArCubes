using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    public class ImageProvider : MonoBehaviour
    {
        public event Action<Texture2D> OnTextureLoaded;

        public IEnumerator LoadImageFromURL(string url)
        {
            var www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();

            if (www.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                var texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                FinishLoading(texture);
            }
        }

        private void FinishLoading(Texture2D texture)
        {
            OnTextureLoaded?.Invoke(texture);
        }
    }
}
