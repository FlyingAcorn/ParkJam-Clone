using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public Camera mainCamera;
    public LayerMask mask;
    private Vector3 _carDirection;
    private Vector3 _startTouchPosition;
    public CarMovement selectedCar;

    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        TouchInput();
    }

//TODO: Add swipe sensitivity to the touch input.
    private void TouchInput()
    {
        
        if (Input.touchCount != 1 || GameManager.Instance.state != GameManager.GameState.Play) return;
        var touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            Ray ray = mainCamera.ScreenPointToRay(touch.position);
            _startTouchPosition = new Vector3(touch.position.x, 0, touch.position.y);
            if (Physics.Raycast(ray, out var hit, 100, mask))
            {
                selectedCar = hit.transform.GetComponent<CarMovement>();
            }
        }

        if (touch.phase != TouchPhase.Ended || selectedCar == null) return;
        var endTouchPos = new Vector3(touch.position.x, 0, touch.position.y);
        var swipeDirection = endTouchPos - _startTouchPosition;
        if (Mathf.Abs(swipeDirection.z) > Mathf.Abs(swipeDirection.x))
            // depending on the cameras rotation z and x swaps positions to align the swipe direction.
        {
            _carDirection = swipeDirection.x > 0 ? Vector3.right : Vector3.left;
        }
        else
        {
            _carDirection = swipeDirection.z > 0 ? new Vector3(0, 0, 1) : new Vector3(0, 0, -1);
        }

        selectedCar.MovementController(_carDirection);
        selectedCar = null;
    }
}