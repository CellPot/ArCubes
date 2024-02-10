using System.IO;
using UnityEngine;

namespace Common.Data
{
    public static class DataIOUtils<T>
    {
        public static bool LoadFromJson(string path, out T data)
        {
            data = default(T);
            var filePath = Path.Combine(UnityEngine.Application.streamingAssetsPath, path);
            if (File.Exists(filePath))
            {
                var jsonString = File.ReadAllText(filePath);
                data = JsonUtility.FromJson<T>(jsonString);
                return true;
            }

            return false;
        }

        public static void WriteToJson(string path, T data)
        {
            var jsonData = JsonUtility.ToJson(data, true);
            var filePath = Path.Combine(UnityEngine.Application.streamingAssetsPath, path);
            File.WriteAllText(filePath, jsonData);
        }
    }
}