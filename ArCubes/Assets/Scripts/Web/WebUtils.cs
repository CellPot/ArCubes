using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Web
{
    public static class WebUtils
    {
        public static IEnumerator LoadImageFromURL(string url, Action<Texture2D> callback)
        {
            var www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();

            if (www.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
                callback.Invoke(null);
            }
            else
            {
                var texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                callback.Invoke(texture);
            }
        }
    }
}