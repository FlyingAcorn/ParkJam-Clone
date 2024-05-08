using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InputManager : MonoBehaviour
{
    private Camera _camera;
    public LayerMask mask;
    private Vector3 _carDirection;
    private Vector3 _startTouchPosition;
    [SerializeField] private CarMovement selectedCar;
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        TouchInput();
    }

    private void Movement()
    {
        
    }
//TODO: Add swipe sensitivity to the touch input.
    private void TouchInput()
    {
        if (Input.touchCount !=1) return;
        var touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            Ray ray = _camera.ScreenPointToRay(touch.position);
            _startTouchPosition = new Vector3(touch.position.x, 0, touch.position.y);
            if (Physics.Raycast(ray, out var hit, 100, mask))
            {
                selectedCar = hit.transform.GetComponent<CarMovement>();
                Debug.Log("deneme");
            }
        }
        
        if (touch.phase != TouchPhase.Ended || selectedCar == null ) return;
        var endTouchPos = new Vector3(touch.position.x, 0, touch.position.y);
        Debug.Log(endTouchPos +""+ _startTouchPosition);
        var swipeDirection = endTouchPos - _startTouchPosition;
        if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.z))
        {
            _carDirection = swipeDirection.x >0 ? Vector3.right : Vector3.left;
        }
        else
        {
            _carDirection = swipeDirection.z >0 ? new Vector3(0,0,1) : new Vector3(0,0,-1);
        }
        selectedCar.Movement(_carDirection);
        selectedCar = null;
    }
    
}
