using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TrackedImagePlacer : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager _arTrackedImageManager;
    [SerializeField] private FilesLoader _filesLoader;
    private GameObject _prefabToSpawn;

    private void OnEnable()
    {
        _arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        _filesLoader.OnModelDownloaded += UpdatePrefab;
    }

    private void OnDisable()
    {
        _arTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        _filesLoader.OnModelDownloaded -= UpdatePrefab;
    }

    private void UpdatePrefab(GameObject obj)
    {
        _prefabToSpawn = obj;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs obj)
    {
        foreach (var trackedImage in obj.added)
        {
            _prefabToSpawn.SetActive(true);
            _prefabToSpawn.transform.parent = trackedImage.transform;
            _prefabToSpawn.transform.position = Vector3.zero;
            _prefabToSpawn.transform.localRotation = Quaternion.identity;
        }

        foreach (var trackedImage in obj.updated)
        {
            _prefabToSpawn.SetActive(trackedImage.trackingState == TrackingState.Tracking);
        }

        foreach (var trackedImage in obj.removed)
        {
            _prefabToSpawn.SetActive(false);
        }
    }
}