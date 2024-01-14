using System;
using System.Collections;
using GLTFast;
using UnityEngine;
using UnityEngine.Networking;

public class FilesLoader : MonoBehaviour
{
    [SerializeField] private string _modelURL;
    [SerializeField] private string _imageURL;
    public event Action<Texture2D, string> OnImageProcessed;
    public event Action<GameObject> OnModelDownloaded;

    private void Start()
    {
        StartCoroutine(LoadData(_imageURL, FileType.Image));
        StartCoroutine(LoadData(_modelURL, FileType.Model));
    }

    private IEnumerator LoadData(string url, FileType fileType)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                switch (fileType)
                {
                    case FileType.Image:
                        ProcessImage(webRequest.downloadHandler.data);
                        break;

                    case FileType.Model:
                        ProcessModel(webRequest.downloadHandler.data);
                        break;

                    default:
                        Debug.LogWarning("Unsupported file type");
                        break;
                }
            }
        }
    }

    private async void ProcessModel(byte[] downloadHandlerData)
    {
        var gltf = new GltfImport();
        bool success = await gltf.LoadGltfBinary(
            downloadHandlerData,
            new Uri(Application.streamingAssetsPath)
        );
        if (success)
        {
            var go = new GameObject("gltf");
            await gltf.InstantiateMainSceneAsync(go.transform);
            go.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            go.SetActive(false);
            OnModelDownloaded?.Invoke(go);
        }
    }

    private void ProcessImage(byte[] downloadHandlerData)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(downloadHandlerData);
        Uri uri = new Uri(_imageURL);
        var filename = System.IO.Path.GetFileName(uri.LocalPath);

        OnImageProcessed?.Invoke(texture, filename);
    }
}