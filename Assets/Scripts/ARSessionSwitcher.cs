using System;
using UnityEngine;

public class ARSessionSwitcher : MonoBehaviour
{
    [SerializeField] private FilesLoader _filesLoader;
    private bool _imageLoaded, _modelLoaded;

    public event Action OnMarkersAreTrue;

    private void OnEnable()
    {
        _filesLoader.OnImageProcessed += SetImageMarker;
        _filesLoader.OnModelDownloaded += SetModelMarker;
    }

    private void OnDisable()
    {
        _filesLoader.OnImageProcessed -= SetImageMarker;
        _filesLoader.OnModelDownloaded -= SetModelMarker;
    }

    private void SetImageMarker(Texture2D arg1, string arg2)
    {
        _imageLoaded = true;
        CheckMarkers();
    }

    private void SetModelMarker(GameObject obj)
    {
        _modelLoaded = true;
        CheckMarkers();
    }

    private void CheckMarkers()
    {
        if (_imageLoaded && _modelLoaded)
        {
            OnMarkersAreTrue?.Invoke();
        }
    }
}