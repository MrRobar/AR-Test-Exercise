using UnityEngine;

public class ARObjectController : MonoBehaviour
{
    [SerializeField] private FilesLoader _filesLoader;
    private GameObject _objectToInteract;
    private Touch _touch0;
    private Touch _touch1;
    private float _rotationSpeed = 0.1f;
    private Vector2 _touchStartPos1, _touchStartPos2;
    private Vector3 _initialScale;
    private float _initialDistance;

    private void OnEnable()
    {
        _filesLoader.OnModelDownloaded += UpdateObject;
    }

    private void OnDisable()
    {
        _filesLoader.OnModelDownloaded -= UpdateObject;
    }

    private void Update()
    {
        if (_objectToInteract == null)
        {
            return;
        }

        HandleInteraction();
    }

    private void UpdateObject(GameObject obj)
    {
        _objectToInteract = obj;
        _initialScale = _objectToInteract.transform.localScale;
    }

    private void HandleInteraction()
    {
        if (Input.touchCount == 1)
        {
            _touch0 = Input.GetTouch(0);
            if (_touch0.phase == TouchPhase.Moved)
            {
                _objectToInteract.transform.Rotate(0f, -_touch0.deltaPosition.x * _rotationSpeed, 0f,
                    Space.Self);
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Canceled ||
                touch2.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Canceled)
            {
                return;
            }

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                _initialDistance = Vector3.Distance(touch1.position, touch2.position);
                _initialScale = _objectToInteract.transform.localScale;
            }
            else
            {
                var currentDistance = Vector3.Distance(touch1.position, touch2.position);
                if (Mathf.Approximately(_initialDistance, 0))
                {
                    return;
                }

                var factor = currentDistance / _initialDistance;
                _objectToInteract.transform.localScale = _initialScale * factor;
            }
        }
    }
}