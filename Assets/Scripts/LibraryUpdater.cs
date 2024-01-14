using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class LibraryUpdater : MonoBehaviour
{
    [SerializeField] private FilesLoader _filesLoader;
    [SerializeField] private XRReferenceImageLibrary _xrReferenceLibrary;
    [SerializeField] private ARTrackedImageManager _manager;

    private void Start()
    {
        _manager.referenceLibrary = _manager.CreateRuntimeLibrary(_xrReferenceLibrary);
        _manager.enabled = true;
    }

    private void OnEnable()
    {
        _filesLoader.OnImageProcessed += UpdateLibrary;
    }

    private void OnDisable()
    {
        _filesLoader.OnImageProcessed -= UpdateLibrary;
    }

    private void UpdateLibrary(Texture2D texture2D, string imageName)
    {
        StartCoroutine(AddImageJob(texture2D, imageName));
    }

    private IEnumerator AddImageJob(Texture2D texture, string imageName)
    {
        yield return null;

        try
        {
            MutableRuntimeReferenceImageLibrary mutable =
                _manager.referenceLibrary as MutableRuntimeReferenceImageLibrary;
            var jobHandle = mutable.ScheduleAddImageJob(texture, imageName, 0.1f);
            while (!jobHandle.IsCompleted)
            {
                Debug.Log("Job is running...");
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}