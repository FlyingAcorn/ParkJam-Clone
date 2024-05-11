using System;
using System.Collections;
using System.Net.Http.Headers;
using DG.Tweening;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    private bool _stopMovement = true;
     [SerializeField] private float speed;
     private Coroutine _movementCoroutine;
     private Vector3 _swipeDirection;
     private float _interpolationAmount;
     private RoadManager _road;
     private bool _isMovingReverse;
    
    public void MovementController(Vector3 swipeDirection)
    {
        if (_movementCoroutine != null) return;
        _swipeDirection = swipeDirection;
        var angleOfDir = (Mathf.FloorToInt(Mathf.Atan2(swipeDirection.x, swipeDirection.z)*Mathf.Rad2Deg));
        if (angleOfDir == -90) angleOfDir = 270;
        Debug.Log(angleOfDir);
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

    private IEnumerator RoadMovement(int idx)
    {
        var roadPoints = _road.roadPoints;
        //Arranging car to the road.
        var startPoint = transform.position;
        bool region = roadPoints[idx].transform.eulerAngles.y == 90 || roadPoints[idx].transform.eulerAngles.y == 270;
        var middlePoint = region ? 
            new Vector3(roadPoints[idx].startPoint.position.x, 0, transform.position.z) 
            :
            new Vector3(transform.position.x, 0, roadPoints[idx].startPoint.position.z);
        var endpoint = !_isMovingReverse ? 
            middlePoint + (Quaternion.Euler(0, 90, 0) * _swipeDirection * 2) :
            middlePoint + (Quaternion.Euler(0, -90, 0) * _swipeDirection * 2);
        while (transform.position !=endpoint)
        {
            _interpolationAmount += Time.deltaTime % 1f;
            var lerp=TransformExtensions.QuadraticLerp(startPoint, middlePoint, endpoint, _interpolationAmount);
            transform.LookAt(!_isMovingReverse ? lerp: 2*transform.position-lerp);
            transform.position = lerp;
            yield return null;
        }
        
        //Linear movement for each side
        for (int i = idx; i < roadPoints.Length; i++)
        {
            _interpolationAmount = 0;
            startPoint = transform.position;
            while (transform.position != roadPoints[i].endPoint.position)
            {
                _interpolationAmount += Time.deltaTime % 1f;
                var lerp = Vector3.Lerp(startPoint, roadPoints[i].endPoint.position,
                    _interpolationAmount);
                transform.LookAt(lerp);
                transform.position = lerp;
                yield return null;

            }
            if (i+1 != roadPoints.Length)
            {
                //Turning part.
                startPoint = transform.position;
                _interpolationAmount = 0;
                //Using trigonometry regions to arrange vector points.
                 region = roadPoints[i].transform.eulerAngles.y == 90 || roadPoints[i].transform.eulerAngles.y == 270;
                var intersectionPoint = region ? 
                    new Vector3(roadPoints[i].endPoint.position.x, 0, roadPoints[i + 1].startPoint.position.z) 
                    :
                    new Vector3(roadPoints[i+1].startPoint.position.x, 0, roadPoints[i].endPoint.position.z);
                
                while (transform.position != roadPoints[i+1].startPoint.position)
                {
                    _interpolationAmount += Time.deltaTime % 1f;
                    var lerp =TransformExtensions.QuadraticLerp(startPoint, intersectionPoint,
                        roadPoints[i + 1].startPoint.position, _interpolationAmount);
                    transform.LookAt(lerp);
                    transform.position=lerp;
                    yield return null;
                }
            }
        }
        
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.TryGetComponent(out CarMovement _) &&
            !collision.transform.TryGetComponent(out Obstacle _)) return;
        if (!_stopMovement)
        {
            var region = transform.eulerAngles.y == 90 || transform.eulerAngles.y == 270;
            transform.Translate(-_swipeDirection*0.5f,Space.World);
            collision.transform.DOPunchRotation((region ?  new Vector3(_swipeDirection.z,0,_swipeDirection.x) : _swipeDirection) * 20, 4,1);
            transform.DOPunchRotation((region ? _swipeDirection  : new Vector3(_swipeDirection.z,0,_swipeDirection.x)) * 20, 4,1);
        }
        _stopMovement = true;
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.transform.TryGetComponent(out Road road))
        {
            _road = road.GetComponentInParent<RoadManager>();
           var idx= Array.IndexOf(_road.roadPoints, road);
            _stopMovement = true;
            _movementCoroutine = StartCoroutine(RoadMovement(idx));
        }
    }
    
}
