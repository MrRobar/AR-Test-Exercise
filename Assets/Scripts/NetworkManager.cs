using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    [SerializeField] private GameObject _noConnectionScreen;
    private bool _noConnection = false;

    private void Update()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable && !_noConnection)
        {
            _noConnectionScreen.SetActive(true);
            _noConnection = true;
        }
    }
}