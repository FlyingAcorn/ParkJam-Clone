using UnityEngine;

public class CarMovement : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Movement(Vector3 swipeDirection)
    {
        var angleOfDir = (Mathf.FloorToInt(Mathf.Atan2(swipeDirection.x, swipeDirection.z)*Mathf.Rad2Deg));
        if (angleOfDir == -90) angleOfDir = 270;
        var angleOfCar = Mathf.FloorToInt(transform.eulerAngles.y);
        Debug.Log(angleOfDir+" "+ angleOfCar);
        if (angleOfCar == angleOfDir || angleOfCar == angleOfDir + 180 || angleOfCar == angleOfDir - 180)
        {
            Debug.Log(swipeDirection+"Deneme");
            //start to movement here (via using bool and another method or on here)
        }
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider trigger)
    {
        
    }
}
