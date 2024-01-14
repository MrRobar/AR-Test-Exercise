using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private ARSessionSwitcher _arSessionSwitcher;
    [SerializeField] private GameObject _initialScreen;

    private void OnEnable()
    {
        _arSessionSwitcher.OnMarkersAreTrue += DisableInitialScreen;
    }

    private void OnDisable()
    {
        _arSessionSwitcher.OnMarkersAreTrue -= DisableInitialScreen;
    }

    private void DisableInitialScreen()
    {
        _initialScreen.SetActive(false);
    }
}