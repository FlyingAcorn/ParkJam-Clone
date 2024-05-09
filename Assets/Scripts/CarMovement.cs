using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
     private bool _stopMovement;
     [SerializeField] private float speed;
     private Coroutine _movementCoroutine;
     private Vector3 _swipeDirection;
     private float _interpolationAmount;

     public float InterpolationAmount
     {
         get => _interpolationAmount;
         set
         {
             if (value ==1)
             {
                 _interpolationAmount = 0;
             }
             else
             {
                 _interpolationAmount = value;
             }
         }
     }
     private bool _isMovingReverse;
    
    public void MovementController(Vector3 swipeDirection)
    {
        if (_movementCoroutine != null) return;
        _swipeDirection = swipeDirection;
        var angleOfDir = (Mathf.FloorToInt(Mathf.Atan2(swipeDirection.x, swipeDirection.z)*Mathf.Rad2Deg));
        if (angleOfDir == -90) angleOfDir = 270;
        var angleOfCar = Mathf.FloorToInt(transform.eulerAngles.y);
        if (angleOfCar != angleOfDir && angleOfCar != angleOfDir + 180 && angleOfCar != angleOfDir - 180) return;
        _isMovingReverse = angleOfDir != angleOfCar;
        _movementCoroutine = StartCoroutine(Movement());
    }

    private IEnumerator Movement()
    {
        _stopMovement = false;
        while (!_stopMovement)
        {
            transform.position += _swipeDirection * (speed * Time.deltaTime);
            yield return null;
        }
        _movementCoroutine = null;
    }

    private IEnumerator RoadMovement()
    {
        //initial part.
        var startPoint = transform.position;
        var middlePoint = transform.position + _swipeDirection * 3.75f;
        var endpoint = !_isMovingReverse ? 
            middlePoint + (Quaternion.Euler(0, 90, 0) * _swipeDirection * 2) :
            middlePoint + (Quaternion.Euler(0, -90, 0) * _swipeDirection * 2);
        while (transform.position !=endpoint)
        {
            InterpolationAmount += Time.deltaTime % 1f;
            var lerp=TransformExtensions.QuadraticLerp(startPoint, middlePoint, endpoint, InterpolationAmount);
            transform.LookAt(!_isMovingReverse ? lerp: 2*transform.position-lerp);
            transform.position = lerp;
            
            yield return null;
        }
        
        
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.TryGetComponent(out CarMovement _) &&
            !collision.transform.TryGetComponent(out Obstacle _)) return;
        _stopMovement = true;
        collision.transform.DOPunchPosition(_swipeDirection*0.5f, 0.5f, 1);
        transform.Translate(-_swipeDirection*0.5f,Space.World);

    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.transform.TryGetComponent(out Road road))
        {
            _stopMovement = true;
            _movementCoroutine = StartCoroutine(RoadMovement());
        }
    }
}
